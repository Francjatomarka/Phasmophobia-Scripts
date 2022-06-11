using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x0200011C RID: 284
public class ParabolicMicrophone : MonoBehaviour
{
	// Token: 0x06000806 RID: 2054 RVA: 0x00030C23 File Offset: 0x0002EE23
	private void Awake()
	{
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
		this.screenText.gameObject.SetActive(false);
		this.rend.material.DisableKeyword("_EMISSION");
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00030C63 File Offset: 0x0002EE63
	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00030C7C File Offset: 0x0002EE7C
	private void Use()
	{
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("UseNetworked", RpcTarget.All, Array.Empty<object>());
			return;
		}
		UseNetworked();
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x00030C94 File Offset: 0x0002EE94
	[PunRPC]
	private void UseNetworked()
	{
		this.isOn = !this.isOn;
		this.screenText.text = "00.0";
		this.screenText.gameObject.SetActive(this.isOn);
		if (this.isOn)
		{
			this.rend.material.EnableKeyword("_EMISSION");
			return;
		}
		this.rend.material.DisableKeyword("_EMISSION");
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00030D0C File Offset: 0x0002EF0C
	private void Update()
	{
		if (this.isOn)
		{
			this.checkTimer -= Time.deltaTime;
			if (this.checkTimer < 0f)
			{
				if(this.noises.Count > 0)
                {
					for (int i = 0; i < this.noises.Count; i++)
					{
						this.volume += this.noises[i].volume;
					}
				}
				this.screenText.text = (this.volume * 10f).ToString("00.0");
				this.noises.Clear();
				this.volume = 0f;
				base.StartCoroutine(this.ResetTrigger());
				this.checkTimer = UnityEngine.Random.Range(1f, 2f);
			}
		}
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x00030DD6 File Offset: 0x0002EFD6
	private IEnumerator ResetTrigger()
	{
		this.col.enabled = false;
		yield return 0;
		this.col.enabled = true;
		yield break;
	}

	// Token: 0x04000801 RID: 2049
	[HideInInspector]
	public bool isOn;

	// Token: 0x04000802 RID: 2050
	public List<Noise> noises = new List<Noise>();

	// Token: 0x04000803 RID: 2051
	private float volume;

	// Token: 0x04000804 RID: 2052
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000805 RID: 2053
	private PhotonView view;

	// Token: 0x04000806 RID: 2054
	private float checkTimer = 2f;

	// Token: 0x04000807 RID: 2055
	[SerializeField]
	private BoxCollider col;

	// Token: 0x04000808 RID: 2056
	[SerializeField]
	private Text screenText;

	// Token: 0x04000809 RID: 2057
	[SerializeField]
	private Renderer rend;
}
