using System;
using System.Collections.Generic;

public class ChallengeList
{
	public void CreateList()
	{
		this.listOfChallenges.Clear();
		this.listOfChallenges.Add(this.PlayContracts);
		this.listOfChallenges.Add(this.PlayTogether);
		this.listOfChallenges.Add(this.PlayWithDefaultItems);
		this.listOfChallenges.Add(this.PlayMediumContract);
		this.listOfChallenges.Add(this.CompleteObjectives);
		this.listOfChallenges.Add(this.DiscoverGhostType);
		this.listOfChallenges.Add(this.FindDNAEvidence);
		this.listOfChallenges.Add(this.GetRewardWithPhotos);
		this.listOfChallenges.Add(this.BuyStoreItem);
		this.listOfChallenges.Add(this.OuijaBoardResponse);
		this.listOfChallenges.Add(this.TakeGhostPhoto);
		this.listOfChallenges.Add(this.SurviveHuntingPhase);
		this.listOfChallenges.Add(this.ReachZeroSanity);
		this.listOfChallenges.Add(this.SpiritBoxResponse);
	}

	public List<Challenge.ChallengeValues> listOfChallenges = new List<Challenge.ChallengeValues>();

	public Challenge.ChallengeValues PlayContracts = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 0,
		reward = 35,
		challengeName = "Challenge_PlayContracts",
		progressionMaxValue = 3
	};

	public Challenge.ChallengeValues PlayTogether = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 1,
		reward = 15,
		challengeName = "Challenge_PlayTogether",
		progressionMaxValue = 1
	};

	public Challenge.ChallengeValues PlayWithDefaultItems = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 2,
		reward = 15,
		challengeName = "Challenge_PlayWithDefaultItems",
		progressionMaxValue = 1
	};

	public Challenge.ChallengeValues PlayMediumContract = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 3,
		reward = 20,
		challengeName = "Challenge_PlayMediumContract",
		progressionMaxValue = 1
	};

	public Challenge.ChallengeValues CompleteObjectives = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 4,
		reward = 30,
		challengeName = "Challenge_CompleteObjectives",
		progressionMaxValue = 5
	};

	public Challenge.ChallengeValues DiscoverGhostType = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 5,
		reward = 25,
		challengeName = "Challenge_DiscoverGhostType",
		progressionMaxValue = 1
	};

	public Challenge.ChallengeValues FindDNAEvidence = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 6,
		reward = 10,
		challengeName = "Challenge_FindDNAEvidence",
		progressionMaxValue = 1
	};

	public Challenge.ChallengeValues GetRewardWithPhotos = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 7,
		reward = 15,
		challengeName = "Challenge_GetPhotoReward",
		progressionMaxValue = 1
	};

	public Challenge.ChallengeValues BuyStoreItem = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 8,
		reward = 10,
		challengeName = "Challenge_BuyStoreItem",
		progressionMaxValue = 1
	};

	public Challenge.ChallengeValues OuijaBoardResponse = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 9,
		reward = 20,
		challengeName = "Challenge_OuijaBoardResponse",
		progressionMaxValue = 1
	};

	public Challenge.ChallengeValues TakeGhostPhoto = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 10,
		reward = 25,
		challengeName = "Challenge_TakeGhostPhoto",
		progressionMaxValue = 1
	};

	public Challenge.ChallengeValues SurviveHuntingPhase = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 11,
		reward = 15,
		challengeName = "Challenge_SurviveHuntingPhase",
		progressionMaxValue = 1
	};

	public Challenge.ChallengeValues ReachZeroSanity = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 12,
		reward = 10,
		challengeName = "Challenge_ReachZeroSanity",
		progressionMaxValue = 1
	};

	public Challenge.ChallengeValues SpiritBoxResponse = new Challenge.ChallengeValues
	{
		uniqueChallengeID = 13,
		reward = 15,
		challengeName = "Challenge_SpiritBoxEvidence",
		progressionMaxValue = 1
	};
}

