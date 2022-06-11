using System;
using TLGFPowerBooks;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class ExampleDragController : MonoBehaviour
{
	// Token: 0x060000AB RID: 171 RVA: 0x00005116 File Offset: 0x00003316
	private void Start()
	{
		if (this.useThisCamera != null)
		{
			this.cam = this.useThisCamera;
		}
		else
		{
			this.cam = Camera.main;
		}
		this.ResetMaxDragStartDistance();
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00005145 File Offset: 0x00003345
	public void SetPBook(PBook book)
	{
		this.pBook = book;
		this.ResetMaxDragStartDistance();
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00005154 File Offset: 0x00003354
	public void ResetMaxDragStartDistance()
	{
		this.bookCollider = this.pBook.transform.Find("Book").GetComponent<Collider>();
		this.minDragDistance = this.bookCollider.bounds.size.x;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x000051A0 File Offset: 0x000033A0
	private void Update()
	{
		if (this.pBook != null)
		{
			Ray ray = this.cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (Input.GetMouseButtonDown(0) && this.dragstate == ExampleDragController.DragState.NODRAG && Physics.Raycast(ray, out raycastHit, this.raycastDistance, this.bookLayer))
			{
				if (raycastHit.transform.parent != null && raycastHit.transform.parent.gameObject == this.pBook.gameObject)
				{
					this.dragStartPosition = raycastHit.point;
					if (this.pBook.GetBookState() == PBook.BookState.CLOSED)
					{
						float num = raycastHit.transform.InverseTransformPoint(raycastHit.point).x * 2.25f;
						num = Mathf.Clamp(num, this.minDragDistance, num);
						this.dragTargetPosition = this.dragStartPosition - this.pBook.transform.right * num;
						this.maxDragDistance = Mathf.Abs((Input.mousePosition - this.cam.WorldToScreenPoint(this.dragTargetPosition)).x);
						this.dragstate = ExampleDragController.DragState.OPENDRAG;
					}
				}
				if (raycastHit.transform.parent.parent != null && raycastHit.transform.parent.parent.gameObject == this.pBook.gameObject && raycastHit.transform.name == "PageCenterRight" && !this.pBook.IsLastPage())
				{
					this.dragStartPosition = raycastHit.point;
					if (this.pBook.GetBookState() == PBook.BookState.OPEN)
					{
						float num2 = Mathf.Abs(raycastHit.transform.parent.parent.InverseTransformPoint(raycastHit.point).x * 2.5f);
						num2 = Mathf.Clamp(num2, this.minDragDistance, num2);
						this.dragTargetPosition = this.dragStartPosition - this.pBook.transform.right * num2;
						this.maxDragDistance = Mathf.Abs((Input.mousePosition - this.cam.WorldToScreenPoint(this.dragTargetPosition)).x);
						this.dragstate = ExampleDragController.DragState.NEXTDRAG;
					}
				}
				if (raycastHit.transform.parent.parent != null && raycastHit.transform.parent.parent.gameObject == this.pBook.gameObject && raycastHit.transform.name == "PageCenterLeft" && !this.pBook.IsFirstPage())
				{
					this.dragStartPosition = raycastHit.point;
					if (this.pBook.GetBookState() == PBook.BookState.OPEN)
					{
						float num3 = Mathf.Abs(raycastHit.transform.parent.parent.InverseTransformPoint(raycastHit.point).x * 1.9f);
						num3 = Mathf.Clamp(num3, this.minDragDistance, num3);
						this.dragTargetPosition = this.dragStartPosition + this.pBook.transform.right * num3;
						this.maxDragDistance = Mathf.Abs((Input.mousePosition - this.cam.WorldToScreenPoint(this.dragTargetPosition)).x);
						this.dragstate = ExampleDragController.DragState.PREVDRAG;
					}
				}
			}
			if (Input.GetMouseButtonDown(1) && this.dragstate == ExampleDragController.DragState.NODRAG && Physics.Raycast(ray, out raycastHit, this.raycastDistance, this.bookLayer) && raycastHit.transform.parent.parent != null && raycastHit.transform.parent.parent.gameObject == this.pBook.gameObject && raycastHit.transform.name == "PageCenterLeft")
			{
				this.dragStartPosition = raycastHit.point;
				if (this.pBook.GetBookState() == PBook.BookState.OPEN)
				{
					float num4 = Mathf.Abs(raycastHit.transform.parent.parent.InverseTransformPoint(raycastHit.point).x * 1.9f);
					num4 = Mathf.Clamp(num4, this.minDragDistance, num4);
					this.dragTargetPosition = this.dragStartPosition + this.pBook.transform.right * num4;
					this.maxDragDistance = Mathf.Abs((Input.mousePosition - this.cam.WorldToScreenPoint(this.dragTargetPosition)).x);
					this.dragstate = ExampleDragController.DragState.CLOSEDRAG;
				}
			}
			if (Input.GetMouseButton(0) && (this.dragstate == ExampleDragController.DragState.OPENDRAG || this.dragstate == ExampleDragController.DragState.NEXTDRAG || this.dragstate == ExampleDragController.DragState.PREVDRAG))
			{
				float num5 = Mathf.Abs((Input.mousePosition - this.cam.WorldToScreenPoint(this.dragStartPosition)).x);
				float num6 = Mathf.Abs((Input.mousePosition - this.cam.WorldToScreenPoint(this.dragTargetPosition)).x);
				this.dragValue = 1f - 1f / this.maxDragDistance * num6;
				this.dragValue = Mathf.Clamp(this.dragValue, 0f, 1f);
				if (num5 <= this.maxDragDistance && this.dragstate == ExampleDragController.DragState.OPENDRAG)
				{
					this.pBook.DragOpenBook(this.dragValue);
				}
				if (num5 <= this.maxDragDistance && this.dragstate == ExampleDragController.DragState.NEXTDRAG)
				{
					this.pBook.DragNextPage(this.dragValue);
				}
				if (num5 <= this.maxDragDistance && this.dragstate == ExampleDragController.DragState.PREVDRAG)
				{
					this.pBook.DragPrevPage(this.dragValue);
				}
			}
			if (Input.GetMouseButton(1) && this.dragstate == ExampleDragController.DragState.CLOSEDRAG)
			{
				float num7 = Mathf.Abs((Input.mousePosition - this.cam.WorldToScreenPoint(this.dragStartPosition)).x);
				float num8 = Mathf.Abs((Input.mousePosition - this.cam.WorldToScreenPoint(this.dragTargetPosition)).x);
				this.dragValue = 1f - 1f / this.maxDragDistance * num8;
				this.dragValue = Mathf.Clamp(this.dragValue, 0f, 1f);
				if (num7 <= this.maxDragDistance)
				{
					this.pBook.DragCloseBook(this.dragValue);
				}
			}
			if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
			{
				this.CancelDrag();
			}
			if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && this.dragstate != ExampleDragController.DragState.NODRAG)
			{
				this.CancelDrag();
			}
		}
	}

	// Token: 0x060000AF RID: 175 RVA: 0x0000587C File Offset: 0x00003A7C
	private void CancelDrag()
	{
		if (this.pBook.GetBookState() != PBook.BookState.OPEN && this.pBook.GetBookState() != PBook.BookState.CLOSED)
		{
			if (this.dragstate == ExampleDragController.DragState.OPENDRAG || this.dragstate == ExampleDragController.DragState.CLOSEDRAG)
			{
				this.pBook.CancelDragOpenCloseBook();
				this.dragstate = ExampleDragController.DragState.NODRAG;
				return;
			}
			if (this.dragstate == ExampleDragController.DragState.NEXTDRAG || this.dragstate == ExampleDragController.DragState.PREVDRAG)
			{
				this.pBook.CancelDragNextPrevPage();
				this.dragstate = ExampleDragController.DragState.NODRAG;
			}
		}
	}

	// Token: 0x0400007B RID: 123
	public PBook pBook;

	// Token: 0x0400007C RID: 124
	public LayerMask bookLayer;

	// Token: 0x0400007D RID: 125
	public float raycastDistance = 3f;

	// Token: 0x0400007E RID: 126
	public Camera useThisCamera;

	// Token: 0x0400007F RID: 127
	private ExampleDragController.DragState dragstate;

	// Token: 0x04000080 RID: 128
	private Vector3 dragStartPosition;

	// Token: 0x04000081 RID: 129
	private Vector3 dragTargetPosition;

	// Token: 0x04000082 RID: 130
	private float maxDragDistance;

	// Token: 0x04000083 RID: 131
	private float dragValue;

	// Token: 0x04000084 RID: 132
	private Collider bookCollider;

	// Token: 0x04000085 RID: 133
	private float minDragDistance;

	// Token: 0x04000086 RID: 134
	private Camera cam;

	// Token: 0x02000479 RID: 1145
	private enum DragState
	{
		// Token: 0x04002165 RID: 8549
		NODRAG,
		// Token: 0x04002166 RID: 8550
		OPENDRAG,
		// Token: 0x04002167 RID: 8551
		CLOSEDRAG,
		// Token: 0x04002168 RID: 8552
		NEXTDRAG,
		// Token: 0x04002169 RID: 8553
		PREVDRAG
	}
}
