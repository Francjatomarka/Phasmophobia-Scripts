using System;
using UnityEngine;
using Photon.Pun;

public class VRBelt : MonoBehaviour
{
	private void Start()
	{
		if (!PhotonNetwork.InRoom)
		{
			base.gameObject.SetActive(false);
		}
	}

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

	[SerializeField]
	private Transform playerCam;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private PhotonView view;

	private Quaternion myQuat;

	private Vector3 myEul;

	private Vector3 newPos;
}

