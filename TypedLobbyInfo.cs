using System;

// Token: 0x02000056 RID: 86
public class TypedLobbyInfo : TypedLobby
{
	// Token: 0x060001A7 RID: 423 RVA: 0x0000B21C File Offset: 0x0000941C
	public override string ToString()
	{
		return string.Format("TypedLobbyInfo '{0}'[{1}] rooms: {2} players: {3}", new object[]
		{
			this.Name,
			this.Type,
			this.RoomCount,
			this.PlayerCount
		});
	}

	// Token: 0x040002C0 RID: 704
	public int PlayerCount;

	// Token: 0x040002C1 RID: 705
	public int RoomCount;
}
