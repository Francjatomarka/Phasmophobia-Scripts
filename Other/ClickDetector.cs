using System;
using UnityEngine;
using Photon.Pun;

public class ClickDetector : MonoBehaviour
{
	public void Update()
	{
		if (int.Parse(PhotonNetwork.LocalPlayer.UserId) != GameLogic.playerWhoIsIt)
		{
			return;
		}
		if (Input.GetButton("Fire1"))
		{
			GameObject gameObject = this.RaycastObject(Input.mousePosition);
			if (gameObject != null && gameObject != base.gameObject && gameObject.name.Equals("monsterprefab(Clone)", StringComparison.OrdinalIgnoreCase))
			{
				GameLogic.TagPlayer(int.Parse(gameObject.transform.root.GetComponent<PhotonView>().Owner.UserId));
			}
		}
	}

	private GameObject RaycastObject(Vector2 screenPos)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPos), out raycastHit, 200f))
		{
			return raycastHit.collider.gameObject;
		}
		return null;
	}
}

