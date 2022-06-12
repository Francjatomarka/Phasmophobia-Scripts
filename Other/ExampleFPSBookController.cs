using System;
using TLGFPowerBooks;
using UnityEngine;
using UnityEngine.UI;

public class ExampleFPSBookController : MonoBehaviour
{
	private void Start()
	{
		this.camTr = base.GetComponent<Transform>();
	}

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

	public LayerMask bookLayer;

	public float raycastDistance = 3f;

	public float raycastStartDistance;

	public KeyCode openCloseBookKey;

	public KeyCode nextPageKey;

	public KeyCode prevPageKey;

	public Image pointer;

	private Transform camTr;

	private PBook activePowerBook;
}

