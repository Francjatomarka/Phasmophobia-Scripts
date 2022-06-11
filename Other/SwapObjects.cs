using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class SwapObjects : MonoBehaviour
{
	// Token: 0x06000138 RID: 312 RVA: 0x00009937 File Offset: 0x00007B37
	private void Start()
	{
		this.A.SetActive(true);
		this.B.SetActive(false);
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00009951 File Offset: 0x00007B51
	private void Update()
	{
		if (Input.GetKeyDown(this.Key))
		{
			this.A.SetActive(this.B.activeInHierarchy);
			this.B.SetActive(!this.B.activeInHierarchy);
		}
	}

	// Token: 0x0400018C RID: 396
	public KeyCode Key = KeyCode.Space;

	// Token: 0x0400018D RID: 397
	public GameObject A;

	// Token: 0x0400018E RID: 398
	public GameObject B;
}
