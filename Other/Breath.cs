using System;
using UnityEngine;

public class Breath : MonoBehaviour
{
	private void Awake()
	{
		this.particles = base.GetComponent<ParticleSystemRenderer>();
	}

	private void Start()
	{
		this.particles.maxParticleSize = 0f;
		this.strength = 0f;
		if (MainManager.instance)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		if (LevelController.instance)
		{
			if (this.player.currentRoom.temperature < 0f)
			{
				this.strength = this.player.currentRoom.temperature * -1f * 10f;
				this.particles.maxParticleSize = this.strength / 3f;
				return;
			}
			this.strength = 0f;
			this.particles.maxParticleSize = 0f;
		}
	}

	[HideInInspector]
	public ParticleSystemRenderer particles;

	private float strength;

	[SerializeField]
	private Player player;
}

