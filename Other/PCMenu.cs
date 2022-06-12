using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Photon.Pun;

public class PCMenu : MonoBehaviour
{
	private void Awake()
	{
		this.isOnMenu = false;
	}

	private void Start()
	{
		
		if (!MainManager.instance)
		{
			base.enabled = false;
			return;
		}
		base.StartCoroutine(this.DisableUIDelay());
	}

	private IEnumerator DisableUIDelay()
	{
		yield return new WaitUntil(() => EventSystem.current);
		yield return new WaitUntil(() => EventSystem.current.currentInputModule);
		EventSystem.current.currentInputModule.enabled = false;
		yield break;
	}

	public void ForceIntoMenu()
	{
		this.OpenMenu();
		base.StopAllCoroutines();
		base.StartCoroutine(this.EnableInputDelay());
	}

	private IEnumerator EnableInputDelay()
	{
		yield return new WaitUntil(() => EventSystem.current);
		yield return new WaitUntil(() => EventSystem.current.currentInputModule);
		EventSystem.current.currentInputModule.enabled = true;
		yield break;
	}

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

	public void OnPause()
	{
		this.LeaveMenu();
	}

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

	[HideInInspector]
	public bool isOnMenu;

	[SerializeField]
	private Player player;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private GameObject firstSelectedButton;

	private string currentControlScheme;
}

