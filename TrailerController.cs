using System;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class TrailerController : MonoBehaviour
{
	// Token: 0x04000642 RID: 1602
	public bool inTrailerMode;

	// Token: 0x04000643 RID: 1603
	private int eventIndex = 1;

	// Token: 0x04000644 RID: 1604
	[SerializeField]
	private Camera cam;

	// Token: 0x04000645 RID: 1605
	public TrailerCamera trailerCamera;

	// Token: 0x04000646 RID: 1606
	[SerializeField]
	private Nightvision cameraNightVision;

	// Token: 0x04000647 RID: 1607
	[SerializeField]
	private AudioSource playerArrivedSound;

	// Token: 0x04000648 RID: 1608
	[SerializeField]
	private AudioSource radioBeCarefulSound;

	// Token: 0x04000649 RID: 1609
	[SerializeField]
	private AudioSource playerNothingSound;

	// Token: 0x0400064A RID: 1610
	[SerializeField]
	private AudioSource playerDirtyWaterSound;

	// Token: 0x0400064B RID: 1611
	[SerializeField]
	private AudioSource radioSpiritOrWraithSound;

	// Token: 0x0400064C RID: 1612
	[SerializeField]
	private AudioSource playerGhostInTheBasementSound;

	// Token: 0x0400064D RID: 1613
	[SerializeField]
	private AudioSource radioDontGetAReadingSound;

	// Token: 0x0400064E RID: 1614
	[HideInInspector]
	public EVPRecorder evpRecorder;

	// Token: 0x0400064F RID: 1615
	[SerializeField]
	private AudioSource playerConfirmEVPEvidenceSound;

	// Token: 0x04000650 RID: 1616
	[SerializeField]
	private AudioSource radioConfirmSpririt;

	// Token: 0x04000651 RID: 1617
	[SerializeField]
	private AudioSource playerEVPSound1;

	// Token: 0x04000652 RID: 1618
	[SerializeField]
	private AudioSource playerEVPSound2;

	// Token: 0x04000653 RID: 1619
	[SerializeField]
	private AudioSource playerLowVitalsSound;

	// Token: 0x04000654 RID: 1620
	[SerializeField]
	private LightSwitch tvRemote;

	// Token: 0x04000655 RID: 1621
	[SerializeField]
	private EMF tvEMFSpot;

	// Token: 0x04000656 RID: 1622
	[SerializeField]
	private Door basementDoor;

	// Token: 0x04000657 RID: 1623
	[SerializeField]
	private AudioSource basementDoorOpeningSound;

	// Token: 0x04000658 RID: 1624
	[SerializeField]
	private EMF basementEMFSpot;

	// Token: 0x04000659 RID: 1625
	[SerializeField]
	private LevelRoom basement;

	// Token: 0x0400065A RID: 1626
	[SerializeField]
	private GameObject basementGhost;

	// Token: 0x0400065B RID: 1627
	[SerializeField]
	private LightSwitch basementLight;

	// Token: 0x0400065C RID: 1628
	[SerializeField]
	private Door mainDoor;

	// Token: 0x0400065D RID: 1629
	[SerializeField]
	private Animator mainDoorAnim;

	// Token: 0x0400065E RID: 1630
	[SerializeField]
	private GameObject hallwayGhost;

	// Token: 0x0400065F RID: 1631
	[HideInInspector]
	public Torch torch;

	// Token: 0x04000662 RID: 1634
	private bool isCCTVActive;
}
