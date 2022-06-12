using System;
using UnityEngine;

public class SwapObjects : MonoBehaviour
{
	private void Start()
	{
		this.A.SetActive(true);
		this.B.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(this.Key))
		{
			this.A.SetActive(this.B.activeInHierarchy);
			this.B.SetActive(!this.B.activeInHierarchy);
		}
	}

	public KeyCode Key = KeyCode.Space;

	public GameObject A;

	public GameObject B;
}

