using System;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorManager : MonoBehaviour
{
	private void Awake()
	{
		if (this.RandomStartFloor)
		{
			this.elevatorsCount = base.transform.childCount;
			this.InitialFloor = UnityEngine.Random.Range(1, this.elevatorsCount + 1);
			this.WasStarted();
		}
	}

	private int elevatorsCount;

	public bool RandomStartFloor = true;

	public int InitialFloor = 1;

	public UnityAction WasStarted;

	[HideInInspector]
	public int _floor;

	private Transform[] elevators;
}

