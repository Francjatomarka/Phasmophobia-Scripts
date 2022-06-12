using System;
using UnityEngine;
using UnityEngine.Events;

public class VRTeleportGrab : MonoBehaviour
{
	private void Awake()
	{
		
	}

	private void OnEnable()
	{
		
	}

	private void OnDisable()
	{
		
	}

	private void GrabPressed()
	{
		
	}

	private void AttemptGrab()
	{
		for (int i = 0; i < this.raycastPoints.Length; i++)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.raycastPoints[i].position, this.raycastPoints[i].forward, out raycastHit, 1.5f, this.mask, QueryTriggerInteraction.Ignore) && raycastHit.collider.GetComponent<Prop>())
			{
				PhotonObjectInteract component = raycastHit.collider.GetComponent<PhotonObjectInteract>();
				if (!component.isGrabbed)
				{
					if (!component.view.IsMine)
					{
						component.view.RequestOwnership();
					}
					component.transform.position = base.transform.position;
				}
			}
		}
	}

	[SerializeField]
	private Transform[] raycastPoints;

	[SerializeField]
	private LayerMask mask;
}

