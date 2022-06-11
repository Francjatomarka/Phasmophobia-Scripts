using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

// Token: 0x020000F3 RID: 243
public class GamepadUISelector : MonoBehaviour
{
	// Token: 0x060006A5 RID: 1701 RVA: 0x000273F8 File Offset: 0x000255F8
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

	// Token: 0x060006A6 RID: 1702 RVA: 0x00027424 File Offset: 0x00025624
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

	// Token: 0x040006C3 RID: 1731
	public static GamepadUISelector instance;

	// Token: 0x040006C4 RID: 1732
	[SerializeField]
	private GameObject selectedObject;

	// Token: 0x040006C5 RID: 1733
	[SerializeField]
	private GameObject[] objectsToDisable;

	// Token: 0x040006C6 RID: 1734
	[SerializeField]
	private GameObject[] objectsToEnable;

	// Token: 0x040006C7 RID: 1735
	[SerializeField]
	private bool startOnEnable = true;

	// Token: 0x040006C8 RID: 1736
	[SerializeField]
	private EventSystem eventSystem;

	// Token: 0x040006C9 RID: 1737
	[SerializeField]
	private bool selectByDefault;

	// Token: 0x040006CA RID: 1738
	[SerializeField]
	private bool delaySelection;
}
