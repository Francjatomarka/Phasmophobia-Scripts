using System;
using UnityEngine;
using Photon.Pun;

public class Challenge
{
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

	public string GetLocalisedName()
	{
		return LocalisationSystem.GetLocalisedValue(this.myChallenge.challengeName);
	}

	public void AddProgression(int value)
	{
		this.progressionValue += value;
		PlayerPrefs.SetInt("challenge" + this.dailyChallengeID + "Progression", this.progressionValue);
		if (this.progressionValue >= this.myChallenge.progressionMaxValue)
		{
			this.Completed();
		}
	}

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

	[HideInInspector]
	public PhotonView view;

	public bool completed;

	private int dailyChallengeID;

	public int progressionValue;

	public Challenge.ChallengeValues myChallenge;

	public struct ChallengeValues
	{
		public int uniqueChallengeID;

		public int reward;

		public string challengeName;

		public int progressionMaxValue;
	}
}

