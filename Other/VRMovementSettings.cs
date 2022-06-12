using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class VRMovementSettings : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

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

	public void InMenuOrJournal(bool inMenu)
	{
		
	}

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

	private void ApplyMovementSetting()
	{
		int @int = PlayerPrefs.GetInt("locomotionValue");

		if (@int != 1)
		{
			Debug.LogError(PlayerPrefs.GetInt("locomotionValue") + " hasn't been applied to ApplyMovementSetting.");
			return;
		}

	}

	private void ApplyRotationSetting()
	{
		
	}

	private void ApplySmoothRotationSpeed()
	{
		
	}

	private void ApplySnapAngleSetting()
	{
		
	}

	private void ApplyMovementDirectionSetting()
	{
		int @int = PlayerPrefs.GetInt("movementDirectionValue");
		if (@int != 1)
		{
			Debug.LogError(PlayerPrefs.GetInt("movementDirectionValue") + " hasn't been applied to ApplyMovementDirectionSetting.");
			return;
		}
	}

	private void ApplyGrabTypeSetting()
	{
		int @int = PlayerPrefs.GetInt("grabTypeValue");
		if (@int != 1)
		{
			Debug.LogError(PlayerPrefs.GetInt("grabTypeValue") + " hasn't been applied to ApplyGrabTypeSetting.");
			return;
		}
	}

	private void ApplyGripTypeSetting()
	{
	
	}

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

	private void ApplyControllerRotation()
	{
		Vector3 vector = new Vector3(this.GetControllerRotationValue((float)PlayerPrefs.GetInt("controllerRotationXValue")), this.GetControllerRotationValue((float)PlayerPrefs.GetInt("controllerRotationYValue")), this.GetControllerRotationValue((float)PlayerPrefs.GetInt("controllerRotationZValue")));
		this.leftControllerGrabModel.localRotation = Quaternion.Euler(vector);
		this.rightControllerGrabModel.localRotation = Quaternion.Euler(vector);
		this.leftController.localRotation = Quaternion.Euler(new Vector3(vector.x, -vector.y, -vector.z));
		this.rightController.localRotation = Quaternion.Euler(vector);
	}

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

	private PhotonView view;

	[SerializeField]
	private Player player;

	private LayerMask defaultMask;

	[SerializeField]
	private LayerMask teleportMask;

	public Animator anim;

	[SerializeField]
	private VRTeleportGrab leftControllerTeleportGrab;

	[SerializeField]
	private VRTeleportGrab rightControllerTeleportGrab;

	[SerializeField]
	private Transform leftControllerGrabModel;

	[SerializeField]
	private Transform rightControllerGrabModel;

	[SerializeField]
	private Transform leftController;

	[SerializeField]
	private Transform rightController;
}

