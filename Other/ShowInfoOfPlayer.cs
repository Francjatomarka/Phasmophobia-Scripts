using System;
using Photon;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class ShowInfoOfPlayer : MonoBehaviourPunCallbacks
{
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

	private GameObject textGo;

	private TextMesh tm;

	public float CharacterSize;

	public Font font;

	public bool DisableOnOwnObjects;
}

