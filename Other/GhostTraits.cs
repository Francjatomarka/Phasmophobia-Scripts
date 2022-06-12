using System;
using UnityEngine;

[SerializeField]
public struct GhostTraits
{
	public GhostTraits.Type ghostType;

	public LevelController.Person victim;

	public int ghostAge;

	public bool isMale;

	public string ghostName;

	public bool isShy;

	public int deathLength;

	public int favouriteRoomID;

	public enum Type
	{
		none,
		Spirit,
		Wraith,
		Phantom,
		Poltergeist,
		Banshee,
		Jinn,
		Mare,
		Revenant,
		Shade,
		Demon,
		Yurei,
		Oni
	}
}

