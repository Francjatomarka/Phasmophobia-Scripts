using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCItemSway : MonoBehaviour
{
	private void OnEnable()
	{
		this.SetPosition();
	}

	public void SetPosition()
	{
		this.startPosition = base.transform.localPosition;
	}

	private void Update()
	{
		this.factorX = Mathf.Clamp(-this.horizontalLook * this.amount, -this.maxAmount, this.maxAmount);
		this.factorY = Mathf.Clamp(-this.verticalLook * this.amount, -this.maxAmount, this.maxAmount);
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(this.startPosition.x + this.factorX, this.startPosition.y + this.factorY, this.startPosition.z), Time.deltaTime * this.smooth);
	}

	public void Look(InputAction.CallbackContext context)
	{
		Vector2 vector = context.ReadValue<Vector2>();
		this.horizontalLook = vector.x;
		this.verticalLook = vector.y;
	}

	private float horizontalLook;

	private float verticalLook;

	[SerializeField]
	private float amount = 0.055f;

	[SerializeField]
	private float maxAmount = 0.055f;

	[SerializeField]
	private float smooth = 3f;

	private Vector3 startPosition;

	private float factorX;

	private float factorY;
}

