using System;
using System.Collections;
using TLGFPowerBooks;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class ExampleJournalController : MonoBehaviour
{
	// Token: 0x060000BF RID: 191 RVA: 0x000060F6 File Offset: 0x000042F6
	private void Start()
	{
		this.cam = Camera.main;
		this.activePowerBook = null;
		if (this.useThisCameraInsteadOfMain != null)
		{
			this.cam = this.useThisCameraInsteadOfMain;
		}
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00006124 File Offset: 0x00004324
	private void Update()
	{
		if (!this.bookIsOpen)
		{
			RaycastHit raycastHit;
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(this.cam.ScreenPointToRay(Input.mousePosition), out raycastHit, this.raycastDistance) && raycastHit.transform.parent != null && raycastHit.transform.parent.GetComponent<PBook>() != null)
			{
				this.activePowerBook = raycastHit.transform.parent.GetComponent<PBook>();
				this.activePowerBookOriginalLayer = this.activePowerBook.gameObject.layer;
				this.SetLayer(this.activePowerBook.gameObject, LayerMask.NameToLayer(this.overlayLayer));
				BoxCollider boxCollider = (BoxCollider)raycastHit.collider;
				this.activePowerBookOriginalPos = raycastHit.transform.position;
				this.activePowerBookOriginalRot = raycastHit.transform.rotation;
				this.activePowerBook.transform.position = new Vector3(boxCollider.size.y / 2f - 0.005f, 0f, -boxCollider.size.z / 2f);
				this.activePowerBook.transform.rotation = Quaternion.Euler(this.perspectiveAngle, 0f, 0f);
				this.overlayCam.transform.position = new Vector3(0f, boxCollider.size.z * 1.4f - boxCollider.size.y / 2f - this.perspectiveAngle * 0.003f, -this.perspectiveAngle * 0.001f);
				this.overlayCam.gameObject.SetActive(true);
				if (this.autoOpenBook)
				{
					this.activePowerBook.OpenBook();
				}
				this.bookIsOpen = true;
				GameObject[] array = this.disabledGameObjectsWhileReading;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(false);
				}
				return;
			}
		}
		else
		{
			if (Input.GetKey(this.openCloseKey))
			{
				if (this.activePowerBook.GetBookState() == PBook.BookState.CLOSED)
				{
					this.activePowerBook.OpenBook();
				}
				else if (this.activePowerBook.GetBookState() == PBook.BookState.OPEN)
				{
					this.CloseOverlay();
				}
			}
			if (Input.GetKey(this.nextPageKey))
			{
				this.activePowerBook.NextPage();
			}
			if (Input.GetKey(this.prevPageKey))
			{
				this.activePowerBook.PrevPage();
			}
		}
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x0000638F File Offset: 0x0000458F
	public void CloseOverlay()
	{
		if (this.activePowerBook != null && this.bookIsOpen)
		{
			base.StartCoroutine(this.CloseOverlayAnim());
		}
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x000063B4 File Offset: 0x000045B4
	private IEnumerator CloseOverlayAnim()
	{
		this.activePowerBook.CloseBook();
		yield return new WaitUntil(() => this.activePowerBook.GetBookState() == PBook.BookState.CLOSED);
		this.activePowerBook.transform.position = this.activePowerBookOriginalPos;
		this.activePowerBook.transform.rotation = this.activePowerBookOriginalRot;
		this.SetLayer(this.activePowerBook.gameObject, this.activePowerBookOriginalLayer);
		this.overlayCam.gameObject.SetActive(false);
		this.bookIsOpen = false;
		GameObject[] array = this.disabledGameObjectsWhileReading;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
		yield return true;
		yield break;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x000063C4 File Offset: 0x000045C4
	public void SetLayer(GameObject parent, int layer)
	{
		parent.layer = layer;
		Transform[] componentsInChildren = parent.transform.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = layer;
		}
	}

	// Token: 0x04000099 RID: 153
	public string overlayLayer;

	// Token: 0x0400009A RID: 154
	public Camera overlayCam;

	// Token: 0x0400009B RID: 155
	public float raycastDistance = 3f;

	// Token: 0x0400009C RID: 156
	[Range(0f, 15f)]
	public float perspectiveAngle;

	// Token: 0x0400009D RID: 157
	public bool autoOpenBook;

	// Token: 0x0400009E RID: 158
	public KeyCode openCloseKey;

	// Token: 0x0400009F RID: 159
	public KeyCode nextPageKey;

	// Token: 0x040000A0 RID: 160
	public KeyCode prevPageKey;

	// Token: 0x040000A1 RID: 161
	public GameObject[] disabledGameObjectsWhileReading;

	// Token: 0x040000A2 RID: 162
	public Camera useThisCameraInsteadOfMain;

	// Token: 0x040000A3 RID: 163
	private Camera cam;

	// Token: 0x040000A4 RID: 164
	private PBook activePowerBook;

	// Token: 0x040000A5 RID: 165
	private Vector3 activePowerBookOriginalPos;

	// Token: 0x040000A6 RID: 166
	private Quaternion activePowerBookOriginalRot;

	// Token: 0x040000A7 RID: 167
	private LayerMask activePowerBookOriginalLayer;

	// Token: 0x040000A8 RID: 168
	private bool bookIsOpen;
}
