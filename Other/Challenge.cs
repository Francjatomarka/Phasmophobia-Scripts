using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x020000E7 RID: 231
public class Challenge
{
	// Token: 0x06000670 RID: 1648 RVA: 0x0002604C File Offset: 0x0002424C
	public void SetupChallenge(int _dailyChallengeID, int _uniqueID, int _reward, string _challengeName, int _progressionMaxValue)
	{
		this.dailyChallengeID = _dailyChallengeID;
		this.myChallenge.uniqueChallengeID = _uniqueID;
		this.myChallenge.reward = _reward;
		this.myChallenge.challengeName = LocalisationSystem.GetLocalisedValue(_challengeName);
		this.myChallenge.progressionMaxValue = _progressionMaxValue;
		this.completed = (PlayerPrefs.GetInt("challenge" + this.dailyChallengeID + "Completed") == 1);
		this.progressionValue = PlayerPrefs.GetInt("challenge" + this.dailyChallengeID + "Progression");
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x000260E4 File Offset: 0x000242E4
	public string GetLocalisedName()
	{
		return LocalisationSystem.GetLocalisedValue(this.myChallenge.challengeName);
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x000260F8 File Offset: 0x000242F8
	public void AddProgression(int value)
	{
		this.progressionValue += value;
		PlayerPrefs.SetInt("challenge" + this.dailyChallengeID + "Progression", this.progressionValue);
		if (this.progressionValue >= this.myChallenge.progressionMaxValue)
		{
			this.Completed();
		}
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00026154 File Offset: 0x00024354
	private void Completed()
	{
		if (!this.completed)
		{
			this.completed = true;
			PlayerPrefs.SetInt("challenge" + this.dailyChallengeID + "Completed", 1);
			PlayerPrefs.SetInt("PlayerMoney", PlayerPrefs.GetInt("PlayerMoney") + this.myChallenge.reward);
			this.progressionValue = this.myChallenge.progressionMaxValue;
			PlayerPrefs.SetInt("challenge" + this.dailyChallengeID + "Progression", this.progressionValue);
		}
	}

	// Token: 0x04000681 RID: 1665
	[HideInInspector]
	public PhotonView view;

	// Token: 0x04000682 RID: 1666
	public bool completed;

	// Token: 0x04000683 RID: 1667
	private int dailyChallengeID;

	// Token: 0x04000684 RID: 1668
	public int progressionValue;

	// Token: 0x04000685 RID: 1669
	public Challenge.ChallengeValues myChallenge;

	// Token: 0x020004BA RID: 1210
	public struct ChallengeValues
	{
		// Token: 0x0400226C RID: 8812
		public int uniqueChallengeID;

		// Token: 0x0400226D RID: 8813
		public int reward;

		// Token: 0x0400226E RID: 8814
		public string challengeName;

		// Token: 0x0400226F RID: 8815
		public int progressionMaxValue;
	}
}
