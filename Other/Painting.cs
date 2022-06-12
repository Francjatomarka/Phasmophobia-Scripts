using System;
using System.Collections;
using UnityEngine;

public class Painting : MonoBehaviour
{
	private void Awake()
	{
		this.rigid = base.GetComponent<Rigidbody>();
		this.noise = base.GetComponentInChildren<Noise>();
	}

	private void Start()
	{
		if (this.noise != null)
		{
			this.noise.gameObject.SetActive(false);
		}
	}

	public void KnockOver()
	{
		this.rigid.isKinematic = false;
		this.rigid.useGravity = true;
		base.enabled = false;
		if (this.noise != null)
		{
			this.PlayNoiseObject();
		}
	}

	private IEnumerator PlayNoiseObject()
	{
		this.noise.gameObject.SetActive(true);
		yield return 0;
		this.noise.gameObject.SetActive(false);
		yield break;
	}

	private Rigidbody rigid;

	private Noise noise;
}

