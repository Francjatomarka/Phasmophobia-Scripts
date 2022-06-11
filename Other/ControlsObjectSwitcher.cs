using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020000F1 RID: 241
public class ControlsObjectSwitcher : MonoBehaviour
{
	// Token: 0x0600069E RID: 1694 RVA: 0x000272BC File Offset: 0x000254BC
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

	// Token: 0x0600069F RID: 1695 RVA: 0x0002737B File Offset: 0x0002557B
	private void OnDisable()
	{
		this.keyboardObj.SetActive(false);
		this.controllerObj.SetActive(false);
	}

	// Token: 0x040006C1 RID: 1729
	[SerializeField]
	private GameObject keyboardObj;

	// Token: 0x040006C2 RID: 1730
	[SerializeField]
	private GameObject controllerObj;
}
