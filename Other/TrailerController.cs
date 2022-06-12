using System;
using UnityEngine;

public class TrailerController : MonoBehaviour
{
	public bool inTrailerMode;

	private int eventIndex = 1;

	[SerializeField]
	private Camera cam;

	public TrailerCamera trailerCamera;

	[SerializeField]
	private Nightvision cameraNightVision;

	[SerializeField]
	private AudioSource playerArrivedSound;

	[SerializeField]
	private AudioSource radioBeCarefulSound;

	[SerializeField]
	private AudioSource playerNothingSound;

	[SerializeField]
	private AudioSource playerDirtyWaterSound;

	[SerializeField]
	private AudioSource radioSpiritOrWraithSound;

	[SerializeField]
	private AudioSource playerGhostInTheBasementSound;

	[SerializeField]
	private AudioSource radioDontGetAReadingSound;

	[HideInInspector]
	public EVPRecorder evpRecorder;

	[SerializeField]
	private AudioSource playerConfirmEVPEvidenceSound;

	[SerializeField]
	private AudioSource radioConfirmSpririt;

	[SerializeField]
	private AudioSource playerEVPSound1;

	[SerializeField]
	private AudioSource playerEVPSound2;

	[SerializeField]
	private AudioSource playerLowVitalsSound;

	[SerializeField]
	private LightSwitch tvRemote;

	[SerializeField]
	private EMF tvEMFSpot;

	[SerializeField]
	private Door basementDoor;

	[SerializeField]
	private AudioSource basementDoorOpeningSound;

	[SerializeField]
	private EMF basementEMFSpot;

	[SerializeField]
	private LevelRoom basement;

	[SerializeField]
	private GameObject basementGhost;

	[SerializeField]
	private LightSwitch basementLight;

	[SerializeField]
	private Door mainDoor;

	[SerializeField]
	private Animator mainDoorAnim;

	[SerializeField]
	private GameObject hallwayGhost;

	[HideInInspector]
	public Torch torch;

	private bool isCCTVActive;
}

