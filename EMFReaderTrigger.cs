using System;
using UnityEngine;

// Token: 0x0200010F RID: 271
public class EMFReaderTrigger : MonoBehaviour
{
	// Token: 0x06000778 RID: 1912 RVA: 0x0002C64A File Offset: 0x0002A84A
	private void Awake()
	{
		this.emfReader = base.GetComponentInParent<EMFReader>();
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x0002C658 File Offset: 0x0002A858
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("EMF") && other.GetComponent<EMF>())
		{
			this.emfReader.AddEMFZone(other.GetComponent<EMF>());
		}
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x0002C685 File Offset: 0x0002A885
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("EMF") && other.GetComponent<EMF>())
		{
			this.emfReader.RemoveEMFZone(other.GetComponent<EMF>());
		}
	}

	// Token: 0x0400078B RID: 1931
	private EMFReader emfReader;
}
