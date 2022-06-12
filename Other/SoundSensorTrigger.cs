using System;
using UnityEngine;

public class SoundSensorTrigger : MonoBehaviour
{
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

	[SerializeField]
	private SoundSensor soundSensor;
}

