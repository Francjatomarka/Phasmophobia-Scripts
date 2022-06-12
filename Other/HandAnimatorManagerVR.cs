using System;
using UnityEngine;

public class HandAnimatorManagerVR : MonoBehaviour
{
	private void Start()
	{
		string[] joystickNames = Input.GetJoystickNames();
		for (int i = 0; i < joystickNames.Length; i++)
		{
			Debug.Log(joystickNames[i]);
		}
		this.handAnimator = base.GetComponent<Animator>();
	}

	private void Update()
	{
		if (Input.GetKeyUp(this.changeKey))
		{
			this.currentState = (this.currentState + 1) % (this.numberOfAnimations + 1);
		}
		if (Input.GetAxis(this.holdKey) > 0f)
		{
			this.hold = true;
		}
		else
		{
			this.hold = false;
		}
		if (Input.GetKey(this.actionKey))
		{
			this.action = true;
		}
		else
		{
			this.action = false;
		}
		if (this.lastState != this.currentState)
		{
			this.lastState = this.currentState;
			this.handAnimator.SetInteger("State", this.currentState);
			this.TurnOnState(this.currentState);
		}
		this.handAnimator.SetBool("Action", this.action);
		this.handAnimator.SetBool("Hold", this.hold);
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

	public bool action;

	public bool hold;

	public string changeKey = "joystick button 9";

	public string actionKey = "joystick button 15";

	public string holdKey = "Axis 12";

	public int numberOfAnimations = 8;
}

