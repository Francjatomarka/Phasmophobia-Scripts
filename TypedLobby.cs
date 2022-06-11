using System;

// Token: 0x02000055 RID: 85
public class TypedLobby
{
	// Token: 0x1700003D RID: 61
	// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000B1A9 File Offset: 0x000093A9
	public bool IsDefault
	{
		get
		{
			return this.Type == LobbyType.Default && string.IsNullOrEmpty(this.Name);
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000B1C0 File Offset: 0x000093C0
	public TypedLobby()
	{
		this.Name = string.Empty;
		this.Type = LobbyType.Default;
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000B1DA File Offset: 0x000093DA
	public TypedLobby(string name, LobbyType type)
	{
		this.Name = name;
		this.Type = type;
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000B1F0 File Offset: 0x000093F0
	public override string ToString()
	{
		return string.Format("lobby '{0}'[{1}]", this.Name, this.Type);
	}

	// Token: 0x040002BD RID: 701
	public string Name;

	// Token: 0x040002BE RID: 702
	public LobbyType Type;

	// Token: 0x040002BF RID: 703
	public static readonly TypedLobby Default = new TypedLobby();
}
