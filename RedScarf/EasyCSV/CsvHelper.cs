using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RedScarf.EasyCSV
{
	// Token: 0x02000413 RID: 1043
	public static class CsvHelper
	{
		// Token: 0x06002133 RID: 8499 RVA: 0x000A0726 File Offset: 0x0009E926
		public static void Init(char separator = ',')
		{
			CsvTable.Init(separator);
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x000A0730 File Offset: 0x0009E930
		public static CsvTable Create(string csvName, string data = "", bool resolveColumnName = true, bool firstColumnIsID = true)
		{
			CsvTable csvTable = new CsvTable(csvName, data, resolveColumnName, firstColumnIsID);
			CsvHelper.tableDict.Remove(csvName);
			CsvHelper.tableDict.Add(csvName, csvTable);
			return csvTable;
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000A0760 File Offset: 0x0009E960
		public static CsvTable Get(string csvName)
		{
			if (CsvHelper.tableDict.ContainsKey(csvName))
			{
				return CsvHelper.tableDict[csvName];
			}
			return null;
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000A077C File Offset: 0x0009E97C
		public static T PaddingData<T>(string csvName, string id) where T : new()
		{
			T result = Activator.CreateInstance<T>();
			CsvTable csvTable = CsvHelper.Get(csvName);
			if (csvTable == null)
			{
				return result;
			}
			int rowByID = csvTable.GetRowByID(id);
			return CsvHelper.PaddingData<T>(csvName, rowByID);
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000A07AC File Offset: 0x0009E9AC
		public static T PaddingData<T>(string csvName, int row) where T : new()
		{
			T t = Activator.CreateInstance<T>();
			object obj = t;
			CsvTable csvTable = CsvHelper.Get(csvName);
			if (csvTable == null)
			{
				return t;
			}
			if (row < 0 || row > csvTable.RowCount)
			{
				return t;
			}
			foreach (FieldInfo fieldInfo in typeof(T).GetFields())
			{
				string value = csvTable.Read(row, fieldInfo.Name);
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						object value2 = Convert.ChangeType(value, fieldInfo.FieldType);
						fieldInfo.SetValue(obj, value2);
					}
					catch (Exception ex)
					{
						Debug.LogErrorFormat("Csv padding data error! {0}", new object[]
						{
							ex
						});
					}
				}
			}
			return (T)((object)obj);
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000A0870 File Offset: 0x0009EA70
		public static void Clear()
		{
			CsvHelper.tableDict.Clear();
		}

		// Token: 0x04001E5B RID: 7771
		private static Dictionary<string, CsvTable> tableDict = new Dictionary<string, CsvTable>();
	}
}
