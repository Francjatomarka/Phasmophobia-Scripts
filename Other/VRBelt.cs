using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000184 RID: 388
public class VRBelt : MonoBehaviour
{
	// Token: 0x06000B0A RID: 2826 RVA: 0x00045D60 File Offset: 0x00043F60
	private void Start()
	{
		if (!PhotonNetwork.InRoom)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x00045D78 File Offset: 0x00043F78
	public void Update()
	{
		if (this.view.IsMine)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.playerCam.transform.position, Vector3.down, out raycastHit, 3.5f, this.mask, QueryTriggerInteraction.Ignore))
			{
				this.newPos = new Vector3(this.playerCam.transform.position.x, raycastHit.point.y + (this.playerCam.position.y - raycastHit.point.y) / 2f + 0.1f, this.playerCam.transform.position.z);
			}
			this.myQuat = base.transform.rotation;
			this.myEul = this.myQuat.eulerAngles;
			this.myEul.y = this.playerCam.rotation.eulerAngles.y;
			this.myQuat.eulerAngles = this.myEul;
			base.transform.position = Vector3.Lerp(base.transform.position, this.newPos, Time.deltaTime * 4f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.myQuat, Time.deltaTime * 4f);
		}
	}

	// Token: 0x04000B92 RID: 2962
	[SerializeField]
	private Transform playerCam;

	// Token: 0x04000B93 RID: 2963
	[SerializeField]
	private LayerMask mask;

	// Token: 0x04000B94 RID: 2964
	[SerializeField]
	private PhotonView view;

	// Token: 0x04000B95 RID: 2965
	private Quaternion myQuat;

	// Token: 0x04000B96 RID: 2966
	private Vector3 myEul;

	// Token: 0x04000B97 RID: 2967
	private Vector3 newPos;
}
