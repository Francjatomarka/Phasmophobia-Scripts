using System;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class TrailerCamera : MonoBehaviour
{
	// Token: 0x06000ABD RID: 2749 RVA: 0x000427BB File Offset: 0x000409BB
	private void Awake()
	{
		base.transform.SetParent(null);
		if (this.cam == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x000427DE File Offset: 0x000409DE
	private void OnEnable()
	{
		base.transform.position = this.cam.transform.position;
		base.transform.rotation = this.cam.transform.rotation;
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x00042818 File Offset: 0x00040A18
	private void Update()
	{
		if (this.cam != null)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.cam.transform.position, Time.deltaTime * this.posSpeed);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.cam.transform.rotation, Time.deltaTime * this.rotSpeed);
		}
	}

	// Token: 0x04000B25 RID: 2853
	[SerializeField]
	private Camera cam;

	// Token: 0x04000B26 RID: 2854
	[SerializeField]
	private float posSpeed = 5f;

	// Token: 0x04000B27 RID: 2855
	[SerializeField]
	private float rotSpeed = 5f;
}
