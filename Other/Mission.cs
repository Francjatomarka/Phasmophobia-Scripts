using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Mission : MonoBehaviour
{
	public void Completed()
	{
		this.completed = true;
		this.myText.color = new Color(this.myText.color.r, this.myText.color.r, this.myText.color.r, 0.3f);
	}

	public void SetUIText()
	{
		this.myText.text = string.Concat(new object[]
		{
			LocalisationSystem.GetLocalisedValue("WhiteBoard_Objective"),
			" ",
			this.sideMissionID + 1,
			": ",
			this.missionName
		});
	}

	[HideInInspector]
	public PhotonView view;

	public Mission.MissionType type;

	public bool completed;

	public string missionName = "Mission name has not been set";

	public Text myText;

	public int sideMissionID;

	public enum MissionType
	{
		none,
		main,
		side
	}
}

