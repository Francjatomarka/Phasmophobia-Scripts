using System;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class ParabolicMicrophoneTrigger : MonoBehaviour
{
	// Token: 0x0600080D RID: 2061 RVA: 0x00030E04 File Offset: 0x0002F004
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

	// Token: 0x0400080A RID: 2058
	[SerializeField]
	private ParabolicMicrophone microphone;
}
