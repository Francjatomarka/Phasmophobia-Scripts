using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class BugsAI : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.bugscale = UnityEngine.Random.Range(this.ScaleRange.x, this.ScaleRange.y);
		base.transform.localScale = new Vector3(this.bugscale, this.bugscale, this.bugscale);
	}

	private void Start()
	{
		this.SetStartingValues();
	}

	private void SetStartingValues()
	{
		this.speed = UnityEngine.Random.Range(this.SpeedRange.x, this.SpeedRange.y);
		this.RunTime = UnityEngine.Random.Range(this.RunRange.x, this.RunRange.y);
		this.waittime = UnityEngine.Random.Range(this.WaitRange.x, this.WaitRange.y);
		if (this.view.IsMine)
		{
			base.StartCoroutine(this.Alive());
		}
		this.BugTargetPos = new Vector3(UnityEngine.Random.Range(-(this.col.size.x / 2f), this.col.size.x / 2f), 0f, UnityEngine.Random.Range(-(this.col.size.z / 2f), this.col.size.z / 2f));
		if (this.view.IsMine)
		{
			base.transform.localPosition = new Vector3(UnityEngine.Random.Range(-(this.col.size.x / 2f), this.col.size.x / 2f), 0f, UnityEngine.Random.Range(-(this.col.size.z / 2f), this.col.size.z / 2f));
		}
	}

	private IEnumerator Alive()
	{
		for (;;)
		{
			this.Sleep = false;
			this.waittime = UnityEngine.Random.Range(this.WaitRange.x, this.WaitRange.y);
			yield return new WaitForSeconds(this.RunTime);
			this.Sleep = true;
			this.RunTime = UnityEngine.Random.Range(this.RunRange.x, this.RunRange.y);
			yield return new WaitForSeconds(this.waittime);
		}
		yield break;
	}

	private void Update()
	{
		if (this.view.IsMine)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				this.BugTargetPos = new Vector3(UnityEngine.Random.Range(-(this.col.size.x / 2f), this.col.size.x / 2f), 0f, UnityEngine.Random.Range(-(this.col.size.z / 2f), this.col.size.z / 2f));
				this.timer = 10f;
			}
			if (this.Sleep)
			{
				return;
			}
			base.transform.Translate(Vector3.forward * this.speed * 1.5f * Time.deltaTime, Space.Self);
			Vector3 forward = this.BugTargetPos - base.transform.localPosition;
			Quaternion localRotation = Quaternion.Slerp(base.transform.localRotation, Quaternion.LookRotation(forward), this.rotSpeed * Time.deltaTime);
			base.transform.localRotation = localRotation;
			base.transform.localEulerAngles = new Vector3(0f, base.transform.localEulerAngles.y, 0f);
		}
	}

	public Vector2 SpeedRange = new Vector2(1f, 3f);

	public Vector2 ScaleRange = new Vector2(1f, 3f);

	public Vector2 RunRange = new Vector2(1f, 5f);

	public Vector2 WaitRange = new Vector2(1f, 5f);

	public float smoothing = 1f;

	public float rotSpeed = 3f;

	[SerializeField]
	private Vector3 BugTargetPos;

	private float speed;

	private Vector3 targetpos;

	private float bugscale;

	private float waittime = 1f;

	private float RunTime = 1f;

	private bool Sleep;

	[HideInInspector]
	public BoxCollider col;

	private float timer = 10f;

	private PhotonView view;
}

