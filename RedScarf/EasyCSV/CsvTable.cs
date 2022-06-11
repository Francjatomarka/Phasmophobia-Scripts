using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RedScarf.EasyCSV
{
	// Token: 0x02000414 RID: 1044
	public sealed class CsvTable
	{
		// Token: 0x06002139 RID: 8505 RVA: 0x000A087C File Offset: 0x0009EA7C
		internal static void Init(char separator)
		{
			CsvTable.s_Separator = separator;
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x000A0884 File Offset: 0x0009EA84
		public CsvTable(string name, string data, bool resolveColumnName, bool firstColumnIsID)
		{
			this.m_Name = name;
			this.m_FirstColumnIsID = firstColumnIsID;
			this.m_ResolveColumnName = resolveColumnName;
			this.m_RawDataList = new List<List<string>>(999);
			this.stringBuilder = new StringBuilder(49950);
			this.columnNameDict = new Dictionary<string, int>(50);
			this.rowIdDict = new Dictionary<string, int>(999);
			this.Append(data);
			this.ResolveColumnName();
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x000A08F8 File Offset: 0x0009EAF8
		private void ResolveRowID(int row)
		{
			if (row < 0 || row >= this.m_RawDataList.Count)
			{
				return;
			}
			string text = this.Read(row, 0);
			if (!string.IsNullOrEmpty(text))
			{
				this.rowIdDict.Remove(text);
				this.rowIdDict.Add(text, row);
			}
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x000A0944 File Offset: 0x0009EB44
		private void ResolveColumnName()
		{
			if (!this.m_ResolveColumnName)
			{
				return;
			}
			if (this.m_RawDataList.Count > 0)
			{
				List<string> list = this.m_RawDataList[0];
				for (int i = 0; i < list.Count; i++)
				{
					string text = list[i];
					if (!string.IsNullOrEmpty(text))
					{
						string.Intern(text);
						if (this.columnNameDict.ContainsKey(text))
						{
							this.columnNameDict.Remove(text);
						}
						this.columnNameDict.Add(text, i);
					}
				}
			}
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x000A09CB File Offset: 0x0009EBCB
		public int GetRowByID(string id)
		{
			if (this.m_FirstColumnIsID && this.rowIdDict.ContainsKey(id))
			{
				return this.rowIdDict[id];
			}
			return -1;
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x000A09F1 File Offset: 0x0009EBF1
		private int GetColumnByColumnName(string columnName)
		{
			if (!string.IsNullOrEmpty(columnName) && this.columnNameDict.ContainsKey(columnName))
			{
				return this.columnNameDict[columnName];
			}
			return -1;
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x000A0A18 File Offset: 0x0009EC18
		public string Read(int row, int column)
		{
			if (row < 0 || row >= this.m_RawDataList.Count || column < 0)
			{
				return string.Empty;
			}
			List<string> list = this.m_RawDataList[row];
			if (column >= list.Count)
			{
				return string.Empty;
			}
			return list[column];
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x000A0A64 File Offset: 0x0009EC64
		public string Read(int row, string columnName)
		{
			if (!string.IsNullOrEmpty(columnName) && row >= 0 && row < this.m_RawDataList.Count)
			{
				int columnByColumnName = this.GetColumnByColumnName(columnName);
				if (columnByColumnName >= 0 && this.m_RawDataList[row].Count > columnByColumnName)
				{
					return this.m_RawDataList[row][columnByColumnName];
				}
			}
			return string.Empty;
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000A0AC3 File Offset: 0x0009ECC3
		public string Read(string id, string columnName)
		{
			return this.Read(this.GetRowByID(id), columnName);
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x000A0AD3 File Offset: 0x0009ECD3
		public string Read(string id, int column)
		{
			return this.Read(this.GetRowByID(id), column);
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x000A0AE4 File Offset: 0x0009ECE4
		public void Write(int row, int column, string value)
		{
			if (row < 0 || column < 0)
			{
				return;
			}
			if (!string.IsNullOrEmpty(value))
			{
				int num = row - this.m_RawDataList.Count + 1;
				if (num > 0)
				{
					for (int i = 0; i < num; i++)
					{
						List<string> item = new List<string>(50);
						this.m_RawDataList.Add(item);
					}
				}
				List<string> list = this.m_RawDataList[row];
				int num2 = column - list.Count + 1;
				if (num2 > 0)
				{
					for (int j = 0; j < num2; j++)
					{
						list.Add(string.Empty);
					}
				}
				list[column] = value;
			}
			else if (row < this.m_RawDataList.Count && column < this.m_RawDataList[row].Count)
			{
				this.m_RawDataList[row][column] = value;
			}
			this.ResolveRowID(row);
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x000A0BB8 File Offset: 0x0009EDB8
		public void Write(int row, string columnName, string value)
		{
			int columnByColumnName = this.GetColumnByColumnName(columnName);
			if (columnByColumnName >= 0)
			{
				this.Write(row, columnByColumnName, value);
			}
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000A0BDA File Offset: 0x0009EDDA
		public void Write(string id, string columnName, string value)
		{
			this.Write(this.GetRowByID(id), columnName, value);
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x000A0BEB File Offset: 0x0009EDEB
		public void Write(string id, int column, string value)
		{
			this.Write(this.GetRowByID(id), column, value);
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x000A0BFC File Offset: 0x0009EDFC
		public void Append(string data)
		{
			this.InsertData(this.m_RawDataList.Count, data);
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x000A0C10 File Offset: 0x0009EE10
		public void InsertData(int row, string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return;
			}
			row = Mathf.Clamp(row, 0, this.m_RawDataList.Count);
			List<List<string>> dataList = this.GetDataList(data);
			if (dataList != null)
			{
				this.m_RawDataList.InsertRange(row, dataList);
				int num = row + dataList.Count;
				for (int i = row; i < num; i++)
				{
					this.ResolveRowID(i);
				}
			}
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x000A0C70 File Offset: 0x0009EE70
		private List<List<string>> GetDataList(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return null;
			}
			this.stringBuilder.Length = 0;
			if (this.m_LineBreak == CsvTable.LineBreak.None)
			{
				if (data.IndexOf("\r\n") >= 0)
				{
					this.m_LineBreak = CsvTable.LineBreak.CRLF;
				}
				else if (data.IndexOf('\r') >= 0)
				{
					this.m_LineBreak = CsvTable.LineBreak.CR;
				}
				else if (data.IndexOf('\n') >= 0)
				{
					this.m_LineBreak = CsvTable.LineBreak.LF;
				}
			}
			List<List<string>> list = new List<List<string>>();
			int length = data.Length;
			int num = 0;
			List<string> list2 = new List<string>();
			int i = 0;
			while (i < length)
			{
				char c = data[i];
				if (c != '"')
				{
					goto IL_9C;
				}
				num++;
				if (num != 1 && num % 2 != 0)
				{
					goto IL_9C;
				}
				IL_140:
				i++;
				continue;
				IL_9C:
				if (num % 2 == 0)
				{
					bool flag = false;
					if (c == CsvTable.s_Separator)
					{
						list2.Add(this.stringBuilder.ToString());
						this.stringBuilder.Length = 0;
						num = 0;
						goto IL_140;
					}
					if (c == '\r')
					{
						flag = true;
						if (i + 1 < length && data[i + 1] == '\n')
						{
							i++;
						}
					}
					else if (c == '\n')
					{
						flag = true;
					}
					if (flag)
					{
						list2.Add(this.stringBuilder.ToString());
						list.Add(list2);
						list2 = new List<string>();
						this.stringBuilder.Length = 0;
						num = 0;
						goto IL_140;
					}
				}
				this.stringBuilder.Append(c);
				goto IL_140;
			}
			string text = this.stringBuilder.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				list2.Add(text);
				list.Add(list2);
			}
			this.stringBuilder.Length = 0;
			return list;
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x000A0E00 File Offset: 0x0009F000
		public void RemoveValue(int row, int column)
		{
			if (row < 0 || column < 0)
			{
				return;
			}
			if (row >= this.m_RawDataList.Count)
			{
				return;
			}
			List<string> list = this.m_RawDataList[row];
			if (column >= list.Count)
			{
				return;
			}
			list.RemoveAt(column);
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x000A0E44 File Offset: 0x0009F044
		public void RemoveValue(int row, string columnName)
		{
			int columnByColumnName = this.GetColumnByColumnName(columnName);
			this.RemoveValue(row, columnByColumnName);
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000A0E61 File Offset: 0x0009F061
		public void RemoveRow(int row)
		{
			if (row < 0 || row >= this.m_RawDataList.Count)
			{
				return;
			}
			this.m_RawDataList.RemoveAt(row);
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x000A0E84 File Offset: 0x0009F084
		public void RemoveColumn(int column)
		{
			if (column < 0)
			{
				return;
			}
			foreach (List<string> list in this.m_RawDataList)
			{
				if (column < list.Count)
				{
					list.RemoveAt(column);
				}
			}
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x000A0EE8 File Offset: 0x0009F0E8
		public void RemoveColumn(string columnName)
		{
			int columnByColumnName = this.GetColumnByColumnName(columnName);
			if (columnByColumnName >= 0)
			{
				this.RemoveColumn(columnByColumnName);
				this.columnNameDict.Remove(columnName);
			}
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x000A0F18 File Offset: 0x0009F118
		public CsvTableCoords FindValue(string value, CsvTableCoords start)
		{
			CsvTableCoords result = new CsvTableCoords(-1, -1);
			int num = Mathf.Clamp(start.row, 0, this.m_RawDataList.Count);
			List<string> list = this.m_RawDataList[num];
			for (int i = start.column; i < list.Count; i++)
			{
				if (list[i] == value)
				{
					result.Set(num, i);
					return result;
				}
			}
			num++;
			for (int j = num; j < this.m_RawDataList.Count; j++)
			{
				List<string> list2 = this.m_RawDataList[j];
				for (int k = 0; k < list2.Count; k++)
				{
					if (list2[k] == value)
					{
						result.Set(j, k);
						return result;
					}
				}
			}
			return result;
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x000A0FE5 File Offset: 0x0009F1E5
		public CsvTableCoords FindValue(string value)
		{
			return this.FindValue(value, new CsvTableCoords(0, 0));
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06002151 RID: 8529 RVA: 0x000A0FF5 File Offset: 0x0009F1F5
		public int RowCount
		{
			get
			{
				return this.m_RawDataList.Count;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06002152 RID: 8530 RVA: 0x000A1002 File Offset: 0x0009F202
		public string Name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06002153 RID: 8531 RVA: 0x000A100A File Offset: 0x0009F20A
		public List<List<string>> RawDataList
		{
			get
			{
				return this.m_RawDataList;
			}
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x000A1014 File Offset: 0x0009F214
		public string GetData(CsvTable.LineBreak lineBreak = CsvTable.LineBreak.None, char separator = ',')
		{
			this.stringBuilder.Length = 0;
			if (lineBreak == CsvTable.LineBreak.None)
			{
				lineBreak = this.m_LineBreak;
			}
			if (lineBreak == CsvTable.LineBreak.None)
			{
				lineBreak = CsvTable.LineBreak.CRLF;
			}
			int num = 0;
			foreach (List<string> list in this.m_RawDataList)
			{
				num = Mathf.Max(num, list.Count);
			}
			foreach (List<string> list2 in this.m_RawDataList)
			{
				int count = list2.Count;
				for (int i = 0; i < num; i++)
				{
					bool flag = false;
					int length = this.stringBuilder.Length;
					if (i < count)
					{
						string text = list2[i];
						int length2 = text.Length;
						for (int j = 0; j < length2; j++)
						{
							char c = text[j];
							if (c == separator)
							{
								flag = true;
							}
							else if (c == '\n' || c == '\r')
							{
								flag = true;
							}
							else if (c == '"')
							{
								this.stringBuilder.Append('"');
								flag = true;
							}
							this.stringBuilder.Append(c);
						}
					}
					if (flag)
					{
						this.stringBuilder.Insert(length, '"');
						this.stringBuilder.Append('"');
					}
					this.stringBuilder.Append(separator);
				}
				this.stringBuilder.Remove(this.stringBuilder.Length - 1, 1);
				this.stringBuilder.Append(CsvTable.lineBreakDict[lineBreak]);
			}
			return this.stringBuilder.ToString();
		}

		// Token: 0x04001E5C RID: 7772
		public const char DEFAULT_SEPARATOR = ',';

		// Token: 0x04001E5D RID: 7773
		private const char ESCAPE_CHAR = '"';

		// Token: 0x04001E5E RID: 7774
		private const char CR = '\r';

		// Token: 0x04001E5F RID: 7775
		private const char LF = '\n';

		// Token: 0x04001E60 RID: 7776
		private const string CRLF = "\r\n";

		// Token: 0x04001E61 RID: 7777
		private const int DEFAULT_COLUMN_COUNT = 50;

		// Token: 0x04001E62 RID: 7778
		private const int DEFAULT_ROW_COUNT = 999;

		// Token: 0x04001E63 RID: 7779
		private static char s_Separator = ',';

		// Token: 0x04001E64 RID: 7780
		private static readonly Dictionary<CsvTable.LineBreak, string> lineBreakDict = new Dictionary<CsvTable.LineBreak, string>
		{
			{
				CsvTable.LineBreak.CRLF,
				"\r\n"
			},
			{
				CsvTable.LineBreak.LF,
				'\n'.ToString()
			},
			{
				CsvTable.LineBreak.CR,
				'\r'.ToString()
			}
		};

		// Token: 0x04001E65 RID: 7781
		private CsvTable.LineBreak m_LineBreak;

		// Token: 0x04001E66 RID: 7782
		private string m_Name;

		// Token: 0x04001E67 RID: 7783
		private List<List<string>> m_RawDataList;

		// Token: 0x04001E68 RID: 7784
		private StringBuilder stringBuilder;

		// Token: 0x04001E69 RID: 7785
		private Dictionary<string, int> columnNameDict;

		// Token: 0x04001E6A RID: 7786
		private Dictionary<string, int> rowIdDict;

		// Token: 0x04001E6B RID: 7787
		private bool m_FirstColumnIsID;

		// Token: 0x04001E6C RID: 7788
		private bool m_ResolveColumnName;

		// Token: 0x02000724 RID: 1828
		public enum LineBreak
		{
			// Token: 0x04002740 RID: 10048
			None,
			// Token: 0x04002741 RID: 10049
			CRLF,
			// Token: 0x04002742 RID: 10050
			LF,
			// Token: 0x04002743 RID: 10051
			CR
		}
	}
}
