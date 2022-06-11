using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

// Token: 0x02000196 RID: 406
public class Mission : MonoBehaviour
{
	// Token: 0x06000B1D RID: 2845 RVA: 0x00045588 File Offset: 0x00043788
	public void Completed()
	{
		this.completed = true;
		this.myText.color = new Color(this.myText.color.r, this.myText.color.r, this.myText.color.r, 0.3f);
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x000455E4 File Offset: 0x000437E4
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

	// Token: 0x04000B74 RID: 2932
	[HideInInspector]
	public PhotonView view;

	// Token: 0x04000B75 RID: 2933
	public Mission.MissionType type;

	// Token: 0x04000B76 RID: 2934
	public bool completed;

	// Token: 0x04000B77 RID: 2935
	public string missionName = "Mission name has not been set";

	// Token: 0x04000B78 RID: 2936
	public Text myText;

	// Token: 0x04000B79 RID: 2937
	public int sideMissionID;

	// Token: 0x0200054A RID: 1354
	public enum MissionType
	{
		// Token: 0x04002577 RID: 9591
		none,
		// Token: 0x04002578 RID: 9592
		main,
		// Token: 0x04002579 RID: 9593
		side
	}
}
