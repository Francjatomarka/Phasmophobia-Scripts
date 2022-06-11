using System;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x020000FC RID: 252
[RequireComponent(typeof(Evidence))]
public class ComputerMonitor : MonoBehaviour
{
	// Token: 0x060006CC RID: 1740 RVA: 0x00027F9B File Offset: 0x0002619B
	private void Awake()
	{
		this.isOn = false;
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.view = base.GetComponent<PhotonView>();
		this.evidence = base.GetComponent<Evidence>();
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x00027FC8 File Offset: 0x000261C8
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

	// Token: 0x060006CE RID: 1742 RVA: 0x0002805F File Offset: 0x0002625F
	private void Use()
	{
		this.view.RPC("ComputerMonitorNetworkedUse", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00028078 File Offset: 0x00026278
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

	// Token: 0x040006E5 RID: 1765
	private PhotonObjectInteract photonInteract;

	// Token: 0x040006E6 RID: 1766
	private PhotonView view;

	// Token: 0x040006E7 RID: 1767
	[SerializeField]
	private Renderer screen;

	// Token: 0x040006E8 RID: 1768
	[SerializeField]
	private Texture tex;

	// Token: 0x040006E9 RID: 1769
	[SerializeField]
	private Light myLight;

	// Token: 0x040006EA RID: 1770
	private Evidence evidence;

	// Token: 0x040006EB RID: 1771
	[SerializeField]
	private LightSwitch mainRoomLight;

	// Token: 0x040006EC RID: 1772
	private bool isOn;
}
