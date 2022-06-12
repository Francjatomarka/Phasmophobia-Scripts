using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(Evidence))]
public class ComputerMonitor : MonoBehaviour
{
	private void Awake()
	{
		this.isOn = false;
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
		this.evidence = base.GetComponent<Evidence>();
	}

	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
		this.screen.material.mainTexture = this.tex;
		this.screen.material.SetTexture("_EmissionMap", this.tex);
		this.screen.material.DisableKeyword("_EMISSION");
		this.screen.material.color = Color.black;
		this.myLight.enabled = false;
		this.evidence.enabled = false;
	}

	private void Use()
	{
		this.view.RPC("ComputerMonitorNetworkedUse", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	private void ComputerMonitorNetworkedUse()
	{
		this.isOn = !this.isOn;
		this.evidence.enabled = this.isOn;
		this.myLight.enabled = this.isOn;
		this.screen.material.color = (this.isOn ? Color.white : Color.black);
		if (this.isOn)
		{
			this.screen.material.EnableKeyword("_EMISSION");
		}
		else
		{
			this.screen.material.DisableKeyword("_EMISSION");
		}
		this.mainRoomLight.ResetReflectionProbes();
	}

	private PhotonObjectInteract photonInteract;

	private PhotonView view;

	[SerializeField]
	private Renderer screen;

	[SerializeField]
	private Texture tex;

	[SerializeField]
	private Light myLight;

	private Evidence evidence;

	[SerializeField]
	private LightSwitch mainRoomLight;

	private bool isOn;
}

