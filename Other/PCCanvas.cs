using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;

// Token: 0x02000162 RID: 354
public class PCCanvas : MonoBehaviour
{
	// Token: 0x06000A01 RID: 2561 RVA: 0x0003D109 File Offset: 0x0003B309
	private void Start()
	{
		this.UpdateCursorBrightness();
		this.SetState(PCCanvas.State.none, false);
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x0003D11C File Offset: 0x0003B31C
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

	// Token: 0x06000A03 RID: 2563 RVA: 0x0003D1A4 File Offset: 0x0003B3A4
	public void UpdateCursorBrightness()
	{
		this.defaultColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(136));
		this.activeColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255));
		this.SetState(this.state, true);
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x0003D244 File Offset: 0x0003B444
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

	// Token: 0x06000A05 RID: 2565 RVA: 0x0003D3AD File Offset: 0x0003B5AD
	public void LoadingGame()
	{
		this.loadingFadeToBlackImage.gameObject.SetActive(true);
		this.crosshair.gameObject.SetActive(false);
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x0003D3D4 File Offset: 0x0003B5D4
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

	// Token: 0x06000A07 RID: 2567 RVA: 0x0003D4AF File Offset: 0x0003B6AF
	public void OnPause(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started && !XRDevice.isPresent)
		{
			this.Pause();
		}
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x0003D4C8 File Offset: 0x0003B6C8
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

	// Token: 0x04000A37 RID: 2615
	public Image crosshair;

	// Token: 0x04000A38 RID: 2616
	[SerializeField]
	private Image loadingFadeToBlackImage;

	// Token: 0x04000A39 RID: 2617
	[SerializeField]
	private Sprite normalSprite;

	// Token: 0x04000A3A RID: 2618
	[SerializeField]
	private Sprite lockSprite;

	// Token: 0x04000A3B RID: 2619
	[SerializeField]
	private Sprite lightSprite;

	// Token: 0x04000A3C RID: 2620
	[SerializeField]
	private Player player;

	// Token: 0x04000A3D RID: 2621
	[HideInInspector]
	public bool isPaused;

	// Token: 0x04000A3E RID: 2622
	private Color32 defaultColour;

	// Token: 0x04000A3F RID: 2623
	private Color32 activeColour;

	// Token: 0x04000A40 RID: 2624
	public GameObject canvas;

	// Token: 0x04000A41 RID: 2625
	private Vector3 normalScale = new Vector3(0.08f, 0.08f, 0.08f);

	// Token: 0x04000A42 RID: 2626
	private string currentControlScheme;

	// Token: 0x04000A43 RID: 2627
	public PCCanvas.State state;

	// Token: 0x020004EC RID: 1260
	public enum State
	{
		// Token: 0x0400233D RID: 9021
		none,
		// Token: 0x0400233E RID: 9022
		active,
		// Token: 0x0400233F RID: 9023
		locked,
		// Token: 0x04002340 RID: 9024
		light
	}
}
