using System;
using TLGFPowerBooks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000019 RID: 25
public class ExampleFPSBookController : MonoBehaviour
{
	// Token: 0x060000BC RID: 188 RVA: 0x00005CAE File Offset: 0x00003EAE
	private void Start()
	{
		this.camTr = base.GetComponent<Transform>();
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00005CBC File Offset: 0x00003EBC
	private void Update()
	{
		this.pointer.color = Color.white;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.camTr.position + this.camTr.forward * this.raycastStartDistance, this.camTr.forward, out raycastHit, this.raycastDistance, this.bookLayer.value) && ((raycastHit.transform.parent != null && raycastHit.transform.parent.GetComponent<PBook>() != null) || (raycastHit.transform.parent.parent != null && raycastHit.transform.parent.parent.GetComponent<PBook>() != null)))
		{
			this.pointer.color = Color.red;
		}
		if (Input.GetKeyDown(this.openCloseBookKey) && this.activePowerBook == null)
		{
			if (Physics.Raycast(this.camTr.position + this.camTr.forward * this.raycastStartDistance, this.camTr.forward, out raycastHit, this.raycastDistance, this.bookLayer.value) && raycastHit.transform.parent != null && raycastHit.transform.parent.GetComponent<PBook>() != null)
			{
				this.activePowerBook = raycastHit.transform.parent.GetComponent<PBook>();
				if (this.activePowerBook.GetBookState() == PBook.BookState.CLOSED)
				{
					this.activePowerBook.OpenBook();
				}
				this.activePowerBook = null;
			}
			if (Physics.Raycast(this.camTr.position + this.camTr.forward * this.raycastStartDistance, this.camTr.forward, out raycastHit, this.raycastDistance, this.bookLayer.value) && raycastHit.transform.parent.parent != null && raycastHit.transform.parent.parent.GetComponent<PBook>() != null)
			{
				this.activePowerBook = raycastHit.transform.parent.parent.GetComponent<PBook>();
				if (this.activePowerBook.GetBookState() == PBook.BookState.OPEN)
				{
					this.activePowerBook.CloseBook();
				}
				this.activePowerBook = null;
			}
		}
		if (Input.GetKeyDown(this.prevPageKey) && this.activePowerBook == null && Physics.Raycast(this.camTr.position + this.camTr.forward * this.raycastStartDistance, this.camTr.forward, out raycastHit, this.raycastDistance, this.bookLayer.value) && raycastHit.transform.parent.parent != null && raycastHit.transform.parent.parent.GetComponent<PBook>() != null)
		{
			this.activePowerBook = raycastHit.transform.parent.parent.GetComponent<PBook>();
			if (this.activePowerBook.GetBookState() == PBook.BookState.OPEN)
			{
				this.activePowerBook.PrevPage();
			}
			this.activePowerBook = null;
		}
		if (Input.GetKeyDown(this.nextPageKey) && this.activePowerBook == null && Physics.Raycast(this.camTr.position + this.camTr.forward * this.raycastStartDistance, this.camTr.forward, out raycastHit, this.raycastDistance, this.bookLayer.value) && raycastHit.transform.parent.parent != null && raycastHit.transform.parent.parent.GetComponent<PBook>() != null)
		{
			this.activePowerBook = raycastHit.transform.parent.parent.GetComponent<PBook>();
			if (this.activePowerBook.GetBookState() == PBook.BookState.OPEN)
			{
				this.activePowerBook.NextPage();
			}
			this.activePowerBook = null;
		}
	}

	// Token: 0x04000090 RID: 144
	public LayerMask bookLayer;

	// Token: 0x04000091 RID: 145
	public float raycastDistance = 3f;

	// Token: 0x04000092 RID: 146
	public float raycastStartDistance;

	// Token: 0x04000093 RID: 147
	public KeyCode openCloseBookKey;

	// Token: 0x04000094 RID: 148
	public KeyCode nextPageKey;

	// Token: 0x04000095 RID: 149
	public KeyCode prevPageKey;

	// Token: 0x04000096 RID: 150
	public Image pointer;

	// Token: 0x04000097 RID: 151
	private Transform camTr;

	// Token: 0x04000098 RID: 152
	private PBook activePowerBook;
}
