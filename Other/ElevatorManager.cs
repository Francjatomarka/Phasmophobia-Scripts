using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000024 RID: 36
public class ElevatorManager : MonoBehaviour
{
	// Token: 0x060000F8 RID: 248 RVA: 0x000081E3 File Offset: 0x000063E3
	private void Awake()
	{
		if (this.RandomStartFloor)
		{
			this.elevatorsCount = base.transform.childCount;
			this.InitialFloor = UnityEngine.Random.Range(1, this.elevatorsCount + 1);
			this.WasStarted();
		}
	}

	// Token: 0x0400010E RID: 270
	private int elevatorsCount;

	// Token: 0x0400010F RID: 271
	public bool RandomStartFloor = true;

	// Token: 0x04000110 RID: 272
	public int InitialFloor = 1;

	// Token: 0x04000111 RID: 273
	public UnityAction WasStarted;

	// Token: 0x04000112 RID: 274
	[HideInInspector]
	public int _floor;

	// Token: 0x04000113 RID: 275
	private Transform[] elevators;
}
