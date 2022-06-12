using System;
using System.IO;
using UnityEngine;

public class PhotosManager : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.photoRends.Length; i++)
		{
			if (File.Exists(string.Concat(new object[]
			{
				Application.dataPath,
				"/../SavedScreen",
				i,
				".png"
			})))
			{
				Texture2D texture2D = new Texture2D(384, 216, TextureFormat.RGB24, false);
				texture2D.LoadImage(File.ReadAllBytes(string.Concat(new object[]
				{
					Application.dataPath,
					"/../SavedScreen",
					i,
					".png"
				})));
				this.photoRends[i].material.mainTexture = texture2D;
				this.photoRends[i].gameObject.SetActive(true);
			}
		}
	}

	[SerializeField]
	private Renderer[] photoRends;
}

