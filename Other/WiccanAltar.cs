using System;
using UnityEngine;
using Photon.Pun;

public class WiccanAltar : MonoBehaviour
{
	[SerializeField]
	private Transform[] markers;

	[SerializeField]
	private Candle candle_1;

	[SerializeField]
	private Candle candle_2;

	[SerializeField]
	private Light myLight;

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private ParticleSystem particles;

	private PhotonObjectInteract photonInteract;

	private Renderer rend;

	[HideInInspector]
	public PhotonView view;

	public static WiccanAltar instance;

	private bool inUse;
}

