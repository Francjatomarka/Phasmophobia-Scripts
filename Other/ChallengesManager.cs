using System;
using UnityEngine;
using UnityEngine.UI;

public class ChallengesManager : MonoBehaviour
{
	private void OnEnable()
	{
		this.resetLocalisedValue = LocalisationSystem.GetLocalisedValue("Challenge_ResetTimer");
		this.UpdateChallengeValues();
	}

	public void ForceResetChallenges()
	{
		DailyChallengesController.Instance.ForceReset();
		this.UpdateChallengeValues();
	}

	public void UpdateChallengeValues()
	{
		this.contract1Name.text = DailyChallengesController.Instance.currentChallenges[0].GetLocalisedName();
		this.contract1Reward.text = "$" + DailyChallengesController.Instance.currentChallenges[0].myChallenge.reward;
		int num = Mathf.Clamp(DailyChallengesController.Instance.currentChallenges[0].progressionValue, 0, DailyChallengesController.Instance.currentChallenges[0].myChallenge.progressionMaxValue);
		this.contract1SliderValue.text = num + "/" + DailyChallengesController.Instance.currentChallenges[0].myChallenge.progressionMaxValue;
		this.contract1Slider.maxValue = (float)DailyChallengesController.Instance.currentChallenges[0].myChallenge.progressionMaxValue;
		this.contract1Slider.value = (float)num;
		this.contract2Name.text = DailyChallengesController.Instance.currentChallenges[1].GetLocalisedName();
		this.contract2Reward.text = "$" + DailyChallengesController.Instance.currentChallenges[1].myChallenge.reward;
		int num2 = Mathf.Clamp(DailyChallengesController.Instance.currentChallenges[1].progressionValue, 0, DailyChallengesController.Instance.currentChallenges[1].myChallenge.progressionMaxValue);
		this.contract2SliderValue.text = num2 + "/" + DailyChallengesController.Instance.currentChallenges[1].myChallenge.progressionMaxValue;
		this.contract2Slider.maxValue = (float)DailyChallengesController.Instance.currentChallenges[1].myChallenge.progressionMaxValue;
		this.contract2Slider.value = (float)num2;
		this.contract3Name.text = DailyChallengesController.Instance.currentChallenges[2].GetLocalisedName();
		this.contract3Reward.text = "$" + DailyChallengesController.Instance.currentChallenges[2].myChallenge.reward;
		int num3 = Mathf.Clamp(DailyChallengesController.Instance.currentChallenges[2].progressionValue, 0, DailyChallengesController.Instance.currentChallenges[2].myChallenge.progressionMaxValue);
		this.contract3SliderValue.text = num3 + "/" + DailyChallengesController.Instance.currentChallenges[1].myChallenge.progressionMaxValue;
		this.contract3Slider.maxValue = (float)DailyChallengesController.Instance.currentChallenges[2].myChallenge.progressionMaxValue;
		this.contract3Slider.value = (float)num3;
	}

	private void Update()
	{
		this.resetTimerText.text = string.Concat(new string[]
		{
			this.resetLocalisedValue,
			": ",
			(23 - DateTime.UtcNow.Hour).ToString("00"),
			":",
			(59 - DateTime.UtcNow.Minute).ToString("00"),
			":",
			(59 - DateTime.UtcNow.Second).ToString("00")
		});
	}

	[SerializeField]
	private Text contract1Name;

	[SerializeField]
	private Text contract1Reward;

	[SerializeField]
	private Slider contract1Slider;

	[SerializeField]
	private Text contract1SliderValue;

	[SerializeField]
	private Text contract2Name;

	[SerializeField]
	private Text contract2Reward;

	[SerializeField]
	private Slider contract2Slider;

	[SerializeField]
	private Text contract2SliderValue;

	[SerializeField]
	private Text contract3Name;

	[SerializeField]
	private Text contract3Reward;

	[SerializeField]
	private Slider contract3Slider;

	[SerializeField]
	private Text contract3SliderValue;

	[SerializeField]
	private Text resetTimerText;

	private string resetLocalisedValue;
}

