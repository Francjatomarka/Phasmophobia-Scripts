using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ControlsManager : MonoBehaviour
{
	private void Start()
	{
		
	}

	private void Update()
	{
		if (!XRDevice.isPresent && MainManager.instance && MainManager.instance.localPlayer && MainManager.instance.localPlayer.playerInput && MainManager.instance.localPlayer.playerInput.currentControlScheme != this.currentControlScheme)
		{
			this.currentControlScheme = MainManager.instance.localPlayer.playerInput.currentControlScheme;
			this.OnControlSchemeChanged();
		}
	}

	public void OnControlSchemeChanged()
	{
		if (this.currentControlScheme == "Keyboard")
		{
			this.keyboardObj.SetActive(true);
			this.controllerObj.SetActive(false);
			return;
		}
		this.keyboardObj.SetActive(false);
		this.controllerObj.SetActive(true);
		GamepadUISelector.instance.SetSelection();
	}

	public void LoadControls()
	{
		this.controlsImage.gameObject.SetActive(false);
		this.pcControls.SetActive(true);
	}

	public void ControlsButton()
	{
		if (!XRDevice.isPresent)
		{
			MainManager.instance.localPlayer.playerInput.DeactivateInput();
		}
	}

	public void ResetButton()
	{
		
	}

	public void BackButton()
	{
		if (!XRDevice.isPresent)
		{
			MainManager.instance.localPlayer.playerInput.ActivateInput();
			MainManager.instance.localPlayer.pcControls.StoreControlOverrides();
		}
	}

	private void OnEnable()
	{
		if (!XRDevice.isPresent)
		{
			if (MainManager.instance.localPlayer.playerInput.currentControlScheme == "Keyboard")
			{
				MainManager.instance.localPlayer.playerInput.DeactivateInput();
				this.keyboardObj.SetActive(true);
				return;
			}
			MainManager.instance.localPlayer.playerInput.DeactivateInput();
			this.controllerObj.SetActive(true);
		}
	}

	private void OnDisable()
	{
		this.keyboardObj.SetActive(false);
		this.controllerObj.SetActive(false);
		if (!XRDevice.isPresent)
		{
			MainManager.instance.localPlayer.playerInput.ActivateInput();
		}
	}

	[SerializeField]
	private Image controlsImage;

	[SerializeField]
	private GameObject pcControls;

	[SerializeField]
	private Sprite oculusRiftSprite;

	[SerializeField]
	private Sprite htcViveSprite;

	[SerializeField]
	private GameObject keyboardObj;

	[SerializeField]
	private GameObject controllerObj;

	private string currentControlScheme;

	[SerializeField]
	private Button resetButton;
}

