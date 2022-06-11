using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x02000168 RID: 360
public class PCMenu : MonoBehaviour
{
	// Token: 0x06000A27 RID: 2599 RVA: 0x0003DF63 File Offset: 0x0003C163
	private void Awake()
	{
		this.isOnMenu = false;
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x0003DF6C File Offset: 0x0003C16C
	private void Start()
	{
		
		if (!MainManager.instance)
		{
			base.enabled = false;
			return;
		}
		base.StartCoroutine(this.DisableUIDelay());
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0003DF8F File Offset: 0x0003C18F
	private IEnumerator DisableUIDelay()
	{
		yield return new WaitUntil(() => EventSystem.current);
		yield return new WaitUntil(() => EventSystem.current.currentInputModule);
		EventSystem.current.currentInputModule.enabled = false;
		yield break;
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0003DF97 File Offset: 0x0003C197
	public void ForceIntoMenu()
	{
		this.OpenMenu();
		base.StopAllCoroutines();
		base.StartCoroutine(this.EnableInputDelay());
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0003DFB2 File Offset: 0x0003C1B2
	private IEnumerator EnableInputDelay()
	{
		yield return new WaitUntil(() => EventSystem.current);
		yield return new WaitUntil(() => EventSystem.current.currentInputModule);
		EventSystem.current.currentInputModule.enabled = true;
		yield break;
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0003DFBC File Offset: 0x0003C1BC
	public void LeaveMenu()
	{
		if (this.isOnMenu)
		{
			this.isOnMenu = false;
			this.player.cam.enabled = true;
			this.player.firstPersonController.enabled = true;
			MainManager.instance.sceneCamera.gameObject.SetActive(false);
			this.player.pcCanvas.crosshair.gameObject.SetActive(true);
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			MainManager.instance.serverManager.EnableOrDisablePlayerModels(true);
			EventSystem.current.currentInputModule.enabled = false;
		}
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x0003E078 File Offset: 0x0003C278
	public void OpenMenu()
	{
		this.isOnMenu = true;
		this.player.cam.enabled = false;
		Debug.Log("Disabled PCMenu:78");
		this.player.firstPersonController.enabled = false;
		if (this.player.charAnim != null)
		{
			this.player.charAnim.SetFloat("speed", 0f);
		}
		MainManager.instance.sceneCamera.gameObject.SetActive(true);
		this.player.pcCanvas.crosshair.gameObject.SetActive(false);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		MainManager.instance.serverManager.EnableOrDisablePlayerModels(false);
		base.StartCoroutine(this.EnableInputDelay());
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x0003E174 File Offset: 0x0003C374
	private void Update()
	{
		
		if (this.isOnMenu && this.player.playerInput.currentControlScheme != this.currentControlScheme)
		{
			if (this.player.view.IsMine || !PhotonNetwork.InRoom)
			{
				//this.OnControlSchemeChanged();
			}
			this.currentControlScheme = this.player.playerInput.currentControlScheme;
		}
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x0003E1DC File Offset: 0x0003C3DC
	public void OnControlSchemeChanged()
	{
		if (XRDevice.isPresent)
		{
			return;
		}
		if (!MainManager.instance)
		{
			return;
		}
		if (!this.isOnMenu)
		{
			return;
		}
		if (this.player.playerInput.currentControlScheme == "Keyboard")
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			return;
		}
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void OnInteract(InputAction.CallbackContext context)
    {
		Ray ray = this.player.cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		RaycastHit hit;
		if (context.phase == InputActionPhase.Canceled && !this.isOnMenu && Physics.Raycast(ray, out hit) && hit.transform.tag == "MainMenuUI")
		{
			this.OpenMenu();
		}
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x0003E2BE File Offset: 0x0003C4BE
	public void OnPause()
	{
		this.LeaveMenu();
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x0003E2BE File Offset: 0x0003C4BE
	public void OnDrop(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Canceled)
		{
			this.LeaveMenu();
		}
	}

	public void OnMenuSecondary(InputAction.CallbackContext context)
	{
		if (!(MainManager.instance == null) && context.phase == InputActionPhase.Canceled)
		{
			if (isOnMenu)
			{
				LeaveMenu();
			}
			else
			{
				OpenMenu();
			}
		}
	}

	// Token: 0x04000A5F RID: 2655
	[HideInInspector]
	public bool isOnMenu;

	// Token: 0x04000A60 RID: 2656
	[SerializeField]
	private Player player;

	// Token: 0x04000A61 RID: 2657
	[SerializeField]
	private LayerMask mask;

	// Token: 0x04000A62 RID: 2658
	[SerializeField]
	private GameObject firstSelectedButton;

	// Token: 0x04000A63 RID: 2659
	private string currentControlScheme;
}
