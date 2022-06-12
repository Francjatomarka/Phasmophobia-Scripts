using System;

public class TypedLobby
{
	// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000B1A9 File Offset: 0x000093A9
	public bool IsDefault
	{
		get
		{
			return this.Type == LobbyType.Default && string.IsNullOrEmpty(this.Name);
		}
	}

	public TypedLobby()
	{
		this.Name = string.Empty;
		this.Type = LobbyType.Default;
	}

	public TypedLobby(string name, LobbyType type)
	{
		this.Name = name;
		this.Type = type;
	}

	public override string ToString()
	{
		return string.Format("lobby '{0}'[{1}]", this.Name, this.Type);
	}

	public string Name;

	public LobbyType Type;

	public static readonly TypedLobby Default = new TypedLobby();
}

