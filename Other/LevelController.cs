using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Token: 0x020000D6 RID: 214
[RequireComponent(typeof(PhotonView))]
public class LevelController : MonoBehaviour
{
	// Token: 0x0600060C RID: 1548 RVA: 0x00023900 File Offset: 0x00021B00
	private void Awake()
	{
		LevelController.instance = this;
		this.view = base.GetComponent<PhotonView>();
		this.currentPlayerRoom = this.outsideRoom;
		this.currentGhostRoom = this.rooms[0];
		this.SetGhostName();
		this.gameController.OnAllPlayersConnected.AddListener(new UnityAction(this.SpawnMainDoorKey));
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x0002395C File Offset: 0x00021B5C
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

	// Token: 0x0600060E RID: 1550 RVA: 0x000239BC File Offset: 0x00021BBC
	[PunRPC]
	private void SyncFuseBoxLocation(int index)
	{
		this.fuseBox.parentObject.position = this.fuseboxSpawnLocations[index].position;
		this.fuseBox.parentObject.rotation = this.fuseboxSpawnLocations[index].rotation;
		this.fuseBox.SetupAudioGroup();
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x00023A0E File Offset: 0x00021C0E
	private void SpawnMainDoorKey()
	{
		this.mainDoorKeySpawner.gameObject.SetActive(true);
		this.mainDoorKeySpawner.Spawn();
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x00023A2C File Offset: 0x00021C2C
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

	// Token: 0x040005C2 RID: 1474
	public static LevelController instance;

	// Token: 0x040005C3 RID: 1475
	[HideInInspector]
	public PhotonView view;

	// Token: 0x040005C4 RID: 1476
	[HideInInspector]
	public LevelRoom currentPlayerRoom;

	// Token: 0x040005C5 RID: 1477
	[HideInInspector]
	public LevelRoom currentGhostRoom;

	// Token: 0x040005C6 RID: 1478
	[HideInInspector]
	public GhostAI currentGhost;

	// Token: 0x040005C7 RID: 1479
	[HideInInspector]
	public List<LevelController.Person> peopleInHouse = new List<LevelController.Person>();

	// Token: 0x040005C8 RID: 1480
	[HideInInspector]
	public List<LevelController.Person> adultsInHouse = new List<LevelController.Person>();

	// Token: 0x040005C9 RID: 1481
	public List<GameObject> doors = new List<GameObject>();

	// Token: 0x040005CA RID: 1482
	public LevelRoom[] rooms = new LevelRoom[0];

	// Token: 0x040005CB RID: 1483
	public Transform[] fuseboxSpawnLocations;

	// Token: 0x040005CC RID: 1484
	public Transform[] MannequinTeleportSpots;

	// Token: 0x040005CD RID: 1485
	public LevelRoom outsideRoom;

	// Token: 0x040005CE RID: 1486
	public FuseBox fuseBox;

	// Token: 0x040005CF RID: 1487
	public JournalController journalController;

	// Token: 0x040005D0 RID: 1488
	[HideInInspector]
	public List<JournalController> journals = new List<JournalController>();

	// Token: 0x040005D1 RID: 1489
	public GameController gameController;

	// Token: 0x040005D2 RID: 1490
	public ItemSpawner itemSpawner;

	// Token: 0x040005D3 RID: 1491
	public float nightVisionPower = 1000f;

	// Token: 0x040005D4 RID: 1492
	public Door[] exitDoors = new Door[0];

	// Token: 0x040005D5 RID: 1493
	public Radio[] radiosInLevel;

	// Token: 0x040005D6 RID: 1494
	[HideInInspector]
	public List<Crucifix> crucifix = new List<Crucifix>();

	// Token: 0x040005D7 RID: 1495
	[HideInInspector]
	public List<Torch> torches = new List<Torch>();

	// Token: 0x040005D8 RID: 1496
	public string streetName = "41 Tanglewood Street \n New Britain, CT 06051";

	// Token: 0x040005D9 RID: 1497
	public LevelController.levelType type;

	// Token: 0x040005DA RID: 1498
	public LevelController.familyNameType nameType;

	// Token: 0x040005DB RID: 1499
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

	// Token: 0x040005DC RID: 1500
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

	// Token: 0x040005DD RID: 1501
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

	// Token: 0x040005DE RID: 1502
	public int adultMalesInLevelCount;

	// Token: 0x040005DF RID: 1503
	public int adultFemalesInLevelCount;

	// Token: 0x040005E0 RID: 1504
	public int kidMalesInLevelCount;

	// Token: 0x040005E1 RID: 1505
	public int kidFemalesInLevelCount;

	// Token: 0x040005E2 RID: 1506
	public int childrenMalesInLevelCount;

	// Token: 0x040005E3 RID: 1507
	public int childrenFemalesInLevelCount;

	// Token: 0x040005E4 RID: 1508
	public Car car;

	// Token: 0x040005E5 RID: 1509
	public KeySpawner mainDoorKeySpawner;

	// Token: 0x020004B1 RID: 1201
	public struct Person
	{
		// Token: 0x04002246 RID: 8774
		public string firstName;

		// Token: 0x04002247 RID: 8775
		public int age;

		// Token: 0x04002248 RID: 8776
		[HideInInspector]
		public string lastName;

		// Token: 0x04002249 RID: 8777
		public bool isMale;
	}

	// Token: 0x020004B2 RID: 1202
	public enum levelType
	{
		// Token: 0x0400224B RID: 8779
		small,
		// Token: 0x0400224C RID: 8780
		medium,
		// Token: 0x0400224D RID: 8781
		large
	}

	// Token: 0x020004B3 RID: 1203
	public enum familyNameType
	{
		// Token: 0x0400224F RID: 8783
		family,
		// Token: 0x04002250 RID: 8784
		random
	}
}
