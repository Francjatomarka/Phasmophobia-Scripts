using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000042 RID: 66
[RequireComponent(typeof(PhotonView))]
public class HighlightOwnedGameObj : MonoBehaviourPunCallbacks
{
	// Token: 0x06000173 RID: 371 RVA: 0x0000A77C File Offset: 0x0000897C
	private void Update()
	{
		if (base.photonView.IsMine)
		{
			if (this.markerTransform == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PointerPrefab);
				gameObject.transform.parent = base.gameObject.transform;
				this.markerTransform = gameObject.transform;
			}
			Vector3 position = base.gameObject.transform.position;
			this.markerTransform.position = new Vector3(position.x, position.y + this.Offset, position.z);
			this.markerTransform.rotation = Quaternion.identity;
			return;
		}
		if (this.markerTransform != null)
		{
			UnityEngine.Object.Destroy(this.markerTransform.gameObject);
			this.markerTransform = null;
		}
	}

	// Token: 0x040001B2 RID: 434
	public GameObject PointerPrefab;

	// Token: 0x040001B3 RID: 435
	public float Offset = 0.5f;

	// Token: 0x040001B4 RID: 436
	private Transform markerTransform;
}
