using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class ClickAndDrag : MonoBehaviourPunCallbacks
{
	// Token: 0x0600016C RID: 364 RVA: 0x0000A3F8 File Offset: 0x000085F8
	private void Update()
	{
		if (!base.photonView.IsMine)
		{
			return;
		}
		InputToEvent component = Camera.main.GetComponent<InputToEvent>();
		if (component == null)
		{
			return;
		}
		if (!this.following)
		{
			if (component.Dragging)
			{
				this.camOnPress = base.transform.position;
				this.following = true;
				return;
			}
			return;
		}
		else
		{
			if (component.Dragging)
			{
				Vector3 b = this.camOnPress - new Vector3(component.DragVector.x, 0f, component.DragVector.y) * this.factor;
				base.transform.position = Vector3.Lerp(base.transform.position, b, Time.deltaTime * 0.5f);
				return;
			}
			this.camOnPress = Vector3.zero;
			this.following = false;
			return;
		}
	}

	// Token: 0x040001AD RID: 429
	private Vector3 camOnPress;

	// Token: 0x040001AE RID: 430
	private bool following;

	// Token: 0x040001AF RID: 431
	private float factor = -0.1f;
}
