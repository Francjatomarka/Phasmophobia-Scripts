using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x02000126 RID: 294
public class TrainingRemote : MonoBehaviour
{
	// Token: 0x06000840 RID: 2112 RVA: 0x0003237C File Offset: 0x0003057C
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.source = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x000323A2 File Offset: 0x000305A2
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x000323BB File Offset: 0x000305BB
	private void Use()
	{
		TrainingController.instance.NextSlide();
		this.source.Play();
	}

	// Token: 0x04000847 RID: 2119
	private PhotonView view;

	// Token: 0x04000848 RID: 2120
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000849 RID: 2121
	private AudioSource source;
}
