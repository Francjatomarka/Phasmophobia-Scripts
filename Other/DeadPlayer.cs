using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class DeadPlayer : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.source = base.GetComponent<AudioSource>();
	}

	private void Start()
	{
		if (GameController.instance != null)
		{
			GameController.instance.OnExitLevel.AddListener(new UnityAction(this.DestroyDeadPlayer));
		}
	}

	public void Spawn(int modelID, int actorID)
	{
		this.view.RPC("SpawnBody", RpcTarget.All, new object[]
		{
			modelID
		});
		this.view.RPC("PlaySound", RpcTarget.All, new object[]
		{
			actorID
		});
	}

	[PunRPC]
	private void SpawnBody(int modelID)
	{
		this.characterModels[modelID].SetActive(true);
		base.StartCoroutine(this.EnableRagdoll());
	}

	private IEnumerator EnableRagdoll()
	{
		yield return new WaitForSeconds(4f);
		for (int i = 0; i < this.ragdollAnims.Length; i++)
		{
			this.ragdollAnims[i].enabled = false;
		}
		for (int j = 0; j < this.ragdollColliders.Length; j++)
		{
			if (this.ragdollColliders[j].gameObject.activeInHierarchy)
			{
				this.ragdollColliders[j].attachedRigidbody.velocity = Vector3.zero;
			}
		}
		for (int k = 0; k < this.ragdollColliders.Length; k++)
		{
			if (this.ragdollColliders[k].gameObject.activeInHierarchy)
			{
				this.ragdollColliders[k].attachedRigidbody.isKinematic = false;
			}
		}
		yield return new WaitForSeconds(6f);
		for (int l = 0; l < this.ragdollColliders.Length; l++)
		{
			if (this.ragdollColliders[l].gameObject.activeInHierarchy)
			{
				this.ragdollColliders[l].attachedRigidbody.velocity = Vector3.zero;
				this.ragdollColliders[l].attachedRigidbody.useGravity = false;
				this.ragdollColliders[l].attachedRigidbody.isKinematic = true;
				this.ragdollColliders[l].enabled = false;
			}
		}
		yield break;
	}

	[PunRPC]
	private void PlaySound(int actorID)
	{
		bool flag = false;
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (GameController.instance.playersData[i].actorID == actorID)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			this.source.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		}
		this.source.Play();
	}

	private void DestroyDeadPlayer()
	{
		if (this.view.IsMine)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
	}

	[SerializeField]
	private GameObject[] characterModels;

	private PhotonView view;

	private AudioSource source;

	[SerializeField]
	private Collider[] ragdollColliders;

	[SerializeField]
	private Animator[] ragdollAnims;
}

