using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DragToMove : MonoBehaviour
{
	public void Start()
	{
		this.cubeStartPositions = new Vector3[this.cubes.Length];
		for (int i = 0; i < this.cubes.Length; i++)
		{
			Transform transform = this.cubes[i];
			this.cubeStartPositions[i] = transform.position;
		}
	}

	public void Update()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (this.recording)
		{
			return;
		}
		if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
		{
			base.StartCoroutine("RecordMouse");
			return;
		}
		if (this.PositionsQueue.Count == 0)
		{
			return;
		}
		Vector3 a = this.PositionsQueue[this.nextPosIndex];
		int index = (this.nextPosIndex > 0) ? (this.nextPosIndex - 1) : (this.PositionsQueue.Count - 1);
		Vector3 a2 = this.PositionsQueue[index];
		this.lerpTime += Time.deltaTime * this.speed;
		for (int i = 0; i < this.cubes.Length; i++)
		{
			Component component = this.cubes[i];
			Vector3 b = a + this.cubeStartPositions[i];
			Vector3 a3 = a2 + this.cubeStartPositions[i];
			component.transform.position = Vector3.Lerp(a3, b, this.lerpTime);
		}
		if (this.lerpTime > 1f)
		{
			this.nextPosIndex = (this.nextPosIndex + 1) % this.PositionsQueue.Count;
			this.lerpTime = 0f;
		}
	}

	public IEnumerator RecordMouse()
	{
		this.recording = true;
		this.PositionsQueue.Clear();
		while (Input.GetMouseButton(0) || Input.touchCount > 0)
		{
			yield return new WaitForSeconds(0.1f);
			Vector3 pos = Input.mousePosition;
			if (Input.touchCount > 0)
			{
				pos = Input.GetTouch(0).position;
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(pos), out raycastHit))
			{
				this.PositionsQueue.Add(raycastHit.point);
			}
		}
		this.nextPosIndex = 0;
		this.recording = false;
		yield break;
	}

	public float speed = 5f;

	public Transform[] cubes;

	public List<Vector3> PositionsQueue = new List<Vector3>(20);

	private Vector3[] cubeStartPositions;

	private int nextPosIndex;

	private float lerpTime;

	private bool recording;
}

