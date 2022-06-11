using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000021 RID: 33
public class BookCoverHelper : MonoBehaviour
{
	// Token: 0x060000DE RID: 222 RVA: 0x00006D50 File Offset: 0x00004F50
	private void Start()
	{
		this.rTr = base.GetComponent<RectTransform>();
		this.text = base.GetComponent<Text>();
		this.image = base.GetComponent<Image>();
		this.targetText = this.target.GetComponent<Text>();
		this.targetImage = this.target.GetComponent<Image>();
		this.target.position = this.rTr.position;
		this.target.localPosition = this.rTr.localPosition;
		this.target.rotation = this.rTr.rotation;
		this.target.localRotation = this.rTr.localRotation;
		this.target.localScale = this.rTr.localScale;
		this.target.anchoredPosition = this.rTr.anchoredPosition;
		this.target.anchoredPosition3D = this.rTr.anchoredPosition3D;
		this.target.anchorMax = this.rTr.anchorMax;
		this.target.anchorMin = this.rTr.anchorMin;
		this.target.offsetMax = this.rTr.offsetMax;
		this.target.offsetMin = this.rTr.offsetMin;
		this.target.pivot = this.rTr.pivot;
		this.target.sizeDelta = this.rTr.sizeDelta;
		if (this.text != null && this.targetText != null)
		{
			this.targetText.alignment = this.text.alignment;
			this.targetText.color = this.text.color;
			this.targetText.font = this.text.font;
			this.targetText.fontSize = this.text.fontSize;
			this.targetText.fontStyle = this.text.fontStyle;
			this.targetText.horizontalOverflow = this.text.horizontalOverflow;
			this.targetText.lineSpacing = this.text.lineSpacing;
			this.targetText.material = this.text.material;
			this.targetText.raycastTarget = this.text.raycastTarget;
			this.targetText.resizeTextForBestFit = this.text.resizeTextForBestFit;
			this.targetText.resizeTextMaxSize = this.text.resizeTextMaxSize;
			this.targetText.resizeTextMinSize = this.text.resizeTextMinSize;
			this.targetText.supportRichText = this.text.supportRichText;
			this.targetText.text = this.text.text;
			this.targetText.verticalOverflow = this.text.verticalOverflow;
		}
		if (this.image != null && this.targetImage != null)
		{
			this.targetImage.sprite = this.image.sprite;
			this.targetImage.color = this.image.color;
			this.targetImage.material = this.image.material;
			this.targetImage.raycastTarget = this.image.raycastTarget;
		}
	}

	// Token: 0x040000CC RID: 204
	public RectTransform target;

	// Token: 0x040000CD RID: 205
	private RectTransform rTr;

	// Token: 0x040000CE RID: 206
	private Text text;

	// Token: 0x040000CF RID: 207
	private Image image;

	// Token: 0x040000D0 RID: 208
	private Text targetText;

	// Token: 0x040000D1 RID: 209
	private Image targetImage;
}
