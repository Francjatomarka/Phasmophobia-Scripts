using System;
using Photon;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000048 RID: 72
[RequireComponent(typeof(PhotonView))]
public class ShowInfoOfPlayer : MonoBehaviourPunCallbacks
{
	// Token: 0x06000181 RID: 385 RVA: 0x0000AA8C File Offset: 0x00008C8C
	private void Start()
	{
		if (this.font == null)
		{
			this.font = (Font)Resources.FindObjectsOfTypeAll(typeof(Font))[0];
			Debug.LogWarning("No font defined. Found font: " + this.font);
		}
		if (this.tm == null)
		{
			this.textGo = new GameObject("3d text");
			this.textGo.transform.parent = base.gameObject.transform;
			this.textGo.transform.localPosition = Vector3.zero;
			this.textGo.AddComponent<MeshRenderer>().material = this.font.material;
			this.tm = this.textGo.AddComponent<TextMesh>();
			this.tm.font = this.font;
			this.tm.anchor = TextAnchor.MiddleCenter;
			if (this.CharacterSize > 0f)
			{
				this.tm.characterSize = this.CharacterSize;
			}
		}
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000AB90 File Offset: 0x00008D90
	private void Update()
	{
		bool flag = !this.DisableOnOwnObjects || base.photonView.IsMine;
		if (this.textGo != null)
		{
			this.textGo.SetActive(flag);
		}
		if (!flag)
		{
			return;
		}
		Photon.Realtime.Player owner = base.photonView.Owner;
		if (owner != null)
		{
			this.tm.text = (string.IsNullOrEmpty(owner.NickName) ? ("player" + owner.UserId) : owner.NickName);
			return;
		}
		if (base.photonView.IsSceneView)
		{
			this.tm.text = "scn";
			return;
		}
		this.tm.text = "n/a";
	}

	// Token: 0x040001BA RID: 442
	private GameObject textGo;

	// Token: 0x040001BB RID: 443
	private TextMesh tm;

	// Token: 0x040001BC RID: 444
	public float CharacterSize;

	// Token: 0x040001BD RID: 445
	public Font font;

	// Token: 0x040001BE RID: 446
	public bool DisableOnOwnObjects;
}
