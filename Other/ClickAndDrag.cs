using System;
using Photon.Pun;
using UnityEngine;

public class ClickAndDrag : MonoBehaviourPunCallbacks
{
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

	private Vector3 camOnPress;

	private bool following;

	private float factor = -0.1f;
}

