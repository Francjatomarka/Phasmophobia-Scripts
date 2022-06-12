using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;

public class PCCanvas : MonoBehaviour
{
	private void Start()
	{
		this.UpdateCursorBrightness();
		this.SetState(PCCanvas.State.none, false);
	}

	private void Update()
	{
		if (MainManager.instance)
		{
			return;
		}
		if ((this.isPaused || LevelController.instance.journalController.isOpen) && this.player.playerInput.currentControlScheme != this.currentControlScheme)
		{
			if (this.player.view.IsMine || !PhotonNetwork.InRoom)
			{
				this.OnControlSchemeChanged();
			}
			this.currentControlScheme = this.player.playerInput.currentControlScheme;
		}
	}

	public void UpdateCursorBrightness()
	{
		this.defaultColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(136));
		this.activeColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255));
		this.SetState(this.state, true);
	}

	public void SetState(PCCanvas.State state, [Optional] bool forceState)
	{
		if (!forceState && state == this.state)
		{
			return;
		}
		this.state = state;
		switch (state)
		{
		case PCCanvas.State.none:
			this.crosshair.enabled = true;
			this.crosshair.color = this.defaultColour;
			this.crosshair.sprite = this.normalSprite;
			this.crosshair.rectTransform.localScale = this.normalScale;
			return;
		case PCCanvas.State.active:
			this.crosshair.enabled = true;
			this.crosshair.color = this.activeColour;
			this.crosshair.sprite = this.normalSprite;
			this.crosshair.rectTransform.localScale = this.normalScale;
			return;
		case PCCanvas.State.locked:
			this.crosshair.enabled = true;
			this.crosshair.color = this.activeColour;
			this.crosshair.sprite = this.lockSprite;
			this.crosshair.rectTransform.localScale = this.normalScale;
			return;
		case PCCanvas.State.light:
			this.crosshair.enabled = true;
			this.crosshair.color = this.activeColour;
			this.crosshair.sprite = this.lightSprite;
			this.crosshair.rectTransform.localScale = this.normalScale * 1.5f;
			return;
		default:
			return;
		}
	}

	public void LoadingGame()
	{
		this.loadingFadeToBlackImage.gameObject.SetActive(true);
		this.crosshair.gameObject.SetActive(false);
	}

	public void Pause()
	{
		if (GameController.instance == null)
		{
			return;
		}
		if (LevelController.instance.journalController.isOpen)
		{
			LevelController.instance.journalController.CloseJournal();
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			return;
		}
		this.isPaused = !this.isPaused;
		PauseMenuController.instance.Pause(this.isPaused);
		base.GetComponent<FirstPersonController>().enabled = !this.isPaused;
		if (this.isPaused)
		{
			this.player.charAnim.SetFloat("speed", 0f);
			if (GameController.instance.myPlayer.player.playerInput.currentControlScheme == "Keyboard")
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				return;
			}
		}
		else
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	public void OnPause(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started && !XRDevice.isPresent)
		{
			this.Pause();
		}
	}

	public void OnControlSchemeChanged()
	{
		if (XRDevice.isPresent)
		{
			return;
		}
		if (MainManager.instance)
		{
			return;
		}
		if (this.isPaused || LevelController.instance.journalController.isOpen)
		{
			if (GameController.instance.myPlayer.player.playerInput.currentControlScheme == "Keyboard")
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				return;
			}
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			GamepadUISelector.instance.SetSelection();
		}
	}

	public Image crosshair;

	[SerializeField]
	private Image loadingFadeToBlackImage;

	[SerializeField]
	private Sprite normalSprite;

	[SerializeField]
	private Sprite lockSprite;

	[SerializeField]
	private Sprite lightSprite;

	[SerializeField]
	private Player player;

	[HideInInspector]
	public bool isPaused;

	private Color32 defaultColour;

	private Color32 activeColour;

	public GameObject canvas;

	private Vector3 normalScale = new Vector3(0.08f, 0.08f, 0.08f);

	private string currentControlScheme;

	public PCCanvas.State state;

	public enum State
	{
		none,
		active,
		locked,
		light
	}
}

