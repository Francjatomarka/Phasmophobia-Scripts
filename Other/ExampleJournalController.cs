using System;
using System.Collections;
using TLGFPowerBooks;
using UnityEngine;

public class ExampleJournalController : MonoBehaviour
{
	private void Start()
	{
		this.cam = Camera.main;
		this.activePowerBook = null;
		if (this.useThisCameraInsteadOfMain != null)
		{
			this.cam = this.useThisCameraInsteadOfMain;
		}
	}

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

	public void CloseOverlay()
	{
		if (this.activePowerBook != null && this.bookIsOpen)
		{
			base.StartCoroutine(this.CloseOverlayAnim());
		}
	}

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

	public void SetLayer(GameObject parent, int layer)
	{
		parent.layer = layer;
		Transform[] componentsInChildren = parent.transform.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = layer;
		}
	}

	public string overlayLayer;

	public Camera overlayCam;

	public float raycastDistance = 3f;

	[Range(0f, 15f)]
	public float perspectiveAngle;

	public bool autoOpenBook;

	public KeyCode openCloseKey;

	public KeyCode nextPageKey;

	public KeyCode prevPageKey;

	public GameObject[] disabledGameObjectsWhileReading;

	public Camera useThisCameraInsteadOfMain;

	private Camera cam;

	private PBook activePowerBook;

	private Vector3 activePowerBookOriginalPos;

	private Quaternion activePowerBookOriginalRot;

	private LayerMask activePowerBookOriginalLayer;

	private bool bookIsOpen;
}

