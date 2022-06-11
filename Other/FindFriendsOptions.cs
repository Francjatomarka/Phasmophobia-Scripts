using System;

// Token: 0x0200009B RID: 155
public class FindFriendsOptions
{
	// Token: 0x06000332 RID: 818 RVA: 0x00013878 File Offset: 0x00011A78
	internal int ToIntFlags()
	{
		int num = 0;
		if (this.CreatedOnGs)
		{
			num |= 1;
		}
		if (this.Visible)
		{
			num |= 2;
		}
		if (this.Open)
		{
			num |= 4;
		}
		return num;
	}

	// Token: 0x04000447 RID: 1095
	public bool CreatedOnGs;

	// Token: 0x04000448 RID: 1096
	public bool Visible;

	// Token: 0x04000449 RID: 1097
	public bool Open;
}
