using System;
using UnityEngine;

public class SanityDrainer : MonoBehaviour
{
	private void Awake()
	{
		this.ghostAI = base.GetComponent<GhostAI>();
	}

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

	private void OnDisable()
	{
		this.ghostIsVisible = false;
		if (this.source != null)
		{
			this.source.Stop();
		}
	}

	private void Start()
	{
		if (this.ghostAI.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Phantom)
		{
			this.strength = 0.4f;
		}
	}

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

	private Vector3 viewPos;

	private GhostAI ghostAI;

	[SerializeField]
	private LayerMask mask;

	private AudioSource source;

	private bool ghostIsVisible;

	private float strength = 0.2f;
}

