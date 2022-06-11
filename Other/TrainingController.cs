using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;

// Token: 0x020000E5 RID: 229
public class TrainingController : MonoBehaviour
{
	// Token: 0x0600066B RID: 1643 RVA: 0x00025E5E File Offset: 0x0002405E
	private void Awake()
	{
		TrainingController.instance = this;
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00025E68 File Offset: 0x00024068
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

	// Token: 0x0600066D RID: 1645 RVA: 0x00025FA9 File Offset: 0x000241A9
	public void GhostSpawned()
	{
		this.tutorialMissionText.text = LocalisationSystem.GetLocalisedValue("Training_MissionFirstPart") + LevelController.instance.currentGhost.ghostInfo.favouriteRoom.roomName + ".";
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x00025FE4 File Offset: 0x000241E4
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

	// Token: 0x04000663 RID: 1635
	public static TrainingController instance;

	// Token: 0x04000664 RID: 1636
	[SerializeField]
	private GameObject tutorialWhiteBoard;

	// Token: 0x04000665 RID: 1637
	[SerializeField]
	private GameObject missionWhiteBoard;

	// Token: 0x04000666 RID: 1638
	[SerializeField]
	private Text tutorialMissionText;

	// Token: 0x04000667 RID: 1639
	[SerializeField]
	private GameObject[] slides;

	// Token: 0x04000668 RID: 1640
	private int currentIndex;

	// Token: 0x04000669 RID: 1641
	[SerializeField]
	private Light tvLight;

	// Token: 0x0400066A RID: 1642
	[SerializeField]
	private LightSwitch tvRemote;

	// Token: 0x0400066B RID: 1643
	[SerializeField]
	private Canvas trainingTVCanvas;

	// Token: 0x0400066C RID: 1644
	[SerializeField]
	private TrainingRemote trainingRemote;

	// Token: 0x0400066D RID: 1645
	[SerializeField]
	private ReflectionProbe[] probes;

	// Token: 0x0400066E RID: 1646
	[SerializeField]
	private GameObject NonVRControls;

	// Token: 0x0400066F RID: 1647
	[SerializeField]
	private GameObject VRControls;

	// Token: 0x04000670 RID: 1648
	[SerializeField]
	private GameObject ViveControls;

	// Token: 0x04000671 RID: 1649
	[SerializeField]
	private GameObject OculusControls;
}
