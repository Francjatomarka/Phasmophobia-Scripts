using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

// Token: 0x02000177 RID: 375
public class ControlsManager : MonoBehaviour
{
	// Token: 0x060009FC RID: 2556 RVA: 0x0003DF8C File Offset: 0x0003C18C
	private void Start()
	{
		
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x0003E048 File Offset: 0x0003C248
	private void Update()
	{
		if (!XRDevice.isPresent && MainManager.instance && MainManager.instance.localPlayer && MainManager.instance.localPlayer.playerInput && MainManager.instance.localPlayer.playerInput.currentControlScheme != this.currentControlScheme)
		{
			this.currentControlScheme = MainManager.instance.localPlayer.playerInput.currentControlScheme;
			this.OnControlSchemeChanged();
		}
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x0003E0D0 File Offset: 0x0003C2D0
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

	// Token: 0x060009FF RID: 2559 RVA: 0x0003E12A File Offset: 0x0003C32A
	public void LoadControls()
	{
		this.controlsImage.gameObject.SetActive(false);
		this.pcControls.SetActive(true);
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x0003E149 File Offset: 0x0003C349
	public void ControlsButton()
	{
		if (!XRDevice.isPresent)
		{
			MainManager.instance.localPlayer.playerInput.DeactivateInput();
		}
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x0003E166 File Offset: 0x0003C366
	public void ResetButton()
	{
		
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x0003E183 File Offset: 0x0003C383
	public void BackButton()
	{
		if (!XRDevice.isPresent)
		{
			MainManager.instance.localPlayer.playerInput.ActivateInput();
			MainManager.instance.localPlayer.pcControls.StoreControlOverrides();
		}
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x0003E1B4 File Offset: 0x0003C3B4
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

	// Token: 0x06000A04 RID: 2564 RVA: 0x0003E229 File Offset: 0x0003C429
	private void OnDisable()
	{
		this.keyboardObj.SetActive(false);
		this.controllerObj.SetActive(false);
		if (!XRDevice.isPresent)
		{
			MainManager.instance.localPlayer.playerInput.ActivateInput();
		}
	}

	// Token: 0x04000A2E RID: 2606
	[SerializeField]
	private Image controlsImage;

	// Token: 0x04000A2F RID: 2607
	[SerializeField]
	private GameObject pcControls;

	// Token: 0x04000A30 RID: 2608
	[SerializeField]
	private Sprite oculusRiftSprite;

	// Token: 0x04000A31 RID: 2609
	[SerializeField]
	private Sprite htcViveSprite;

	// Token: 0x04000A32 RID: 2610
	[SerializeField]
	private GameObject keyboardObj;

	// Token: 0x04000A33 RID: 2611
	[SerializeField]
	private GameObject controllerObj;

	// Token: 0x04000A34 RID: 2612
	private string currentControlScheme;

	// Token: 0x04000A35 RID: 2613
	[SerializeField]
	private Button resetButton;
}
