using System;
using UnityEngine;

public class HandAnimatorManager : MonoBehaviour
{
	private void Start()
	{
		this.handAnimator = base.GetComponent<Animator>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			this.currentState = 0;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.currentState = 1;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.currentState = 2;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.currentState = 3;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.currentState = 4;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			this.currentState = 5;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			this.currentState = 6;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			this.currentState = 7;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			this.currentState = 8;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			this.currentState = 9;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			this.currentState = 10;
		}
		else if (Input.GetKeyDown(KeyCode.I))
		{
			this.currentState = 100;
		}
		if (this.lastState != this.currentState)
		{
			this.lastState = this.currentState;
			this.handAnimator.SetInteger("State", this.currentState);
			this.TurnOnState(this.currentState);
		}
		this.handAnimator.SetBool("Action", Input.GetMouseButton(0));
		this.handAnimator.SetBool("Hold", Input.GetMouseButton(1));
	}

	private void TurnOnState(int stateNumber)
	{
		foreach (StateModel stateModel in this.stateModels)
		{
			if (stateModel.stateNumber == stateNumber && !stateModel.go.activeSelf)
			{
				stateModel.go.SetActive(true);
			}
			else if (stateModel.go.activeSelf)
			{
				stateModel.go.SetActive(false);
			}
		}
	}

	public StateModel[] stateModels;

	private Animator handAnimator;

	public int currentState = 100;

	private int lastState = -1;
}

