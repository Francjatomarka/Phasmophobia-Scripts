using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class SanityEffectsController : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	private void Update()
	{
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				this.AttemptToSpawnWindowGhost();
			}
			this.timer = UnityEngine.Random.Range(10f, (GameController.instance.myPlayer.player.insanity > 50f) ? 30f : 20f);
		}
	}

	private void AttemptToSpawnWindowGhost()
	{
		if (this.windows.Length == 0 || this.windowGhostObj == null)
		{
			return;
		}
		if (UnityEngine.Random.Range(0, 4) != 1)
		{
			return;
		}
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (!GameController.instance.playersData[i].player.isDead && GameController.instance.playersData[i].player.currentRoom == LevelController.instance.outsideRoom)
			{
				return;
			}
		}
		this.view.RPC("SpawnGhostNetworked", RpcTarget.All, new object[]
		{
			this.windows[UnityEngine.Random.Range(0, this.windows.Length)].view.ViewID
		});
	}

	[PunRPC]
	private void SpawnGhostNetworked(int windowViewId)
	{
		if (GameController.instance.myPlayer.player.isDead)
		{
			return;
		}
		base.StartCoroutine(this.SpawnGhostAtWindow(PhotonView.Find(windowViewId).GetComponent<Window>()));
	}

	private IEnumerator SpawnGhostAtWindow(Window window)
	{
		if (window.windowGhostStart == null)
		{
			yield return null;
		}
		this.windowGhostObj.transform.position = window.windowGhostStart.position;
		this.windowGhostObj.transform.rotation = window.windowGhostStart.rotation;
		this.windowGhostObj.SetActive(true);
		while (Vector3.Distance(this.windowGhostObj.transform.position, window.windowGhostEnd.position) > 0.2f)
		{
			this.windowGhostObj.transform.Translate(Vector3.forward * Time.deltaTime * 4f);
			yield return null;
		}
		this.windowGhostObj.SetActive(false);
		yield break;
	}

	private float timer = 30f;

	private Vector3 pos;

	private PhotonView view;

	[SerializeField]
	private Window[] windows;

	[SerializeField]
	private GameObject windowGhostObj;

	private GhostTraits.Type ghostType;
}

