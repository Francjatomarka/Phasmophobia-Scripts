using System;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class SoundSensorTrigger : MonoBehaviour
{
	// Token: 0x06000829 RID: 2089 RVA: 0x00031B5C File Offset: 0x0002FD5C
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Noise>())
		{
			Noise component = other.GetComponent<Noise>();
			if (component.volume > this.soundSensor.highestVolume)
			{
				this.soundSensor.highestVolume = component.volume;
			}
		}
	}

	// Token: 0x0400082C RID: 2092
	[SerializeField]
	private SoundSensor soundSensor;
}
