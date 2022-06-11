using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class DailyChallengesController
{
	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06000677 RID: 1655 RVA: 0x000265EB File Offset: 0x000247EB
	public static DailyChallengesController Instance
	{
		get
		{
			if (DailyChallengesController._instance == null)
			{
				DailyChallengesController._instance = new DailyChallengesController();
				DailyChallengesController._instance.Setup();
			}
			return DailyChallengesController._instance;
		}
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x00026610 File Offset: 0x00024810
	private void Setup()
	{
		this.currentChallenges.Clear();
		this.challengeList.CreateList();
		this.GenerateNewDailyChallenges();
		PlayerPrefs.SetInt("currentChallengesSeed", this.seed);
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x000266BC File Offset: 0x000248BC
	public void ForceReset()
	{
		this.currentChallenges.Clear();
		this.challengeList.CreateList();
		PlayerPrefs.SetInt("currentChallengesSeed", this.seed);
		this.GenerateNewDailyChallenges();
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00026720 File Offset: 0x00024920
	private void GenerateNewDailyChallenges()
	{
		int num = UnityEngine.Random.Range(0, 4);
		int num2 = UnityEngine.Random.Range(5, 8);
		int num3 = UnityEngine.Random.Range(9, 13);
		PlayerPrefs.SetInt("challenge1", num);
		PlayerPrefs.SetInt("challenge2", num2);
		PlayerPrefs.SetInt("challenge3", num3);
		PlayerPrefs.SetInt("challenge1Progression", 0);
		PlayerPrefs.SetInt("challenge2Progression", 0);
		PlayerPrefs.SetInt("challenge3Progression", 0);
		this.GenerateChallenge(num, 1);
		this.GenerateChallenge(num2, 2);
		this.GenerateChallenge(num3, 3);
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x000267B4 File Offset: 0x000249B4
	private void GenerateChallenge(int challengeID, int challengeNumber)
	{
		Challenge challenge = new Challenge();
		challenge.SetupChallenge(challengeNumber, challengeID, this.challengeList.listOfChallenges[challengeID].reward, this.challengeList.listOfChallenges[challengeID].challengeName, this.challengeList.listOfChallenges[challengeID].progressionMaxValue);
		this.currentChallenges.Add(challenge);
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00026820 File Offset: 0x00024A20
	public void ChangeChallengeProgression(ChallengeType type, int value)
	{
		for (int i = 0; i < this.currentChallenges.Count; i++)
		{
			if (this.currentChallenges[i].myChallenge.uniqueChallengeID == this.challengeList.listOfChallenges[(int)type].uniqueChallengeID)
			{
				this.currentChallenges[i].AddProgression(value);
			}
		}
	}

	// Token: 0x04000695 RID: 1685
	private static DailyChallengesController _instance;

	// Token: 0x04000696 RID: 1686
	private ChallengeList challengeList = new ChallengeList();

	// Token: 0x04000697 RID: 1687
	public List<Challenge> currentChallenges = new List<Challenge>();

	// Token: 0x04000699 RID: 1689
	private int seed;
}
