using System;
using System.IO;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class PhotosManager : MonoBehaviour
{
	// Token: 0x06000921 RID: 2337 RVA: 0x00038230 File Offset: 0x00036430
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

	// Token: 0x0400093F RID: 2367
	[SerializeField]
	private Renderer[] photoRends;
}
