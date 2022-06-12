using System;
using UnityEngine;

public class EMFReaderTrigger : MonoBehaviour
{
	private void Awake()
	{
		this.emfReader = base.GetComponentInParent<EMFReader>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("EMF") && other.GetComponent<EMF>())
		{
			this.emfReader.AddEMFZone(other.GetComponent<EMF>());
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("EMF") && other.GetComponent<EMF>())
		{
			this.emfReader.RemoveEMFZone(other.GetComponent<EMF>());
		}
	}

	private EMFReader emfReader;
}

