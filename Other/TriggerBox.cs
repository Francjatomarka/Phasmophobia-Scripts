using System;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class TriggerBox : MonoBehaviour
{
	// Token: 0x060006BA RID: 1722 RVA: 0x00027B11 File Offset: 0x00025D11
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.gameObject.name);
	}
}
