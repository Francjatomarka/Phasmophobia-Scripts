using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000166 RID: 358
public class PCItemSway : MonoBehaviour
{
	// Token: 0x06000A20 RID: 2592 RVA: 0x0003DE1E File Offset: 0x0003C01E
	private void OnEnable()
	{
		this.SetPosition();
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x0003DE26 File Offset: 0x0003C026
	public void SetPosition()
	{
		this.startPosition = base.transform.localPosition;
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x0003DE3C File Offset: 0x0003C03C
	private void Update()
	{
		this.factorX = Mathf.Clamp(-this.horizontalLook * this.amount, -this.maxAmount, this.maxAmount);
		this.factorY = Mathf.Clamp(-this.verticalLook * this.amount, -this.maxAmount, this.maxAmount);
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(this.startPosition.x + this.factorX, this.startPosition.y + this.factorY, this.startPosition.z), Time.deltaTime * this.smooth);
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x0003DEF0 File Offset: 0x0003C0F0
	public void Look(InputAction.CallbackContext context)
	{
		Vector2 vector = context.ReadValue<Vector2>();
		this.horizontalLook = vector.x;
		this.verticalLook = vector.y;
	}

	// Token: 0x04000A55 RID: 2645
	private float horizontalLook;

	// Token: 0x04000A56 RID: 2646
	private float verticalLook;

	// Token: 0x04000A57 RID: 2647
	[SerializeField]
	private float amount = 0.055f;

	// Token: 0x04000A58 RID: 2648
	[SerializeField]
	private float maxAmount = 0.055f;

	// Token: 0x04000A59 RID: 2649
	[SerializeField]
	private float smooth = 3f;

	// Token: 0x04000A5A RID: 2650
	private Vector3 startPosition;

	// Token: 0x04000A5B RID: 2651
	private float factorX;

	// Token: 0x04000A5C RID: 2652
	private float factorY;
}
