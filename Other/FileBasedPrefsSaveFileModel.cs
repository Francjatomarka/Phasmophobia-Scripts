using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class FileBasedPrefsSaveFileModel
{
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

	public void UpdateOrAddData(string key, object value)
	{
		if (this.HasKeyFromObject(key, value))
		{
			this.SetValueForExistingKey(key, value);
			return;
		}
		this.SetValueForNewKey(key, value);
	}

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

	public FileBasedPrefsSaveFileModel.StringItem[] StringData = new FileBasedPrefsSaveFileModel.StringItem[0];

	public FileBasedPrefsSaveFileModel.IntItem[] IntData = new FileBasedPrefsSaveFileModel.IntItem[0];

	public FileBasedPrefsSaveFileModel.FloatItem[] FloatData = new FileBasedPrefsSaveFileModel.FloatItem[0];

	public FileBasedPrefsSaveFileModel.BoolItem[] BoolData = new FileBasedPrefsSaveFileModel.BoolItem[0];

	[Serializable]
	public class StringItem
	{
		public StringItem(string K, string V)
		{
			this.Key = K;
			this.Value = V;
		}

		public string Key;

		public string Value;
	}

	[Serializable]
	public class IntItem
	{
		public IntItem(string K, int V)
		{
			this.Key = K;
			this.Value = V;
		}

		public string Key;

		public int Value;
	}

	[Serializable]
	public class FloatItem
	{
		public FloatItem(string K, float V)
		{
			this.Key = K;
			this.Value = V;
		}

		public string Key;

		public float Value;
	}

	[Serializable]
	public class BoolItem
	{
		public BoolItem(string K, bool V)
		{
			this.Key = K;
			this.Value = V;
		}

		public string Key;

		public bool Value;
	}
}

