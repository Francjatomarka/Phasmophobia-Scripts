using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(SphereCollider))]
public class EMF : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	public void SetType(EMF.Type type)
	{
		if (type == EMF.Type.GhostInteraction)
		{
			this.strength = 1;
			return;
		}
		if (type == EMF.Type.GhostThrowing)
		{
			this.strength = 2;
			return;
		}
		if (type == EMF.Type.GhostAppeared)
		{
			this.strength = 3;
			return;
		}
		if (type == EMF.Type.GhostEvidence)
		{
			this.strength = 4;
		}
	}

	private void Update()
	{
		this.timerUntilDeath -= Time.deltaTime;
		if (this.timerUntilDeath <= 0f)
		{
			this.timerUntilDeath = 20f;
			for (int i = 0; i < this.emfReaders.Count; i++)
			{
				this.emfReaders[i].RemoveEMFZone(this);
			}
			base.gameObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		if (EMFData.instance && !EMFData.instance.emfSpots.Contains(this))
		{
			EMFData.instance.emfSpots.Add(this);
		}
	}

	private void OnDisable()
	{
		if (EMFData.instance && EMFData.instance.emfSpots.Contains(this))
		{
			EMFData.instance.emfSpots.Remove(this);
		}
	}

	private PhotonView view;

	public List<EMFReader> emfReaders;

	public int strength;

	public EMF.Type type;

	private float timerUntilDeath = 20f;

	public enum Type
	{
		GhostInteraction,
		GhostThrowing,
		GhostAppeared,
		GhostEvidence
	}
}

