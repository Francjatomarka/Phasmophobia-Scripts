using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Photon.Pun;

public class PCCrouch : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.startPos = this.player.headObject.transform.localPosition;
		this.crouchPos = new Vector3(this.startPos.x, 0f, this.startPos.z);
	}

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

	[PunRPC]
	private void NetworkedCrouch(bool isCrouched)
	{
		this.player.headObject.transform.localPosition = (isCrouched ? this.crouchPos : this.startPos);
	}

	public void OnCrouch(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started && !XRDevice.isPresent)
		{
			this.Crouch();
		}
	}

	private Vector3 startPos;

	private Vector3 crouchPos;

	private PhotonView view;

	[SerializeField]
	private Player player;

	[HideInInspector]
	public bool isCrouched;
}

