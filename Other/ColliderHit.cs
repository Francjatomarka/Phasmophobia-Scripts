using System;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class ColliderHit : MonoBehaviour
{
	// Token: 0x06000697 RID: 1687 RVA: 0x000271F1 File Offset: 0x000253F1
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Collision Enter: " + collision.gameObject.name);
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0002720D File Offset: 0x0002540D
	private void OnCollisionExit(Collision collision)
	{
		Debug.Log("Collision Exit: " + collision.gameObject.name);
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x00027229 File Offset: 0x00025429
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Trigger Stay: " + other.gameObject.name);
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x00027229 File Offset: 0x00025429
	private void OnTriggerExit(Collider other)
	{
		Debug.Log("Trigger Stay: " + other.gameObject.name);
	}
}
