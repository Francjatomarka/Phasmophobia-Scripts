using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class LevelController : MonoBehaviour
{
	private void Awake()
	{
		LevelController.instance = this;
		this.view = base.GetComponent<PhotonView>();
		this.currentPlayerRoom = this.outsideRoom;
		this.currentGhostRoom = this.rooms[0];
		this.SetGhostName();
		this.gameController.OnAllPlayersConnected.AddListener(new UnityAction(this.SpawnMainDoorKey));
	}

	private void Start()
	{
		if (PhotonNetwork.InRoom)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				this.view.RPC("SyncFuseBoxLocation", RpcTarget.AllBuffered, new object[]
				{
					UnityEngine.Random.Range(0, this.fuseboxSpawnLocations.Length)
				});
				return;
			}
		}
		else
		{
			this.SyncFuseBoxLocation(UnityEngine.Random.Range(0, this.fuseboxSpawnLocations.Length));
		}
	}

	[PunRPC]
	private void SyncFuseBoxLocation(int index)
	{
		this.fuseBox.parentObject.position = this.fuseboxSpawnLocations[index].position;
		this.fuseBox.parentObject.rotation = this.fuseboxSpawnLocations[index].rotation;
		this.fuseBox.SetupAudioGroup();
	}

	private void SpawnMainDoorKey()
	{
		this.mainDoorKeySpawner.gameObject.SetActive(true);
		this.mainDoorKeySpawner.Spawn();
	}

	private void SetGhostName()
	{
		string str = "";
		if (this.nameType == LevelController.familyNameType.family)
		{
			str = this.possibleLastNames[UnityEngine.Random.Range(0, this.possibleLastNames.Length)];
		}
		for (int i = 0; i < this.adultMalesInLevelCount; i++)
		{
			LevelController.Person person = new LevelController.Person
			{
				isMale = true,
				firstName = this.possibleMaleFirstNames[UnityEngine.Random.Range(0, this.possibleMaleFirstNames.Length)],
				lastName = ((this.nameType == LevelController.familyNameType.random) ? (" " + this.possibleLastNames[UnityEngine.Random.Range(0, this.possibleLastNames.Length)]) : (" " + str)),
				age = UnityEngine.Random.Range(21, 80)
			};
			LevelController.Person item = person;
			this.peopleInHouse.Add(item);
			this.adultsInHouse.Add(item);
		}
		for (int j = 0; j < this.adultFemalesInLevelCount; j++)
		{
			LevelController.Person person = new LevelController.Person
			{
				isMale = false,
				firstName = this.possibleFemaleFirstNames[UnityEngine.Random.Range(0, this.possibleFemaleFirstNames.Length)],
				lastName = ((this.nameType == LevelController.familyNameType.random) ? (" " + this.possibleLastNames[UnityEngine.Random.Range(0, this.possibleLastNames.Length)]) : (" " + str)),
				age = UnityEngine.Random.Range(21, 80)
			};
			LevelController.Person item2 = person;
			this.peopleInHouse.Add(item2);
			this.adultsInHouse.Add(item2);
		}
		for (int k = 0; k < this.kidMalesInLevelCount; k++)
		{
			LevelController.Person person = new LevelController.Person
			{
				isMale = true,
				firstName = this.possibleMaleFirstNames[UnityEngine.Random.Range(0, this.possibleMaleFirstNames.Length)],
				lastName = ((this.nameType == LevelController.familyNameType.random) ? (" " + this.possibleLastNames[UnityEngine.Random.Range(0, this.possibleLastNames.Length)]) : (" " + str)),
				age = UnityEngine.Random.Range(5, 21)
			};
			LevelController.Person item3 = person;
			this.peopleInHouse.Add(item3);
		}
		for (int l = 0; l < this.kidFemalesInLevelCount; l++)
		{
			LevelController.Person person = new LevelController.Person
			{
				isMale = true,
				firstName = this.possibleFemaleFirstNames[UnityEngine.Random.Range(0, this.possibleFemaleFirstNames.Length)],
				lastName = ((this.nameType == LevelController.familyNameType.random) ? (" " + this.possibleLastNames[UnityEngine.Random.Range(0, this.possibleLastNames.Length)]) : (" " + str)),
				age = UnityEngine.Random.Range(5, 21)
			};
			LevelController.Person item4 = person;
			this.peopleInHouse.Add(item4);
		}
		for (int m = 0; m < this.childrenMalesInLevelCount; m++)
		{
			LevelController.Person person = new LevelController.Person
			{
				isMale = true,
				firstName = this.possibleMaleFirstNames[UnityEngine.Random.Range(0, this.possibleMaleFirstNames.Length)],
				lastName = ((this.nameType == LevelController.familyNameType.random) ? (" " + this.possibleLastNames[UnityEngine.Random.Range(0, this.possibleLastNames.Length)]) : (" " + str)),
				age = UnityEngine.Random.Range(1, 5)
			};
			LevelController.Person item5 = person;
			this.peopleInHouse.Add(item5);
		}
		for (int n = 0; n < this.childrenFemalesInLevelCount; n++)
		{
			LevelController.Person person = new LevelController.Person
			{
				isMale = false,
				firstName = this.possibleFemaleFirstNames[UnityEngine.Random.Range(0, this.possibleFemaleFirstNames.Length)],
				lastName = ((this.nameType == LevelController.familyNameType.random) ? (" " + this.possibleLastNames[UnityEngine.Random.Range(0, this.possibleLastNames.Length)]) : (" " + str)),
				age = UnityEngine.Random.Range(1, 5)
			};
			LevelController.Person item6 = person;
			this.peopleInHouse.Add(item6);
		}
	}

	public static LevelController instance;

	[HideInInspector]
	public PhotonView view;

	[HideInInspector]
	public LevelRoom currentPlayerRoom;

	[HideInInspector]
	public LevelRoom currentGhostRoom;

	[HideInInspector]
	public GhostAI currentGhost;

	[HideInInspector]
	public List<LevelController.Person> peopleInHouse = new List<LevelController.Person>();

	[HideInInspector]
	public List<LevelController.Person> adultsInHouse = new List<LevelController.Person>();

	public List<GameObject> doors = new List<GameObject>();

	public LevelRoom[] rooms = new LevelRoom[0];

	public Transform[] fuseboxSpawnLocations;

	public Transform[] MannequinTeleportSpots;

	public LevelRoom outsideRoom;

	public FuseBox fuseBox;

	public JournalController journalController;

	[HideInInspector]
	public List<JournalController> journals = new List<JournalController>();

	public GameController gameController;

	public ItemSpawner itemSpawner;

	public float nightVisionPower = 1000f;

	public Door[] exitDoors = new Door[0];

	public Radio[] radiosInLevel;

	[HideInInspector]
	public List<Crucifix> crucifix = new List<Crucifix>();

	[HideInInspector]
	public List<Torch> torches = new List<Torch>();

	public string streetName = "41 Tanglewood Street \n New Britain, CT 06051";

	public LevelController.levelType type;

	public LevelController.familyNameType nameType;

	[HideInInspector]
	public string[] possibleMaleFirstNames = new string[]
	{
		"James",
		"John",
		"Robert",
		"Michael",
		"William",
		"David",
		"Richard",
		"Charles",
		"Joseph",
		"Thomas",
		"Christopher",
		"Daniel",
		"Paul",
		"Mark",
		"Donald",
		"George",
		"Kenneth",
		"Steven",
		"Edward",
		"Brian",
		"Ronald",
		"Anthony",
		"Kevin",
		"Jason",
		"Gary",
		"Larry",
		"Eric",
		"Raymond",
		"Jerry",
		"Harold",
		"Pater",
		"Justin",
		"Billy",
		"Carlos",
		"Russell"
	};

	[HideInInspector]
	public string[] possibleFemaleFirstNames = new string[]
	{
		"Mary",
		"Patricia",
		"Linda",
		"Barbara",
		"Elizabeth",
		"Jennifer",
		"Maria",
		"Susan",
		"Margaret",
		"Dorothy",
		"Lisa",
		"Nancy",
		"Karen",
		"Betty",
		"Helen",
		"Sandra",
		"Donna",
		"Carol",
		"Ruth",
		"Ann",
		"Julie",
		"Doris",
		"Gloria",
		"Judy",
		"Lori",
		"Jane",
		"Ellen",
		"April",
		"Megan",
		"Robin",
		"Holly",
		"Carla",
		"Ella",
		"Stacey",
		"Marcia",
		"Nellie",
		"Shelly"
	};

	[HideInInspector]
	public string[] possibleLastNames = new string[]
	{
		"Smith",
		"Johnson",
		"Williams",
		"Jones",
		"Brown",
		"Davis",
		"Miller",
		"Wilson",
		"Moore",
		"Taylor",
		"Anderson",
		"Thomas",
		"Jackson",
		"White",
		"Harris",
		"Martin",
		"Thompson",
		"Garcia",
		"Martinez",
		"Robinson",
		"Clark",
		"Lewis",
		"Walker",
		"Young",
		"Hill",
		"Hall",
		"Wright",
		"Roberts",
		"Carter",
		"Baker",
		"Wilson",
		"Anderson"
	};

	public int adultMalesInLevelCount;

	public int adultFemalesInLevelCount;

	public int kidMalesInLevelCount;

	public int kidFemalesInLevelCount;

	public int childrenMalesInLevelCount;

	public int childrenFemalesInLevelCount;

	public Car car;

	public KeySpawner mainDoorKeySpawner;

	public struct Person
	{
		public string firstName;

		public int age;

		[HideInInspector]
		public string lastName;

		public bool isMale;
	}

	public enum levelType
	{
		small,
		medium,
		large
	}

	public enum familyNameType
	{
		family,
		random
	}
}

