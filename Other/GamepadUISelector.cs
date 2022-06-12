using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class GamepadUISelector : MonoBehaviour
{
	private void OnEnable()
	{
		if (this.startOnEnable)
		{
			if (this.delaySelection)
			{
				base.Invoke("SetSelection", 1f);
				return;
			}
			this.SetSelection();
		}
	}

	public void SetSelection()
	{
		GamepadUISelector.instance = this;
		if (XRDevice.isPresent)
		{
			return;
		}
		if (this.eventSystem)
		{
			this.eventSystem.SetSelectedGameObject(null);
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		if (this.selectByDefault)
		{
			if (this.eventSystem)
			{
				this.eventSystem.SetSelectedGameObject(this.selectedObject);
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(this.selectedObject);
			}
		}
		if (MainManager.instance)
		{
			if (MainManager.instance.localPlayer && MainManager.instance.localPlayer.playerInput.currentControlScheme == "Gamepad")
			{
				EventSystem.current.SetSelectedGameObject(this.selectedObject);
			}
		}
		else if (GameController.instance && GameController.instance.myPlayer != null && GameController.instance.myPlayer.player.playerInput.currentControlScheme == "Gamepad")
		{
			EventSystem.current.SetSelectedGameObject(this.selectedObject);
		}
		for (int i = 0; i < this.objectsToDisable.Length; i++)
		{
			this.objectsToDisable[i].SetActive(false);
		}
		for (int j = 0; j < this.objectsToEnable.Length; j++)
		{
			this.objectsToEnable[j].SetActive(true);
		}
	}

	public static GamepadUISelector instance;

	[SerializeField]
	private GameObject selectedObject;

	[SerializeField]
	private GameObject[] objectsToDisable;

	[SerializeField]
	private GameObject[] objectsToEnable;

	[SerializeField]
	private bool startOnEnable = true;

	[SerializeField]
	private EventSystem eventSystem;

	[SerializeField]
	private bool selectByDefault;

	[SerializeField]
	private bool delaySelection;
}

