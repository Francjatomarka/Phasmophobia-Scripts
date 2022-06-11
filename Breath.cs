using System;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class Breath : MonoBehaviour
{
	// Token: 0x06000A50 RID: 2640 RVA: 0x0003FAEA File Offset: 0x0003DCEA
	private void Awake()
	{
		this.particles = base.GetComponent<ParticleSystemRenderer>();
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x0003FAF8 File Offset: 0x0003DCF8
	private void Start()
	{
		this.particles.maxParticleSize = 0f;
		this.strength = 0f;
		if (MainManager.instance)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x0003FB28 File Offset: 0x0003DD28
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

	// Token: 0x04000A75 RID: 2677
	[HideInInspector]
	public ParticleSystemRenderer particles;

	// Token: 0x04000A76 RID: 2678
	private float strength;

	// Token: 0x04000A77 RID: 2679
	[SerializeField]
	private Player player;
}
