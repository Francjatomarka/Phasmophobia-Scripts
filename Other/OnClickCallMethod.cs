using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OnClickCallMethod : MonoBehaviourPunCallbacks
{
	public void OnClick()
	{
		if (this.TargetGameObject == null || string.IsNullOrEmpty(this.TargetMethod))
		{
			Debug.LogWarning(this + " can't call, cause GO or Method are empty.");
			return;
		}
		this.TargetGameObject.SendMessage(this.TargetMethod);
	}

	public GameObject TargetGameObject;

	public string TargetMethod;
}

