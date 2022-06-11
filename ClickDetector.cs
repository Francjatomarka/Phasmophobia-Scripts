using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x02000071 RID: 113
public class ClickDetector : MonoBehaviour
{
	// Token: 0x06000292 RID: 658 RVA: 0x00011554 File Offset: 0x0000F754
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

	// Token: 0x06000293 RID: 659 RVA: 0x000115DC File Offset: 0x0000F7DC
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
