using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;

public class TrainingController : MonoBehaviour
{
	private void Awake()
	{
		TrainingController.instance = this;
	}

	private void Start()
	{
		if (PlayerPrefs.GetInt("isTutorial") == 1)
		{
			GameController.instance.OnGhostSpawned.AddListener(new UnityAction(this.GhostSpawned));
			GameController.instance.isTutorial = true;
			this.missionWhiteBoard.SetActive(false);
			this.tutorialWhiteBoard.SetActive(true);
			SetupPhaseController.instance.BeginHuntingPhase();
			this.tvLight.enabled = true;
			this.tvLight.intensity = 0.4f;
			this.trainingTVCanvas.gameObject.SetActive(true);
			UnityEngine.Object.Destroy(this.tvRemote);
			this.trainingRemote.enabled = true;
			for (int i = 0; i < this.probes.Length; i++)
			{
				this.probes[i].RenderProbe();
			}
			for (int j = 0; j < this.slides.Length; j++)
			{
				this.slides[j].SetActive(false);
			}
			this.slides[0].SetActive(true);
			this.NonVRControls.SetActive(true);
		}
	}

	public void GhostSpawned()
	{
		this.tutorialMissionText.text = LocalisationSystem.GetLocalisedValue("Training_MissionFirstPart") + LevelController.instance.currentGhost.ghostInfo.favouriteRoom.roomName + ".";
	}

	public void NextSlide()
	{
		this.currentIndex++;
		if (this.currentIndex >= this.slides.Length)
		{
			this.currentIndex = 0;
		}
		for (int i = 0; i < this.slides.Length; i++)
		{
			this.slides[i].SetActive(false);
		}
		this.slides[this.currentIndex].SetActive(true);
	}

	public static TrainingController instance;

	[SerializeField]
	private GameObject tutorialWhiteBoard;

	[SerializeField]
	private GameObject missionWhiteBoard;

	[SerializeField]
	private Text tutorialMissionText;

	[SerializeField]
	private GameObject[] slides;

	private int currentIndex;

	[SerializeField]
	private Light tvLight;

	[SerializeField]
	private LightSwitch tvRemote;

	[SerializeField]
	private Canvas trainingTVCanvas;

	[SerializeField]
	private TrainingRemote trainingRemote;

	[SerializeField]
	private ReflectionProbe[] probes;

	[SerializeField]
	private GameObject NonVRControls;

	[SerializeField]
	private GameObject VRControls;

	[SerializeField]
	private GameObject ViveControls;

	[SerializeField]
	private GameObject OculusControls;
}

