using System;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class SanityDrainer : MonoBehaviour
{
	// Token: 0x06000523 RID: 1315 RVA: 0x0001C54F File Offset: 0x0001A74F
	private void Awake()
	{
		this.ghostAI = base.GetComponent<GhostAI>();
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x0001C560 File Offset: 0x0001A760
	private void OnEnable()
	{
		if (GameController.instance == null)
		{
			base.enabled = false;
			return;
		}
		if (GameController.instance.myPlayer == null)
		{
			base.enabled = false;
			return;
		}
		if (GameController.instance.myPlayer.player == null)
		{
			base.enabled = false;
			return;
		}
		this.ghostIsVisible = false;
		if (!(this.source == null))
		{
			return;
		}
		if (GameController.instance == null)
		{
			return;
		}
		if (GameController.instance.myPlayer != null)
		{
			this.source = GameController.instance.myPlayer.player.heartBeatAudioSource;
			return;
		}
		base.enabled = false;
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x0001C607 File Offset: 0x0001A807
	private void OnDisable()
	{
		this.ghostIsVisible = false;
		if (this.source != null)
		{
			this.source.Stop();
		}
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0001C629 File Offset: 0x0001A829
	private void Start()
	{
		if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Phantom)
		{
			this.strength = 0.4f;
		}
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x0001C650 File Offset: 0x0001A850
	private void Update()
	{
		this.ghostIsVisible = false;
		this.viewPos = GameController.instance.myPlayer.player.cam.WorldToViewportPoint(base.transform.position);
		if ((this.ghostAI.myRends[0].isVisible || this.ghostAI.isHunting) && (this.ghostAI.ghostIsAppeared || this.ghostAI.isHunting) && this.viewPos.x > 0f && this.viewPos.x < 1f && this.viewPos.y > 0f && this.viewPos.y < 1f && !Physics.Linecast(base.transform.position, GameController.instance.myPlayer.player.headObject.transform.position, this.mask) && Vector3.Distance(base.transform.position, GameController.instance.myPlayer.player.headObject.transform.position) < 10f)
		{
			GameController.instance.myPlayer.player.insanity += Time.deltaTime * this.strength;
			this.ghostIsVisible = true;
			if (!this.source.isPlaying)
			{
				this.source.Play();
			}
		}
		if (!this.ghostIsVisible && this.source.isPlaying)
		{
			this.source.Stop();
		}
	}

	// Token: 0x040004DF RID: 1247
	private Vector3 viewPos;

	// Token: 0x040004E0 RID: 1248
	private GhostAI ghostAI;

	// Token: 0x040004E1 RID: 1249
	[SerializeField]
	private LayerMask mask;

	// Token: 0x040004E2 RID: 1250
	private AudioSource source;

	// Token: 0x040004E3 RID: 1251
	private bool ghostIsVisible;

	// Token: 0x040004E4 RID: 1252
	private float strength = 0.2f;
}
