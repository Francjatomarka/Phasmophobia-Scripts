using System;
using UnityEngine;

public class ParabolicMicrophoneTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (this.microphone.isOn && other.GetComponent<Noise>())
		{
			Noise component = other.GetComponent<Noise>();
			if (!this.microphone.noises.Contains(component))
			{
				this.microphone.noises.Add(component);
			}
		}
	}

	[SerializeField]
	private ParabolicMicrophone microphone;
}

