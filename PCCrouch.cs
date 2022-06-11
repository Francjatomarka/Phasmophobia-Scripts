using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x02000164 RID: 356
public class PCCrouch : MonoBehaviour
{
	// Token: 0x06000A10 RID: 2576 RVA: 0x0003D814 File Offset: 0x0003BA14
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.startPos = this.player.headObject.transform.localPosition;
		this.crouchPos = new Vector3(this.startPos.x, 0f, this.startPos.z);
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x0003D870 File Offset: 0x0003BA70
	private void Crouch()
	{
		this.isCrouched = !this.isCrouched;
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("NetworkedCrouch", RpcTarget.OthersBuffered, new object[]
			{
				this.isCrouched
			});
		}
		base.StopCoroutine("CrouchAnim");
		base.StartCoroutine("CrouchAnim");
		if (this.player.charAnim)
		{
			this.player.charAnim.SetBool("isCrouched", this.isCrouched);
		}
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0003D8FC File Offset: 0x0003BAFC
	private IEnumerator CrouchAnim()
	{
		float elapsedTime = 0f;
		while (elapsedTime < 0.2f)
		{
			if (this.isCrouched)
			{
				this.player.headObject.transform.localPosition = Vector3.Lerp(this.startPos, this.crouchPos, elapsedTime / 0.2f);
			}
			else
			{
				this.player.headObject.transform.localPosition = Vector3.Lerp(this.crouchPos, this.startPos, elapsedTime / 0.2f);
			}
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		this.player.headObject.transform.localPosition = (this.isCrouched ? this.crouchPos : this.startPos);
		yield return null;
		yield break;
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0003D90B File Offset: 0x0003BB0B
	[PunRPC]
	private void NetworkedCrouch(bool isCrouched)
	{
		this.player.headObject.transform.localPosition = (isCrouched ? this.crouchPos : this.startPos);
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x0003D933 File Offset: 0x0003BB33
	public void OnCrouch(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started && !XRDevice.isPresent)
		{
			this.Crouch();
		}
	}

	// Token: 0x04000A46 RID: 2630
	private Vector3 startPos;

	// Token: 0x04000A47 RID: 2631
	private Vector3 crouchPos;

	// Token: 0x04000A48 RID: 2632
	private PhotonView view;

	// Token: 0x04000A49 RID: 2633
	[SerializeField]
	private Player player;

	// Token: 0x04000A4A RID: 2634
	[HideInInspector]
	public bool isCrouched;
}
