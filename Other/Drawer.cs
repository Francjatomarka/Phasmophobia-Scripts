using System;
using UnityEngine;
using Photon.Pun;

public class Drawer : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.body = base.GetComponent<Rigidbody>();
		this.startPos = base.transform.localPosition;
		this.startWorldPos = base.transform.position;
		this.closed = true;
	}


	private void Start()
	{
		if (this.loopSource)
		{
			this.loopSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		}
		if (this.closedSource)
		{
			this.closedSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		}
	}

	private void Update()
	{
		if (this.isZ)
		{
			if (base.transform.localPosition.z < this.startPos.z)
			{
				this.pos = base.transform.localPosition;
				this.pos.z = this.startPos.z;
				base.transform.localPosition = this.pos;
			}
		}
		else if (this.isX)
		{
			if (base.transform.localPosition.x < this.startPos.x)
			{
				this.pos = base.transform.localPosition;
				this.pos.x = this.startPos.x;
				base.transform.localPosition = this.pos;
			}
		}
		else if (this.isY && base.transform.localPosition.y < this.startPos.y)
		{
			this.pos = base.transform.localPosition;
			this.pos.y = this.startPos.y;
			base.transform.localPosition = this.pos;
		}
		if (Time.frameCount % 3 == 0)
		{
			if (!this.closed)
			{
				if (!this.loopSource.isPlaying)
				{
					this.loopSource.Play();
				}
				this.velocity = (base.transform.position - this.oldPos).magnitude / Time.deltaTime;
				this.oldPos = base.transform.position;
				this.loopSource.volume = this.velocity / 6f;
				return;
			}
			if (this.loopSource.isPlaying)
			{
				this.loopSource.Stop();
			}
		}
	}

	public void UnGrab()
	{
		Vector3 localPosition = base.transform.localPosition;
		if (Vector3.Distance(base.transform.position, this.startWorldPos) <= 0.03f)
		{
			if (this.isZ)
			{
				localPosition.z = 0f;
			}
			else if (this.isY)
			{
				localPosition.y = 0f;
			}
			else if (this.isX)
			{
				localPosition.z = 0f;
			}
			this.view.RPC("NetworkedPlayClosedSound", RpcTarget.All, Array.Empty<object>());
			base.transform.localPosition = localPosition;
		}
	}

	public void Grab()
	{
		this.view.RPC("NetworkedGrab", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	private void NetworkedGrab()
	{
		this.closed = false;
	}


	[PunRPC]
	private void NetworkedPlayClosedSound()
	{
		this.closed = true;
		if (this.closedSource.isPlaying)
		{
			return;
		}
		if (this.doorClosedClips.Length != 0)
		{
			this.closedSource.clip = this.doorClosedClips[UnityEngine.Random.Range(0, this.doorClosedClips.Length)];
			this.closedSource.Play();
			return;
		}
		Debug.LogError(base.gameObject.name + " needs a drawer closing audio clip");
	}

	private Vector3 startPos;

	public bool isX;

	public bool isY;

	public bool isZ = true;

	private Vector3 pos;

	private Rigidbody body;

	private PhotonView view;

	[HideInInspector]
	public bool closed;

	[SerializeField]
	private AudioClip[] doorClosedClips;

	[SerializeField]
	private AudioSource loopSource;

	[SerializeField]
	private AudioSource closedSource;

	private Vector3 startWorldPos;

	private Vector3 oldPos;

	private float velocity;
}

