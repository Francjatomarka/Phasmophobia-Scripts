using System;
using UnityEngine;
using Photon.Pun;

public class Footstep : MonoBehaviour
{
	private void OnEnable()
	{
		if (LevelController.instance == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (LevelController.instance.currentGhost == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType != GhostTraits.Type.Wraith)
		{
			this.rend.enabled = LevelController.instance.currentGhost.isHunting;
			this.rend.enabled = LevelController.instance.currentGhost.ghostInteraction.hasWalkedInSalt;
		}
		else
		{
			this.rend.enabled = false;
		}
		if (LevelController.instance.currentGhost.ghostIsAppeared)
		{
			if (LevelController.instance.currentGhost.agent.velocity.magnitude > 0.1f)
			{
				this.src.clip = this.huntingNoises[UnityEngine.Random.Range(0, this.huntingNoises.Length)];
				this.src.volume = 1f;
				if (Physics.Linecast(LevelController.instance.currentGhost.raycastPoint.position, GameController.instance.myPlayer.player.headObject.transform.position, LevelController.instance.currentGhost.mask, QueryTriggerInteraction.Ignore))
				{
					this.src.volume = 0.3f;
				}
			}
		}
		else
		{
			this.src.clip = this.footstepNoises[UnityEngine.Random.Range(0, this.footstepNoises.Length)];
			if (Physics.Linecast(LevelController.instance.currentGhost.raycastPoint.position, GameController.instance.myPlayer.player.headObject.transform.position, LevelController.instance.currentGhost.mask, QueryTriggerInteraction.Ignore))
			{
				this.src.volume = 0.05f;
			}
		}
		if (LevelController.instance.currentGhost.isHunting)
		{
			this.src.clip = this.huntingNoises[UnityEngine.Random.Range(0, this.huntingNoises.Length)];
			this.src.volume = 1f;
			if (Physics.Linecast(LevelController.instance.currentGhost.raycastPoint.position, GameController.instance.myPlayer.player.headObject.transform.position, LevelController.instance.currentGhost.mask, QueryTriggerInteraction.Ignore))
			{
				this.src.volume = 0.3f;
			}
		}
		this.src.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(this.src.transform.position.y);
		this.src.Play();
	}

	[PunRPC]
	public void Spawn(bool isRight)
	{
		this.timer = UnityEngine.Random.Range(10f, 15f);
		this.rend.material.mainTexture = (isRight ? this.leftTexture : this.RightTexture);
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x += (isRight ? 0.1f : -0.1f);
		base.transform.localPosition = localPosition;
	}

	private void Update()
	{
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			base.gameObject.SetActive(false);
		}
	}

	[SerializeField]
	private Texture leftTexture;

	[SerializeField]
	private Texture RightTexture;

	[SerializeField]
	private AudioSource src;

	private float timer = 10f;

	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private AudioClip[] footstepNoises = new AudioClip[0];

	[SerializeField]
	private AudioClip[] huntingNoises = new AudioClip[0];

	[SerializeField]
	private Renderer rend;
}

