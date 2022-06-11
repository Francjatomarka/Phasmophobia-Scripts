using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class HandAnimatorManagerVR : MonoBehaviour
{
	// Token: 0x06000103 RID: 259 RVA: 0x000086F8 File Offset: 0x000068F8
	private void Start()
	{
		string[] joystickNames = Input.GetJoystickNames();
		for (int i = 0; i < joystickNames.Length; i++)
		{
			Debug.Log(joystickNames[i]);
		}
		this.handAnimator = base.GetComponent<Animator>();
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00008730 File Offset: 0x00006930
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

	// Token: 0x06000105 RID: 261 RVA: 0x00008808 File Offset: 0x00006A08
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

	// Token: 0x0400012B RID: 299
	public StateModel[] stateModels;

	// Token: 0x0400012C RID: 300
	private Animator handAnimator;

	// Token: 0x0400012D RID: 301
	public int currentState = 100;

	// Token: 0x0400012E RID: 302
	private int lastState = -1;

	// Token: 0x0400012F RID: 303
	public bool action;

	// Token: 0x04000130 RID: 304
	public bool hold;

	// Token: 0x04000131 RID: 305
	public string changeKey = "joystick button 9";

	// Token: 0x04000132 RID: 306
	public string actionKey = "joystick button 15";

	// Token: 0x04000133 RID: 307
	public string holdKey = "Axis 12";

	// Token: 0x04000134 RID: 308
	public int numberOfAnimations = 8;
}
