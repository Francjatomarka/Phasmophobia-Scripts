using System;
using UnityEngine;
using UnityEngine.XR;

public class ControlsObjectSwitcher : MonoBehaviour
{
	private void OnEnable()
	{
		this.keyboardObj.SetActive(false);
		this.controllerObj.SetActive(false);
		if (!XRDevice.isPresent)
		{
			if (MainManager.instance)
			{
				if (MainManager.instance.localPlayer.playerInput.currentControlScheme == "Keyboard")
				{
					this.keyboardObj.SetActive(true);
					return;
				}
				this.controllerObj.SetActive(true);
				return;
			}
			else if (GameController.instance)
			{
				if (GameController.instance.myPlayer.player.playerInput.currentControlScheme == "Keyboard")
				{
					this.keyboardObj.SetActive(true);
					return;
				}
				this.controllerObj.SetActive(true);
			}
		}
	}

	private void OnDisable()
	{
		this.keyboardObj.SetActive(false);
		this.controllerObj.SetActive(false);
	}

	[SerializeField]
	private GameObject keyboardObj;

	[SerializeField]
	private GameObject controllerObj;
}

