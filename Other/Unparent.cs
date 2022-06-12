using System;
using UnityEngine;

public class Unparent : MonoBehaviour
{
	private void Start()
	{
		base.gameObject.transform.parent = null;
	}
}

