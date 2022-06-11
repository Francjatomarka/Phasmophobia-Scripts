using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x02000004 RID: 4
[Serializable]
public class FileBasedPrefsSaveFileModel
{
	// Token: 0x06000026 RID: 38 RVA: 0x00002358 File Offset: 0x00000558
	public object GetValueFromKey(string key, object defaultValue)
	{
		if (defaultValue is string)
		{
			for (int i = 0; i < this.StringData.Length; i++)
			{
				if (this.StringData[i].Key.Equals(key))
				{
					return this.StringData[i].Value;
				}
			}
		}
		if (defaultValue is int)
		{
			for (int j = 0; j < this.IntData.Length; j++)
			{
				if (this.IntData[j].Key.Equals(key))
				{
					return this.IntData[j].Value;
				}
			}
		}
		if (defaultValue is float)
		{
			for (int k = 0; k < this.FloatData.Length; k++)
			{
				if (this.FloatData[k].Key.Equals(key))
				{
					return this.FloatData[k].Value;
				}
			}
		}
		if (defaultValue is bool)
		{
			for (int l = 0; l < this.BoolData.Length; l++)
			{
				if (this.BoolData[l].Key.Equals(key))
				{
					return this.BoolData[l].Value;
				}
			}
		}
		return defaultValue;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x0000246D File Offset: 0x0000066D
	public void UpdateOrAddData(string key, object value)
	{
		if (this.HasKeyFromObject(key, value))
		{
			this.SetValueForExistingKey(key, value);
			return;
		}
		this.SetValueForNewKey(key, value);
	}

	// Token: 0x06000028 RID: 40 RVA: 0x0000248C File Offset: 0x0000068C
	private void SetValueForNewKey(string key, object value)
	{
		if (value is string)
		{
			List<FileBasedPrefsSaveFileModel.StringItem> list = this.StringData.ToList<FileBasedPrefsSaveFileModel.StringItem>();
			list.Add(new FileBasedPrefsSaveFileModel.StringItem(key, (string)value));
			this.StringData = list.ToArray();
		}
		if (value is int)
		{
			List<FileBasedPrefsSaveFileModel.IntItem> list2 = this.IntData.ToList<FileBasedPrefsSaveFileModel.IntItem>();
			list2.Add(new FileBasedPrefsSaveFileModel.IntItem(key, (int)value));
			this.IntData = list2.ToArray();
		}
		if (value is float)
		{
			List<FileBasedPrefsSaveFileModel.FloatItem> list3 = this.FloatData.ToList<FileBasedPrefsSaveFileModel.FloatItem>();
			list3.Add(new FileBasedPrefsSaveFileModel.FloatItem(key, (float)value));
			this.FloatData = list3.ToArray();
		}
		if (value is bool)
		{
			List<FileBasedPrefsSaveFileModel.BoolItem> list4 = this.BoolData.ToList<FileBasedPrefsSaveFileModel.BoolItem>();
			list4.Add(new FileBasedPrefsSaveFileModel.BoolItem(key, (bool)value));
			this.BoolData = list4.ToArray();
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002564 File Offset: 0x00000764
	private void SetValueForExistingKey(string key, object value)
	{
		if (value is string)
		{
			for (int i = 0; i < this.StringData.Length; i++)
			{
				if (this.StringData[i].Key.Equals(key))
				{
					this.StringData[i].Value = (string)value;
				}
			}
		}
		if (value is int)
		{
			for (int j = 0; j < this.IntData.Length; j++)
			{
				if (this.IntData[j].Key.Equals(key))
				{
					this.IntData[j].Value = (int)value;
				}
			}
		}
		if (value is float)
		{
			for (int k = 0; k < this.FloatData.Length; k++)
			{
				if (this.FloatData[k].Key.Equals(key))
				{
					this.FloatData[k].Value = (float)value;
				}
			}
		}
		if (value is bool)
		{
			for (int l = 0; l < this.BoolData.Length; l++)
			{
				if (this.BoolData[l].Key.Equals(key))
				{
					this.BoolData[l].Value = (bool)value;
				}
			}
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002680 File Offset: 0x00000880
	public bool HasKeyFromObject(string key, object value)
	{
		if (value is string)
		{
			for (int i = 0; i < this.StringData.Length; i++)
			{
				if (this.StringData[i].Key.Equals(key))
				{
					return true;
				}
			}
		}
		if (value is int)
		{
			for (int j = 0; j < this.IntData.Length; j++)
			{
				if (this.IntData[j].Key.Equals(key))
				{
					return true;
				}
			}
		}
		if (value is float)
		{
			for (int k = 0; k < this.FloatData.Length; k++)
			{
				if (this.FloatData[k].Key.Equals(key))
				{
					return true;
				}
			}
		}
		if (value is bool)
		{
			for (int l = 0; l < this.BoolData.Length; l++)
			{
				if (this.BoolData[l].Key.Equals(key))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002758 File Offset: 0x00000958
	public void DeleteKey(string key)
	{
		for (int i = 0; i < this.StringData.Length; i++)
		{
			if (this.StringData[i].Key.Equals(key))
			{
				List<FileBasedPrefsSaveFileModel.StringItem> list = this.StringData.ToList<FileBasedPrefsSaveFileModel.StringItem>();
				list.RemoveAt(i);
				this.StringData = list.ToArray();
			}
		}
		for (int j = 0; j < this.IntData.Length; j++)
		{
			if (this.IntData[j].Key.Equals(key))
			{
				List<FileBasedPrefsSaveFileModel.IntItem> list2 = this.IntData.ToList<FileBasedPrefsSaveFileModel.IntItem>();
				list2.RemoveAt(j);
				this.IntData = list2.ToArray();
			}
		}
		for (int k = 0; k < this.FloatData.Length; k++)
		{
			if (this.FloatData[k].Key.Equals(key))
			{
				List<FileBasedPrefsSaveFileModel.FloatItem> list3 = this.FloatData.ToList<FileBasedPrefsSaveFileModel.FloatItem>();
				list3.RemoveAt(k);
				this.FloatData = list3.ToArray();
			}
		}
		for (int l = 0; l < this.BoolData.Length; l++)
		{
			if (this.BoolData[l].Key.Equals(key))
			{
				List<FileBasedPrefsSaveFileModel.BoolItem> list4 = this.BoolData.ToList<FileBasedPrefsSaveFileModel.BoolItem>();
				list4.RemoveAt(l);
				this.BoolData = list4.ToArray();
			}
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002894 File Offset: 0x00000A94
	public void DeleteString(string key)
	{
		for (int i = 0; i < this.StringData.Length; i++)
		{
			if (this.StringData[i].Key.Equals(key))
			{
				List<FileBasedPrefsSaveFileModel.StringItem> list = this.StringData.ToList<FileBasedPrefsSaveFileModel.StringItem>();
				list.RemoveAt(i);
				this.StringData = list.ToArray();
			}
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x000028E8 File Offset: 0x00000AE8
	public void DeleteInt(string key)
	{
		for (int i = 0; i < this.IntData.Length; i++)
		{
			if (this.IntData[i].Key.Equals(key))
			{
				List<FileBasedPrefsSaveFileModel.IntItem> list = this.IntData.ToList<FileBasedPrefsSaveFileModel.IntItem>();
				list.RemoveAt(i);
				this.IntData = list.ToArray();
			}
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x0000293C File Offset: 0x00000B3C
	public void DeleteFloat(string key)
	{
		for (int i = 0; i < this.FloatData.Length; i++)
		{
			if (this.FloatData[i].Key.Equals(key))
			{
				List<FileBasedPrefsSaveFileModel.FloatItem> list = this.FloatData.ToList<FileBasedPrefsSaveFileModel.FloatItem>();
				list.RemoveAt(i);
				this.FloatData = list.ToArray();
			}
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002990 File Offset: 0x00000B90
	public void DeleteBool(string key)
	{
		for (int i = 0; i < this.BoolData.Length; i++)
		{
			if (this.BoolData[i].Key.Equals(key))
			{
				List<FileBasedPrefsSaveFileModel.BoolItem> list = this.BoolData.ToList<FileBasedPrefsSaveFileModel.BoolItem>();
				list.RemoveAt(i);
				this.BoolData = list.ToArray();
			}
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000029E4 File Offset: 0x00000BE4
	public bool HasKey(string key)
	{
		for (int i = 0; i < this.StringData.Length; i++)
		{
			if (this.StringData[i].Key.Equals(key))
			{
				return true;
			}
		}
		for (int j = 0; j < this.IntData.Length; j++)
		{
			if (this.IntData[j].Key.Equals(key))
			{
				return true;
			}
		}
		for (int k = 0; k < this.FloatData.Length; k++)
		{
			if (this.FloatData[k].Key.Equals(key))
			{
				return true;
			}
		}
		for (int l = 0; l < this.BoolData.Length; l++)
		{
			if (this.BoolData[l].Key.Equals(key))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000008 RID: 8
	public FileBasedPrefsSaveFileModel.StringItem[] StringData = new FileBasedPrefsSaveFileModel.StringItem[0];

	// Token: 0x04000009 RID: 9
	public FileBasedPrefsSaveFileModel.IntItem[] IntData = new FileBasedPrefsSaveFileModel.IntItem[0];

	// Token: 0x0400000A RID: 10
	public FileBasedPrefsSaveFileModel.FloatItem[] FloatData = new FileBasedPrefsSaveFileModel.FloatItem[0];

	// Token: 0x0400000B RID: 11
	public FileBasedPrefsSaveFileModel.BoolItem[] BoolData = new FileBasedPrefsSaveFileModel.BoolItem[0];

	// Token: 0x0200030A RID: 778
	[Serializable]
	public class StringItem
	{
		// Token: 0x0600149D RID: 5277 RVA: 0x00057E09 File Offset: 0x00056009
		public StringItem(string K, string V)
		{
			this.Key = K;
			this.Value = V;
		}

		// Token: 0x040014CE RID: 5326
		public string Key;

		// Token: 0x040014CF RID: 5327
		public string Value;
	}

	// Token: 0x0200030B RID: 779
	[Serializable]
	public class IntItem
	{
		// Token: 0x0600149E RID: 5278 RVA: 0x00057E1F File Offset: 0x0005601F
		public IntItem(string K, int V)
		{
			this.Key = K;
			this.Value = V;
		}

		// Token: 0x040014D0 RID: 5328
		public string Key;

		// Token: 0x040014D1 RID: 5329
		public int Value;
	}

	// Token: 0x0200030C RID: 780
	[Serializable]
	public class FloatItem
	{
		// Token: 0x0600149F RID: 5279 RVA: 0x00057E35 File Offset: 0x00056035
		public FloatItem(string K, float V)
		{
			this.Key = K;
			this.Value = V;
		}

		// Token: 0x040014D2 RID: 5330
		public string Key;

		// Token: 0x040014D3 RID: 5331
		public float Value;
	}

	// Token: 0x0200030D RID: 781
	[Serializable]
	public class BoolItem
	{
		// Token: 0x060014A0 RID: 5280 RVA: 0x00057E4B File Offset: 0x0005604B
		public BoolItem(string K, bool V)
		{
			this.Key = K;
			this.Value = V;
		}

		// Token: 0x040014D4 RID: 5332
		public string Key;

		// Token: 0x040014D5 RID: 5333
		public bool Value;
	}
}
