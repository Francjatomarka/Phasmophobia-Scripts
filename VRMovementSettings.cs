using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

// Token: 0x02000172 RID: 370
public class VRMovementSettings : MonoBehaviour
{
	// Token: 0x06000A93 RID: 2707 RVA: 0x00041A3B File Offset: 0x0003FC3B
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x00041A5C File Offset: 0x0003FC5C
	private void Start()
	{
		if (PhotonNetwork.InRoom)
		{
			if (this.view.IsMine)
			{
				base.Invoke("ApplySettings", 1f);
				return;
			}
			base.enabled = false;
			return;
		}
		else
		{
			if (MainManager.instance)
			{
				base.Invoke("ApplySettings", 1f);
				return;
			}
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x00041ABA File Offset: 0x0003FCBA
	public void InMenuOrJournal(bool inMenu)
	{
		
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x00041AEC File Offset: 0x0003FCEC
	public void ApplySettings()
	{
		this.ApplyRotationSetting();
		this.ApplyMovementSetting();
		this.ApplySnapAngleSetting();
		this.ApplyMovementDirectionSetting();
		this.ApplyGrabTypeSetting();
		this.ApplyGripTypeSetting();
		this.ApplyTeleportGrabSetting();
		this.ApplySmoothRotationSpeed();
		this.ApplyControllerRotation();
		this.player.ActivateOrDeactivateRecordingCam(PlayerPrefs.GetInt("SmoothCamValue") == 1);
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x00041B80 File Offset: 0x0003FD80
	private void ApplyMovementSetting()
	{
		int @int = PlayerPrefs.GetInt("locomotionValue");

		if (@int != 1)
		{
			Debug.LogError(PlayerPrefs.GetInt("locomotionValue") + " hasn't been applied to ApplyMovementSetting.");
			return;
		}

	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x00041D28 File Offset: 0x0003FF28
	private void ApplyRotationSetting()
	{
		
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x00041DAD File Offset: 0x0003FFAD
	private void ApplySmoothRotationSpeed()
	{
		
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x00041DD4 File Offset: 0x0003FFD4
	private void ApplySnapAngleSetting()
	{
		
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00041E54 File Offset: 0x00040054
	private void ApplyMovementDirectionSetting()
	{
		int @int = PlayerPrefs.GetInt("movementDirectionValue");
		if (@int != 1)
		{
			Debug.LogError(PlayerPrefs.GetInt("movementDirectionValue") + " hasn't been applied to ApplyMovementDirectionSetting.");
			return;
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00041EB0 File Offset: 0x000400B0
	private void ApplyGrabTypeSetting()
	{
		int @int = PlayerPrefs.GetInt("grabTypeValue");
		if (@int != 1)
		{
			Debug.LogError(PlayerPrefs.GetInt("grabTypeValue") + " hasn't been applied to ApplyGrabTypeSetting.");
			return;
		}
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x00041F24 File Offset: 0x00040124
	private void ApplyGripTypeSetting()
	{
	
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x00041FBC File Offset: 0x000401BC
	private void ApplyTeleportGrabSetting()
	{
		int @int = PlayerPrefs.GetInt("teleportGrabValue");
		if (@int == 0)
		{
			this.leftControllerTeleportGrab.enabled = false;
			this.rightControllerTeleportGrab.enabled = false;
			return;
		}
		if (@int != 1)
		{
			Debug.LogError(PlayerPrefs.GetInt("teleportGrabValue") + " hasn't been applied to ApplyTeleportGrabSetting.");
			return;
		}
		this.leftControllerTeleportGrab.enabled = true;
		this.rightControllerTeleportGrab.enabled = true;
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x00042030 File Offset: 0x00040230
	private void ApplyControllerRotation()
	{
		Vector3 vector = new Vector3(this.GetControllerRotationValue((float)PlayerPrefs.GetInt("controllerRotationXValue")), this.GetControllerRotationValue((float)PlayerPrefs.GetInt("controllerRotationYValue")), this.GetControllerRotationValue((float)PlayerPrefs.GetInt("controllerRotationZValue")));
		this.leftControllerGrabModel.localRotation = Quaternion.Euler(vector);
		this.rightControllerGrabModel.localRotation = Quaternion.Euler(vector);
		this.leftController.localRotation = Quaternion.Euler(new Vector3(vector.x, -vector.y, -vector.z));
		this.rightController.localRotation = Quaternion.Euler(vector);
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x000420D4 File Offset: 0x000402D4
	private float GetControllerRotationValue(float value)
	{
		if (value == -6f)
		{
			return -90f;
		}
		if (value == -5f)
		{
			return -75f;
		}
		if (value == -4f)
		{
			return -60f;
		}
		if (value == -3f)
		{
			return -45f;
		}
		if (value == -2f)
		{
			return -30f;
		}
		if (value == -1f)
		{
			return -15f;
		}
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 15f;
		}
		if (value == 2f)
		{
			return 30f;
		}
		if (value == 3f)
		{
			return 45f;
		}
		if (value == 4f)
		{
			return 60f;
		}
		if (value == 5f)
		{
			return 75f;
		}
		return 90f;
	}

	// Token: 0x04000AF5 RID: 2805
	private PhotonView view;

	// Token: 0x04000AF6 RID: 2806
	[SerializeField]
	private Player player;

	// Token: 0x04000AFD RID: 2813
	private LayerMask defaultMask;

	// Token: 0x04000AFE RID: 2814
	[SerializeField]
	private LayerMask teleportMask;

	// Token: 0x04000B03 RID: 2819
	public Animator anim;

	// Token: 0x04000B07 RID: 2823
	[SerializeField]
	private VRTeleportGrab leftControllerTeleportGrab;

	// Token: 0x04000B08 RID: 2824
	[SerializeField]
	private VRTeleportGrab rightControllerTeleportGrab;

	// Token: 0x04000B0A RID: 2826
	[SerializeField]
	private Transform leftControllerGrabModel;

	// Token: 0x04000B0B RID: 2827
	[SerializeField]
	private Transform rightControllerGrabModel;

	// Token: 0x04000B0C RID: 2828
	[SerializeField]
	private Transform leftController;

	// Token: 0x04000B0D RID: 2829
	[SerializeField]
	private Transform rightController;
}
