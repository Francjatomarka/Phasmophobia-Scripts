using System;
using Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrailerVRController : MonoBehaviour
{
	private void Awake()
	{
		if (SceneManager.GetActiveScene().name == "Menu_New")
		{
			base.enabled = false;
		}
	}
}

