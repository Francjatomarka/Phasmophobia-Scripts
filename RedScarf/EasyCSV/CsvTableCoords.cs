using System;

namespace RedScarf.EasyCSV
{
	// Token: 0x02000415 RID: 1045
	public struct CsvTableCoords
	{
		// Token: 0x06002156 RID: 8534 RVA: 0x000A1250 File Offset: 0x0009F450
		public CsvTableCoords(int row, int column)
		{
			this.row = row;
			this.column = column;
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x000A1250 File Offset: 0x0009F450
		public void Set(int row, int column)
		{
			this.row = row;
			this.column = column;
		}

		// Token: 0x04001E6D RID: 7789
		public int row;

		// Token: 0x04001E6E RID: 7790
		public int column;
	}
}
