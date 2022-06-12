using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RpsDebug : MonoBehaviour
{
	public void ToggleConnectionDebug()
	{
		this.ShowConnectionDebug = !this.ShowConnectionDebug;
	}

	public void Update()
	{
		this.ConnectionDebugButton.GetComponentInChildren<Text>().text = "";
	}

	[SerializeField]
	private Button ConnectionDebugButton;

	public bool ShowConnectionDebug;
}

