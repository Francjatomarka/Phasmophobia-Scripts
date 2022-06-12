using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(AudioSource))]
public class HandCamera : MonoBehaviour
{
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	private void Awake()
	{
		this.noise.gameObject.SetActive(false);
		this.UpdateUIValue();
	}

	private void Update()
	{
		if (!this.canTakePhoto)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				this.canTakePhoto = true;
				this.timer = 3f;
			}
		}
	}

	private void OnDisable()
	{
		this.canTakePhoto = true;
		this.timer = 3f;
		if (this.flashLight)
		{
			this.flashLight.enabled = false;
		}
	}

	public void Use()
	{
		if (this.canTakePhoto)
		{
			if (PhotonNetwork.InRoom)
			{
				this.view.RPC("NetworkedUse", RpcTarget.All, Array.Empty<object>());
			}
			else
			{
				StartCoroutine(NetworkedUse());
			}
		}
	}

	[PunRPC]
	private IEnumerator NetworkedUse()
	{
		this.canTakePhoto = false;
		if (this.currentAmountOfPhotos > 0)
		{
			this.flashLight.enabled = true;
			base.StartCoroutine(this.PlayNoiseObject());
			this.source.Play();
			string evidenceName = "";
			int evidenceAmount = 0;
			this.currentAmountOfPhotos--;
			this.UpdateUIValue();
			bool flag = false;
			AddPhotoToJournal("Test", 1);
			if (this.view.IsMine || !PhotonNetwork.InRoom)
			{
				if(LevelController.instance != null)
                {
					Vector3 vector = this.cam.WorldToViewportPoint(LevelController.instance.currentGhost.raycastPoint.position);
					if (vector.x > 0f && vector.x < 1f && vector.y > 0f && vector.y < 1f && !Physics.Linecast(base.transform.position, LevelController.instance.currentGhost.raycastPoint.position, this.mask) && LevelController.instance.currentGhost.ghostIsAppeared)
					{
						flag = true;
						if (MissionCapturePhoto.instance)
						{
							MissionCapturePhoto.instance.CompleteMission();
						}
						if (LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Phantom)
						{
							LevelController.instance.currentGhost.UnAppear(true);
						}
						DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.PhotoOfGhost, 1);
					}
					if (flag)
					{
						for (int i = 0; i < EvidenceController.instance.evidenceInLevel.Count; i++)
						{
							if (EvidenceController.instance.evidenceInLevel[i] != null && !EvidenceController.instance.evidenceInLevel[i].hasAlreadyTakenPhoto && EvidenceController.instance.evidenceInLevel[i].EvidenceType == Evidence.Type.ghost)
							{
								evidenceAmount = EvidenceController.instance.evidenceInLevel[i].GetEvidenceAmount();
								evidenceName = EvidenceController.instance.evidenceInLevel[i].evidenceName;
								break;
							}
						}
					}
					else
					{
						for (int j = 0; j < EvidenceController.instance.evidenceInLevel.Count; j++)
						{
							if (EvidenceController.instance.evidenceInLevel[j] != null && EvidenceController.instance.evidenceInLevel[j].gameObject.activeInHierarchy && EvidenceController.instance.evidenceInLevel[j].isActiveAndEnabled && !EvidenceController.instance.evidenceInLevel[j].hasAlreadyTakenPhoto)
							{
								vector = this.cam.WorldToViewportPoint(EvidenceController.instance.evidenceInLevel[j].transform.position);
								if (vector.x > 0f && vector.x < 1f && vector.y > 0f && vector.y < 1f && !Physics.Linecast(base.transform.position, EvidenceController.instance.evidenceInLevel[j].transform.position, this.mask) && Vector3.Distance(base.transform.position, EvidenceController.instance.evidenceInLevel[j].transform.position) < 3f)
								{
									if (EvidenceController.instance.evidenceInLevel[j].EvidenceType != Evidence.Type.ghost || LevelController.instance.currentGhost.ghostIsAppeared)
									{
										evidenceAmount = EvidenceController.instance.evidenceInLevel[j].GetEvidenceAmount();
										if (MissionVictimName.instance && EvidenceController.instance.evidenceInLevel[j].showsGhostVictim)
										{
											MissionVictimName.instance.CompleteMission();
										}
										if (MissionDirtyWater.instance && EvidenceController.instance.evidenceInLevel[j].EvidenceType == Evidence.Type.dirtyWater)
										{
											MissionDirtyWater.instance.CompleteMission();
										}
										evidenceName = EvidenceController.instance.evidenceInLevel[j].evidenceName;
										break;
									}
									break;
								}
							}
						}
					}
					this.AddPhotoToJournal(evidenceName, evidenceAmount);
				}
			}
		}
		yield return new WaitForSeconds(0.125f);
		this.flashLight.enabled = false;
		yield break;
	}

	private void UpdateUIValue()
	{
		if (this.currentAmountOfPhotos >= 0)
		{
			this.photosValueText.text = this.currentAmountOfPhotos.ToString();
		}
	}

	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return new WaitForEndOfFrame();
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	private void AddPhotoToJournal(string evidenceName, int evidenceAmount)
	{
		RenderTexture renderTexture = new RenderTexture(384, 216, 24);
		this.cam.targetTexture = renderTexture;
		Texture2D texture2D = new Texture2D(384, 216, TextureFormat.RGB24, false);
		this.cam.Render();
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0f, 0f, 384f, 216f), 0, 0);
		texture2D.Apply();
		this.cam.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(renderTexture);
		if (PhotonNetwork.PlayerList.Length > 1)
		{
			this.view.RPC("AddPhoto", RpcTarget.All, new object[]
			{
				texture2D.GetRawTextureData(),
				evidenceName,
				evidenceAmount
			});
			return;
		}
		this.AddPhoto(texture2D.GetRawTextureData(), evidenceName, evidenceAmount);
	}

	[PunRPC]
	private void AddPhoto(byte[] sprBytes, string evidenceName, int evidenceAmount)
	{
		if(LevelController.instance != null)
        {
			for (int i = 0; i < LevelController.instance.journals.Count; i++)
			{
				LevelController.instance.journals[i].AddPhoto(sprBytes, evidenceName, evidenceAmount);
			}
		}
		Texture2D texture2D = new Texture2D(384, 216, TextureFormat.RGB24, false);
		texture2D.LoadRawTextureData(sprBytes);
		texture2D.Apply();
		this.screen.material.mainTexture = texture2D;
		this.screen.material.color = Color.white;
		this.screen.material.SetTexture("_EmissionMap", texture2D);
		this.screen.material.SetColor("_EmissionColor", Color.white);
		this.screen.material.EnableKeyword("_EMISSION");
		int num = PlayerPrefs.GetInt("SavedPhotosIndex");
		byte[] bytes = texture2D.EncodeToPNG();
		File.WriteAllBytes(string.Concat(new object[]
		{
			Application.dataPath,
			"/../SavedScreen",
			num,
			".png"
		}), bytes);
		num++;
		if (num > 5)
		{
			num = 0;
		}
		PlayerPrefs.SetInt("SavedPhotosIndex", num);
	}

	public Camera cam;

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private Light flashLight;

	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private PhotonObjectInteract photonInteract;

	public LayerMask mask;

	[SerializeField]
	private Noise noise;

	private float timer = 2f;

	[SerializeField]private bool canTakePhoto = true;

	private int currentAmountOfPhotos = 5;

	[SerializeField]
	private Text photosValueText;

	[SerializeField]
	private Renderer screen;
}

