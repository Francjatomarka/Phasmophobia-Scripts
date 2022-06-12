using System;
using System.Collections.Generic;
using UnityEngine;

public class DailyChallengesController
{
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

	private void Setup()
	{
		this.currentChallenges.Clear();
		this.challengeList.CreateList();
		this.GenerateNewDailyChallenges();
		PlayerPrefs.SetInt("currentChallengesSeed", this.seed);
	}

	public void ForceReset()
	{
		this.currentChallenges.Clear();
		this.challengeList.CreateList();
		PlayerPrefs.SetInt("currentChallengesSeed", this.seed);
		this.GenerateNewDailyChallenges();
	}

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

	private void GenerateChallenge(int challengeID, int challengeNumber)
	{
		Challenge challenge = new Challenge();
		challenge.SetupChallenge(challengeNumber, challengeID, this.challengeList.listOfChallenges[challengeID].reward, this.challengeList.listOfChallenges[challengeID].challengeName, this.challengeList.listOfChallenges[challengeID].progressionMaxValue);
		this.currentChallenges.Add(challenge);
	}

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

	private static DailyChallengesController _instance;

	private ChallengeList challengeList = new ChallengeList();

	public List<Challenge> currentChallenges = new List<Challenge>();

	private int seed;
}

