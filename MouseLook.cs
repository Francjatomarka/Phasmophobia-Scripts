using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.FirstPerson
{
	// Token: 0x0200007D RID: 125
	[Serializable]
	public class MouseLook
	{
		// Token: 0x06000392 RID: 914 RVA: 0x00020544 File Offset: 0x0001E744
		public void Init(Transform character, Transform camera)
		{
			this.m_CharacterTargetRot = character.localRotation;
			this.m_CameraTargetRot = camera.localRotation;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00020560 File Offset: 0x0001E760
		public void LookRotation(Transform character, Transform camera)
		{
			float y = this.horizontalLook * this.XSensitivity;
			float num = this.verticalLook * this.YSensitivity;
			this.m_CharacterTargetRot *= Quaternion.Euler(0f, y, 0f);
			this.m_CameraTargetRot *= Quaternion.Euler(-num, 0f, 0f);
			if (this.clampVerticalRotation)
			{
				this.m_CameraTargetRot = this.ClampRotationAroundXAxis(this.m_CameraTargetRot);
			}
			if (this.smooth)
			{
				character.localRotation = Quaternion.Slerp(character.localRotation, this.m_CharacterTargetRot, this.smoothTime * Time.deltaTime);
				camera.localRotation = Quaternion.Slerp(camera.localRotation, this.m_CameraTargetRot, this.smoothTime * Time.deltaTime);
				return;
			}
			character.localRotation = this.m_CharacterTargetRot;
			camera.localRotation = this.m_CameraTargetRot;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00020650 File Offset: 0x0001E850
		private Quaternion ClampRotationAroundXAxis(Quaternion q)
		{
			q.x /= q.w;
			q.y /= q.w;
			q.z /= q.w;
			q.w = 1f;
			float num = 114.59156f * Mathf.Atan(q.x);
			num = Mathf.Clamp(num, this.MinimumX, this.MaximumX);
			q.x = Mathf.Tan(0.008726646f * num);
			return q;
		}

		// Token: 0x040004D3 RID: 1235
		public float XSensitivity = 2f;

		// Token: 0x040004D4 RID: 1236
		public float YSensitivity = 2f;

		// Token: 0x040004D5 RID: 1237
		public bool clampVerticalRotation = true;

		// Token: 0x040004D6 RID: 1238
		public float MinimumX = -90f;

		// Token: 0x040004D7 RID: 1239
		public float MaximumX = 90f;

		// Token: 0x040004D8 RID: 1240
		public bool smooth;

		// Token: 0x040004D9 RID: 1241
		public float smoothTime = 5f;

		// Token: 0x040004DA RID: 1242
		public bool lockCursor = true;

		// Token: 0x040004DB RID: 1243
		private Quaternion m_CharacterTargetRot;

		// Token: 0x040004DC RID: 1244
		private Quaternion m_CameraTargetRot;

		// Token: 0x040004DD RID: 1245
		private bool m_cursorIsLocked = true;

		// Token: 0x040004DE RID: 1246
		[HideInInspector]
		public float horizontalLook;

		// Token: 0x040004DF RID: 1247
		[HideInInspector]
		public float verticalLook;
	}
}
