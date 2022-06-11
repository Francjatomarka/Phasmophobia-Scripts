using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class EMFData : MonoBehaviour
{
	// Token: 0x06000AC4 RID: 2756 RVA: 0x000429B0 File Offset: 0x00040BB0
	private void Awake()
	{
		EMFData.instance = this;
		this.rend.positionCount = 61;
		for (int i = 0; i < this.rend.positionCount; i++)
		{
			this.rend.SetPosition(i, new Vector3(this.startingPos.localPosition.x + (float)(i * 12), 0f, 0f));
		}
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x00042A17 File Offset: 0x00040C17
	private void Update()
	{
		this.updateTimer -= Time.deltaTime;
		if (this.updateTimer < 0f)
		{
			this.UpdateGraph();
			this.updateTimer = 1f;
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x00042A4C File Offset: 0x00040C4C
	private void UpdateGraph()
	{
		this.rend.SetPosition(60, new Vector3(this.startingPos.localPosition.x + 720f, this.rend.GetPosition(59).y, 0f));
		this.rend.SetPosition(59, new Vector3(this.startingPos.localPosition.x + 708f, this.rend.GetPosition(58).y, 0f));
		this.rend.SetPosition(58, new Vector3(this.startingPos.localPosition.x + 696f, this.rend.GetPosition(57).y, 0f));
		this.rend.SetPosition(57, new Vector3(this.startingPos.localPosition.x + 684f, this.rend.GetPosition(56).y, 0f));
		this.rend.SetPosition(56, new Vector3(this.startingPos.localPosition.x + 672f, this.rend.GetPosition(55).y, 0f));
		this.rend.SetPosition(55, new Vector3(this.startingPos.localPosition.x + 660f, this.rend.GetPosition(54).y, 0f));
		this.rend.SetPosition(54, new Vector3(this.startingPos.localPosition.x + 648f, this.rend.GetPosition(53).y, 0f));
		this.rend.SetPosition(53, new Vector3(this.startingPos.localPosition.x + 636f, this.rend.GetPosition(52).y, 0f));
		this.rend.SetPosition(52, new Vector3(this.startingPos.localPosition.x + 624f, this.rend.GetPosition(51).y, 0f));
		this.rend.SetPosition(51, new Vector3(this.startingPos.localPosition.x + 612f, this.rend.GetPosition(50).y, 0f));
		this.rend.SetPosition(50, new Vector3(this.startingPos.localPosition.x + 600f, this.rend.GetPosition(49).y, 0f));
		this.rend.SetPosition(49, new Vector3(this.startingPos.localPosition.x + 588f, this.rend.GetPosition(48).y, 0f));
		this.rend.SetPosition(48, new Vector3(this.startingPos.localPosition.x + 576f, this.rend.GetPosition(47).y, 0f));
		this.rend.SetPosition(47, new Vector3(this.startingPos.localPosition.x + 564f, this.rend.GetPosition(46).y, 0f));
		this.rend.SetPosition(46, new Vector3(this.startingPos.localPosition.x + 552f, this.rend.GetPosition(45).y, 0f));
		this.rend.SetPosition(45, new Vector3(this.startingPos.localPosition.x + 540f, this.rend.GetPosition(44).y, 0f));
		this.rend.SetPosition(44, new Vector3(this.startingPos.localPosition.x + 528f, this.rend.GetPosition(43).y, 0f));
		this.rend.SetPosition(43, new Vector3(this.startingPos.localPosition.x + 516f, this.rend.GetPosition(42).y, 0f));
		this.rend.SetPosition(42, new Vector3(this.startingPos.localPosition.x + 504f, this.rend.GetPosition(41).y, 0f));
		this.rend.SetPosition(41, new Vector3(this.startingPos.localPosition.x + 492f, this.rend.GetPosition(40).y, 0f));
		this.rend.SetPosition(40, new Vector3(this.startingPos.localPosition.x + 480f, this.rend.GetPosition(39).y, 0f));
		this.rend.SetPosition(39, new Vector3(this.startingPos.localPosition.x + 468f, this.rend.GetPosition(38).y, 0f));
		this.rend.SetPosition(38, new Vector3(this.startingPos.localPosition.x + 456f, this.rend.GetPosition(37).y, 0f));
		this.rend.SetPosition(37, new Vector3(this.startingPos.localPosition.x + 444f, this.rend.GetPosition(36).y, 0f));
		this.rend.SetPosition(36, new Vector3(this.startingPos.localPosition.x + 432f, this.rend.GetPosition(35).y, 0f));
		this.rend.SetPosition(35, new Vector3(this.startingPos.localPosition.x + 420f, this.rend.GetPosition(34).y, 0f));
		this.rend.SetPosition(34, new Vector3(this.startingPos.localPosition.x + 408f, this.rend.GetPosition(33).y, 0f));
		this.rend.SetPosition(33, new Vector3(this.startingPos.localPosition.x + 396f, this.rend.GetPosition(32).y, 0f));
		this.rend.SetPosition(32, new Vector3(this.startingPos.localPosition.x + 384f, this.rend.GetPosition(31).y, 0f));
		this.rend.SetPosition(31, new Vector3(this.startingPos.localPosition.x + 372f, this.rend.GetPosition(30).y, 0f));
		this.rend.SetPosition(30, new Vector3(this.startingPos.localPosition.x + 360f, this.rend.GetPosition(29).y, 0f));
		this.rend.SetPosition(29, new Vector3(this.startingPos.localPosition.x + 348f, this.rend.GetPosition(28).y, 0f));
		this.rend.SetPosition(28, new Vector3(this.startingPos.localPosition.x + 336f, this.rend.GetPosition(27).y, 0f));
		this.rend.SetPosition(27, new Vector3(this.startingPos.localPosition.x + 324f, this.rend.GetPosition(26).y, 0f));
		this.rend.SetPosition(26, new Vector3(this.startingPos.localPosition.x + 312f, this.rend.GetPosition(25).y, 0f));
		this.rend.SetPosition(25, new Vector3(this.startingPos.localPosition.x + 300f, this.rend.GetPosition(24).y, 0f));
		this.rend.SetPosition(24, new Vector3(this.startingPos.localPosition.x + 288f, this.rend.GetPosition(23).y, 0f));
		this.rend.SetPosition(23, new Vector3(this.startingPos.localPosition.x + 276f, this.rend.GetPosition(22).y, 0f));
		this.rend.SetPosition(22, new Vector3(this.startingPos.localPosition.x + 264f, this.rend.GetPosition(21).y, 0f));
		this.rend.SetPosition(21, new Vector3(this.startingPos.localPosition.x + 252f, this.rend.GetPosition(20).y, 0f));
		this.rend.SetPosition(20, new Vector3(this.startingPos.localPosition.x + 240f, this.rend.GetPosition(19).y, 0f));
		this.rend.SetPosition(19, new Vector3(this.startingPos.localPosition.x + 228f, this.rend.GetPosition(18).y, 0f));
		this.rend.SetPosition(18, new Vector3(this.startingPos.localPosition.x + 216f, this.rend.GetPosition(17).y, 0f));
		this.rend.SetPosition(17, new Vector3(this.startingPos.localPosition.x + 204f, this.rend.GetPosition(16).y, 0f));
		this.rend.SetPosition(16, new Vector3(this.startingPos.localPosition.x + 192f, this.rend.GetPosition(15).y, 0f));
		this.rend.SetPosition(15, new Vector3(this.startingPos.localPosition.x + 180f, this.rend.GetPosition(14).y, 0f));
		this.rend.SetPosition(14, new Vector3(this.startingPos.localPosition.x + 168f, this.rend.GetPosition(13).y, 0f));
		this.rend.SetPosition(13, new Vector3(this.startingPos.localPosition.x + 156f, this.rend.GetPosition(12).y, 0f));
		this.rend.SetPosition(12, new Vector3(this.startingPos.localPosition.x + 144f, this.rend.GetPosition(11).y, 0f));
		this.rend.SetPosition(11, new Vector3(this.startingPos.localPosition.x + 132f, this.rend.GetPosition(10).y, 0f));
		this.rend.SetPosition(10, new Vector3(this.startingPos.localPosition.x + 120f, this.rend.GetPosition(9).y, 0f));
		this.rend.SetPosition(9, new Vector3(this.startingPos.localPosition.x + 108f, this.rend.GetPosition(8).y, 0f));
		this.rend.SetPosition(8, new Vector3(this.startingPos.localPosition.x + 96f, this.rend.GetPosition(7).y, 0f));
		this.rend.SetPosition(7, new Vector3(this.startingPos.localPosition.x + 84f, this.rend.GetPosition(6).y, 0f));
		this.rend.SetPosition(6, new Vector3(this.startingPos.localPosition.x + 72f, this.rend.GetPosition(5).y, 0f));
		this.rend.SetPosition(5, new Vector3(this.startingPos.localPosition.x + 60f, this.rend.GetPosition(4).y, 0f));
		this.rend.SetPosition(4, new Vector3(this.startingPos.localPosition.x + 48f, this.rend.GetPosition(3).y, 0f));
		this.rend.SetPosition(3, new Vector3(this.startingPos.localPosition.x + 36f, this.rend.GetPosition(2).y, 0f));
		this.rend.SetPosition(2, new Vector3(this.startingPos.localPosition.x + 24f, this.rend.GetPosition(1).y, 0f));
		this.rend.SetPosition(1, new Vector3(this.startingPos.localPosition.x + 12f, this.rend.GetPosition(0).y, 0f));
		this.rend.SetPosition(0, new Vector3(this.startingPos.localPosition.x + 0f, (float)(this.GetEMFValue() * 50), 0f));
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x00043944 File Offset: 0x00041B44
	private int GetEMFValue()
	{
		int num = 0;
		for (int i = 0; i < this.emfSpots.Count; i++)
		{
			num += this.emfSpots[i].strength;
		}
		if (LevelController.instance && LevelController.instance.currentGhost && (num > 10 || LevelController.instance.currentGhost.isHunting))
		{
			num = 10;
		}
		return num;
	}

	// Token: 0x04000B28 RID: 2856
	public static EMFData instance;

	// Token: 0x04000B29 RID: 2857
	[SerializeField]
	private LineRenderer rend;

	// Token: 0x04000B2A RID: 2858
	public List<EMF> emfSpots = new List<EMF>();

	// Token: 0x04000B2B RID: 2859
	[SerializeField]
	private Transform startingPos;

	// Token: 0x04000B2C RID: 2860
	private float updateTimer = 0.5f;
}
