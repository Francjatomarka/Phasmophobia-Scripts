using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x0200016C RID: 364
public class DeadPlayer : MonoBehaviour
{
	// Token: 0x06000A54 RID: 2644 RVA: 0x0003FBAD File Offset: 0x0003DDAD
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.source = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x0003FBC7 File Offset: 0x0003DDC7
	private void Start()
	{
		if (GameController.instance != null)
		{
			GameController.instance.OnExitLevel.AddListener(new UnityAction(this.DestroyDeadPlayer));
		}
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x0003FBF0 File Offset: 0x0003DDF0
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

	// Token: 0x06000A57 RID: 2647 RVA: 0x0003FC3D File Offset: 0x0003DE3D
	[PunRPC]
	private void SpawnBody(int modelID)
	{
		this.characterModels[modelID].SetActive(true);
		base.StartCoroutine(this.EnableRagdoll());
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x0003FC5A File Offset: 0x0003DE5A
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

	// Token: 0x06000A59 RID: 2649 RVA: 0x0003FC6C File Offset: 0x0003DE6C
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

	// Token: 0x06000A5A RID: 2650 RVA: 0x0003FCE2 File Offset: 0x0003DEE2
	private void DestroyDeadPlayer()
	{
		if (this.view.IsMine)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000A78 RID: 2680
	[SerializeField]
	private GameObject[] characterModels;

	// Token: 0x04000A79 RID: 2681
	private PhotonView view;

	// Token: 0x04000A7A RID: 2682
	private AudioSource source;

	// Token: 0x04000A7B RID: 2683
	[SerializeField]
	private Collider[] ragdollColliders;

	// Token: 0x04000A7C RID: 2684
	[SerializeField]
	private Animator[] ragdollAnims;
}
