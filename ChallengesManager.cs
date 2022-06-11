using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000132 RID: 306
public class ChallengesManager : MonoBehaviour
{
	// Token: 0x06000874 RID: 2164 RVA: 0x00033729 File Offset: 0x00031929
	private void OnEnable()
	{
		this.resetLocalisedValue = LocalisationSystem.GetLocalisedValue("Challenge_ResetTimer");
		this.UpdateChallengeValues();
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00033741 File Offset: 0x00031941
	public void ForceResetChallenges()
	{
		DailyChallengesController.Instance.ForceReset();
		this.UpdateChallengeValues();
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x00033754 File Offset: 0x00031954
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

	// Token: 0x06000877 RID: 2167 RVA: 0x00033A48 File Offset: 0x00031C48
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

	// Token: 0x0400087E RID: 2174
	[SerializeField]
	private Text contract1Name;

	// Token: 0x0400087F RID: 2175
	[SerializeField]
	private Text contract1Reward;

	// Token: 0x04000880 RID: 2176
	[SerializeField]
	private Slider contract1Slider;

	// Token: 0x04000881 RID: 2177
	[SerializeField]
	private Text contract1SliderValue;

	// Token: 0x04000882 RID: 2178
	[SerializeField]
	private Text contract2Name;

	// Token: 0x04000883 RID: 2179
	[SerializeField]
	private Text contract2Reward;

	// Token: 0x04000884 RID: 2180
	[SerializeField]
	private Slider contract2Slider;

	// Token: 0x04000885 RID: 2181
	[SerializeField]
	private Text contract2SliderValue;

	// Token: 0x04000886 RID: 2182
	[SerializeField]
	private Text contract3Name;

	// Token: 0x04000887 RID: 2183
	[SerializeField]
	private Text contract3Reward;

	// Token: 0x04000888 RID: 2184
	[SerializeField]
	private Slider contract3Slider;

	// Token: 0x04000889 RID: 2185
	[SerializeField]
	private Text contract3SliderValue;

	// Token: 0x0400088A RID: 2186
	[SerializeField]
	private Text resetTimerText;

	// Token: 0x0400088B RID: 2187
	private string resetLocalisedValue;
}
