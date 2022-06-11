using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x020000D5 RID: 213
[RequireComponent(typeof(PhotonView))]
public class JournalController : MonoBehaviour
{
	// Token: 0x060005F3 RID: 1523 RVA: 0x000227CF File Offset: 0x000209CF
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.keyAmount = 0;
		this.CreateGhosts();
		this.CreateEvidence();
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x000227F0 File Offset: 0x000209F0
	private void Start()
	{
		if (LevelController.instance)
		{
			LevelController.instance.journals.Add(this);
		}
		this.index = 0;
		for (int i = 0; i < this.pages.Length; i++)
		{
			this.pages[i].SetActive(false);
		}
		this.pages[this.index].SetActive(true);
		this.pages[this.index + 1].SetActive(true);
		this.SetGhostTypes();
		if (!this.isVRJournal && !XRDevice.isPresent)
		{
			if (GameController.instance.myPlayer == null)
			{
				GameController.instance.OnLocalPlayerSpawned.AddListener(new UnityAction(this.OnPlayerSpawned));
				return;
			}
			this.OnPlayerSpawned();
		}
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x000228AC File Offset: 0x00020AAC
	private void CreateEvidence()
	{
		this.evidenceNames.Add(LocalisationSystem.GetLocalisedValue("Journal_NoEvidence"));
		this.evidenceNames.Add(LocalisationSystem.GetLocalisedValue("Journal_EMFEvidence"));
		this.evidenceNames.Add(LocalisationSystem.GetLocalisedValue("Journal_SpiritBoxEvidence"));
		this.evidenceNames.Add(LocalisationSystem.GetLocalisedValue("Journal_FingerprintsEvidence"));
		this.evidenceNames.Add(LocalisationSystem.GetLocalisedValue("Journal_GhostOrbEvidence"));
		this.evidenceNames.Add(LocalisationSystem.GetLocalisedValue("Journal_GhostWritingEvidence"));
		this.evidenceNames.Add(LocalisationSystem.GetLocalisedValue("Journal_FreezingEvidence"));
		this.evidence1Text.text = this.evidenceNames[0];
		this.evidence2Text.text = this.evidenceNames[0];
		this.evidence3Text.text = this.evidenceNames[0];
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x00022994 File Offset: 0x00020B94
	private void CreateGhosts()
	{
		JournalController.Ghost item = new JournalController.Ghost
		{
			type = GhostTraits.Type.none,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_NoGhostType")
		};
		JournalController.Ghost item2 = new JournalController.Ghost
		{
			type = GhostTraits.Type.Spirit,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_SpiritTitle"),
			evidence1 = JournalController.evidenceType.SpiritBox,
			evidence2 = JournalController.evidenceType.Fingerprints,
			evidence3 = JournalController.evidenceType.GhostWritingBook
		};
		JournalController.Ghost item3 = new JournalController.Ghost
		{
			type = GhostTraits.Type.Wraith,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_WraithTitle"),
			evidence1 = JournalController.evidenceType.SpiritBox,
			evidence2 = JournalController.evidenceType.Fingerprints,
			evidence3 = JournalController.evidenceType.Temperature
		};
		JournalController.Ghost item4 = new JournalController.Ghost
		{
			type = GhostTraits.Type.Phantom,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_PhantomTitle"),
			evidence1 = JournalController.evidenceType.EMF,
			evidence2 = JournalController.evidenceType.GhostOrb,
			evidence3 = JournalController.evidenceType.Temperature
		};
		JournalController.Ghost item5 = new JournalController.Ghost
		{
			type = GhostTraits.Type.Poltergeist,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_PoltergeistTitle"),
			evidence1 = JournalController.evidenceType.SpiritBox,
			evidence2 = JournalController.evidenceType.Fingerprints,
			evidence3 = JournalController.evidenceType.GhostOrb
		};
		JournalController.Ghost item6 = new JournalController.Ghost
		{
			type = GhostTraits.Type.Banshee,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_BansheeTitle"),
			evidence1 = JournalController.evidenceType.EMF,
			evidence2 = JournalController.evidenceType.Fingerprints,
			evidence3 = JournalController.evidenceType.Temperature
		};
		JournalController.Ghost item7 = new JournalController.Ghost
		{
			type = GhostTraits.Type.Jinn,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_JinnTitle"),
			evidence1 = JournalController.evidenceType.EMF,
			evidence2 = JournalController.evidenceType.SpiritBox,
			evidence3 = JournalController.evidenceType.GhostOrb
		};
		JournalController.Ghost item8 = new JournalController.Ghost
		{
			type = GhostTraits.Type.Mare,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_MareTitle"),
			evidence1 = JournalController.evidenceType.Temperature,
			evidence2 = JournalController.evidenceType.SpiritBox,
			evidence3 = JournalController.evidenceType.GhostOrb
		};
		JournalController.Ghost item9 = new JournalController.Ghost
		{
			type = GhostTraits.Type.Revenant,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_RevenantTitle"),
			evidence1 = JournalController.evidenceType.EMF,
			evidence2 = JournalController.evidenceType.Fingerprints,
			evidence3 = JournalController.evidenceType.GhostWritingBook
		};
		JournalController.Ghost item10 = new JournalController.Ghost
		{
			type = GhostTraits.Type.Shade,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_ShadeTitle"),
			evidence1 = JournalController.evidenceType.EMF,
			evidence2 = JournalController.evidenceType.GhostOrb,
			evidence3 = JournalController.evidenceType.GhostWritingBook
		};
		JournalController.Ghost item11 = new JournalController.Ghost
		{
			type = GhostTraits.Type.Demon,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_DemonTitle"),
			evidence1 = JournalController.evidenceType.SpiritBox,
			evidence2 = JournalController.evidenceType.GhostWritingBook,
			evidence3 = JournalController.evidenceType.Temperature
		};
		JournalController.Ghost item12 = new JournalController.Ghost
		{
			type = GhostTraits.Type.Yurei,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_YureiTitle"),
			evidence1 = JournalController.evidenceType.GhostOrb,
			evidence2 = JournalController.evidenceType.GhostWritingBook,
			evidence3 = JournalController.evidenceType.Temperature
		};
		JournalController.Ghost item13 = new JournalController.Ghost
		{
			type = GhostTraits.Type.Oni,
			localisedName = LocalisationSystem.GetLocalisedValue("Journal_OniTitle"),
			evidence1 = JournalController.evidenceType.EMF,
			evidence2 = JournalController.evidenceType.SpiritBox,
			evidence3 = JournalController.evidenceType.GhostWritingBook
		};
		this.ghosts.Add(item);
		this.ghosts.Add(item2);
		this.ghosts.Add(item3);
		this.ghosts.Add(item4);
		this.ghosts.Add(item5);
		this.ghosts.Add(item6);
		this.ghosts.Add(item7);
		this.ghosts.Add(item8);
		this.ghosts.Add(item9);
		this.ghosts.Add(item10);
		this.ghosts.Add(item11);
		this.ghosts.Add(item12);
		this.ghosts.Add(item13);
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x00022D48 File Offset: 0x00020F48
	private void OnEnable()
	{
		if (GameController.instance)
		{
			if (base.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace && GameController.instance.myPlayer != null)
			{
				base.GetComponent<Canvas>().worldCamera = GameController.instance.myPlayer.player.cam;
				return;
			}
		}
		else if (base.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace)
		{
			base.GetComponent<Canvas>().worldCamera = FindObjectOfType<Player>().cam;
		}
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x00022DC0 File Offset: 0x00020FC0
	private void OpenCloseJournal()
	{
		if (GameController.instance == null)
		{
			return;
		}
		if (this.isVRJournal)
		{
			return;
		}
		if (!this.isOpen && GameController.instance.myPlayer.player.pcCanvas.isPaused)
		{
			return;
		}
		this.isOpen = !this.isOpen;
		this.openSource.clip = (this.isOpen ? this.openClip : this.closeClip);
		if (!this.openSource.isPlaying)
		{
			this.openSource.Play();
		}
		this.content.SetActive(this.isOpen);
		if (GameController.instance)
		{
			GameController.instance.myPlayer.player.firstPersonController.enabled = !this.isOpen;
			GameController.instance.myPlayer.player.charAnim.SetFloat("speed", 0f);
		}
		else if (MainManager.instance)
		{
			MainManager.instance.localPlayer.firstPersonController.enabled = !this.isOpen;
		}
		if (GameController.instance.myPlayer.player.playerInput.currentControlScheme == "Keyboard")
		{
			Cursor.visible = this.isOpen;
		}
		else
		{
			Cursor.visible = false;
		}
		if (this.isOpen)
		{
			if (GameController.instance.myPlayer.player.playerInput.currentControlScheme == "Keyboard")
			{
				Cursor.lockState = CursorLockMode.None;
				return;
			}
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x00022F50 File Offset: 0x00021150
	public void CloseJournal()
	{
		if (GameController.instance == null)
		{
			return;
		}
		if (this.isVRJournal)
		{
			return;
		}
		this.openSource.clip = this.closeClip;
		if (!this.openSource.isPlaying)
		{
			this.openSource.Play();
		}
		if (!this.isOpen)
		{
			return;
		}
		this.isOpen = false;
		this.content.SetActive(false);
		if (GameController.instance)
		{
			GameController.instance.myPlayer.player.firstPersonController.enabled = true;
		}
		else if (MainManager.instance)
		{
			MainManager.instance.localPlayer.firstPersonController.enabled = true;
		}
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x00023010 File Offset: 0x00021210
	public void NextPage()
	{
		if (!this.isVRJournal)
		{
			this.PageSync(2);
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("PageSync", RpcTarget.AllBuffered, new object[]
			{
				2
			});
			return;
		}
		this.PageSync(2);
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0002305C File Offset: 0x0002125C
	public void PreviousPage()
	{
		if (!this.isVRJournal)
		{
			this.PageSync(-2);
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("PageSync", RpcTarget.AllBuffered, new object[]
			{
				-2
			});
			return;
		}
		this.PageSync(-2);
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x000230AB File Offset: 0x000212AB
	public void EndPage()
	{
		if (!this.isVRJournal)
		{
			this.EndPageSync();
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			this.view.RPC("EndPageSync", RpcTarget.AllBuffered, Array.Empty<object>());
			return;
		}
		this.EndPageSync();
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x000230E0 File Offset: 0x000212E0
	[PunRPC]
	private void EndPageSync()
	{
		this.index = this.pages.Length - 2;
		for (int i = 0; i < this.pages.Length; i++)
		{
			this.pages[i].SetActive(false);
		}
		this.pages[this.index].SetActive(true);
		this.pages[this.index + 1].SetActive(true);
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x00023148 File Offset: 0x00021348
	[PunRPC]
	private void PageSync(int value)
	{
		this.index += value;
		for (int i = 0; i < this.pages.Length; i++)
		{
			this.pages[i].SetActive(false);
		}
		this.pages[this.index].SetActive(true);
		this.pages[this.index + 1].SetActive(true);
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x000231AC File Offset: 0x000213AC
	public void AddPhoto(byte[] sprBytes, string evidenceName, int evidenceAmount)
	{
		if (this.photosAmount < 10)
		{
			EvidenceController.instance.totalEvidenceFoundInPhotos += evidenceAmount;
			Texture2D texture2D = new Texture2D(this.resWidth, this.resHeight, TextureFormat.RGB24, false);
			texture2D.LoadRawTextureData(sprBytes);
			texture2D.Apply();
			this.photos[this.photosAmount].sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
			this.photosNames[this.photosAmount].text = evidenceName;
			this.photosAmount++;
			GameController.instance.myPlayer.player.evidenceAudioSource.Play();
		}
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x00023278 File Offset: 0x00021478
	public GhostTraits.Type GetGhostType()
	{
		GhostTraits.Type result = GhostTraits.Type.none;
		if (this.ghostTypeIndex != 0)
		{
			result = this.values[this.ghostTypeIndex].type;
		}
		return result;
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x000232A8 File Offset: 0x000214A8
	public void AddKey(string name)
	{
		this.keyAmount++;
		switch (this.keyAmount)
		{
		case 1:
			this.Key1Text.text = name;
			break;
		case 2:
			this.Key2Text.text = name;
			break;
		case 3:
			this.Key3Text.text = name;
			break;
		case 4:
			this.Key4Text.text = name;
			break;
		case 5:
			this.Key5Text.text = name;
			break;
		case 6:
			this.Key6Text.text = name;
			break;
		}
		GameController.instance.myPlayer.player.evidenceAudioSource.Play();
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x00023358 File Offset: 0x00021558
	public void SetGhostTypes()
	{
		this.values.Clear();
		this.values.Add(this.ghosts[0]);
		JournalController.evidenceType evidenceType = (JournalController.evidenceType)this.evidence1Index;
		JournalController.evidenceType evidenceType2 = (JournalController.evidenceType)this.evidence2Index;
		JournalController.evidenceType evidenceType3 = (JournalController.evidenceType)this.evidence3Index;
		for (int i = 0; i < this.ghosts.Count; i++)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (evidenceType != JournalController.evidenceType.None && (evidenceType == this.ghosts[i].evidence1 || evidenceType == this.ghosts[i].evidence2 || evidenceType == this.ghosts[i].evidence3))
			{
				flag = true;
			}
			if (evidenceType2 != JournalController.evidenceType.None && (evidenceType2 == this.ghosts[i].evidence1 || evidenceType2 == this.ghosts[i].evidence2 || evidenceType2 == this.ghosts[i].evidence3))
			{
				flag2 = true;
			}
			if (evidenceType3 != JournalController.evidenceType.None && (evidenceType3 == this.ghosts[i].evidence1 || evidenceType3 == this.ghosts[i].evidence2 || evidenceType3 == this.ghosts[i].evidence3))
			{
				flag3 = true;
			}
			if (flag && evidenceType2 == JournalController.evidenceType.None && evidenceType3 == JournalController.evidenceType.None)
			{
				this.values.Add(this.ghosts[i]);
			}
			else if (flag2 && evidenceType == JournalController.evidenceType.None && evidenceType3 == JournalController.evidenceType.None)
			{
				this.values.Add(this.ghosts[i]);
			}
			else if (flag3 && evidenceType == JournalController.evidenceType.None && evidenceType2 == JournalController.evidenceType.None)
			{
				this.values.Add(this.ghosts[i]);
			}
			else if (flag && flag2 && evidenceType3 == JournalController.evidenceType.None)
			{
				this.values.Add(this.ghosts[i]);
			}
			else if (flag && flag3 && evidenceType2 == JournalController.evidenceType.None)
			{
				this.values.Add(this.ghosts[i]);
			}
			else if (flag2 && flag3 && evidenceType == JournalController.evidenceType.None)
			{
				this.values.Add(this.ghosts[i]);
			}
			else if (flag && flag2 && flag3)
			{
				this.values.Add(this.ghosts[i]);
			}
			else if (evidenceType == JournalController.evidenceType.None && evidenceType2 == JournalController.evidenceType.None && evidenceType3 == JournalController.evidenceType.None)
			{
				this.values.Add(this.ghosts[i]);
			}
		}
		if (evidenceType == JournalController.evidenceType.None && evidenceType2 == JournalController.evidenceType.None && evidenceType3 == JournalController.evidenceType.None)
		{
			this.values.RemoveAt(0);
		}
		this.ghostTypeIndex = 0;
		this.ghostTypeText.text = this.values[0].localisedName;
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x000235E0 File Offset: 0x000217E0
	public void ChangeGhostTypeButton(int value)
	{
		this.ghostTypeIndex += value;
		if (this.ghostTypeIndex < 0)
		{
			this.ghostTypeIndex = this.values.Count - 1;
		}
		if (this.ghostTypeIndex == this.values.Count)
		{
			this.ghostTypeIndex = 0;
		}
		this.ghostTypeText.text = this.values[this.ghostTypeIndex].localisedName;
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x00023654 File Offset: 0x00021854
	public void ChangeEvidence1Button(int value)
	{
		this.evidence1Index += value;
		if (this.evidence1Index < 0)
		{
			this.evidence1Index = this.evidenceNames.Count - 1;
		}
		if (this.evidence1Index == this.evidenceNames.Count)
		{
			this.evidence1Index = 0;
		}
		this.evidence1Text.text = this.evidenceNames[this.evidence1Index];
		this.SetGhostTypes();
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x000236C8 File Offset: 0x000218C8
	public void ChangeEvidence2Button(int value)
	{
		this.evidence2Index += value;
		if (this.evidence2Index < 0)
		{
			this.evidence2Index = this.evidenceNames.Count - 1;
		}
		if (this.evidence2Index == this.evidenceNames.Count)
		{
			this.evidence2Index = 0;
		}
		this.evidence2Text.text = this.evidenceNames[this.evidence2Index];
		this.SetGhostTypes();
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0002373C File Offset: 0x0002193C
	public void ChangeEvidence3Button(int value)
	{
		this.evidence3Index += value;
		if (this.evidence3Index < 0)
		{
			this.evidence3Index = this.evidenceNames.Count - 1;
		}
		if (this.evidence3Index == this.evidenceNames.Count)
		{
			this.evidence3Index = 0;
		}
		this.evidence3Text.text = this.evidenceNames[this.evidence3Index];
		this.SetGhostTypes();
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x000237B0 File Offset: 0x000219B0
	private void OnPlayerSpawned()
	{
		if (!XRDevice.isPresent && !this.isVRJournal && GameController.instance && GameController.instance.myPlayer != null && GameController.instance.myPlayer.player.playerInput)
		{
			GameController.instance.myPlayer.player.playerInput.actions["Journal"].performed += delegate(InputAction.CallbackContext ctx)
			{
				this.OpenCloseJournal();
			};
		}
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x00023834 File Offset: 0x00021A34
	private void OnDisable()
	{
		if (!XRDevice.isPresent && !this.isVRJournal && GameController.instance && GameController.instance.myPlayer != null && GameController.instance.myPlayer.player.playerInput)
		{
			GameController.instance.myPlayer.player.playerInput.actions["Journal"].performed -= delegate(InputAction.CallbackContext ctx)
			{
				this.OpenCloseJournal();
			};
		}
	}

	// Token: 0x040005A2 RID: 1442
	[SerializeField]
	private GameObject content;

	// Token: 0x040005A3 RID: 1443
	[SerializeField]
	private AudioSource openSource;

	// Token: 0x040005A4 RID: 1444
	[SerializeField]
	private AudioClip openClip;

	// Token: 0x040005A5 RID: 1445
	[SerializeField]
	private AudioClip closeClip;

	// Token: 0x040005A6 RID: 1446
	[HideInInspector]
	public bool isOpen;

	// Token: 0x040005A7 RID: 1447
	[SerializeField]
	private bool isVRJournal;

	// Token: 0x040005A8 RID: 1448
	[SerializeField]
	private GameObject[] pages;

	// Token: 0x040005A9 RID: 1449
	private int index;

	// Token: 0x040005AA RID: 1450
	[SerializeField]
	private Image[] photos;

	// Token: 0x040005AB RID: 1451
	[SerializeField]
	private Text[] photosNames;

	// Token: 0x040005AC RID: 1452
	private int photosAmount;

	// Token: 0x040005AD RID: 1453
	private int resWidth = 384;

	// Token: 0x040005AE RID: 1454
	private int resHeight = 216;

	// Token: 0x040005AF RID: 1455
	[HideInInspector]
	public PhotonView view;

	// Token: 0x040005B0 RID: 1456
	[Header("Keys Page")]
	private int keyAmount;

	// Token: 0x040005B1 RID: 1457
	[SerializeField]
	private Text Key1Text;

	// Token: 0x040005B2 RID: 1458
	[SerializeField]
	private Text Key2Text;

	// Token: 0x040005B3 RID: 1459
	[SerializeField]
	private Text Key3Text;

	// Token: 0x040005B4 RID: 1460
	[SerializeField]
	private Text Key4Text;

	// Token: 0x040005B5 RID: 1461
	[SerializeField]
	private Text Key5Text;

	// Token: 0x040005B6 RID: 1462
	[SerializeField]
	private Text Key6Text;

	// Token: 0x040005B7 RID: 1463
	[Header("Evidence Page")]
	private List<JournalController.Ghost> values = new List<JournalController.Ghost>();

	// Token: 0x040005B8 RID: 1464
	private List<JournalController.Ghost> ghosts = new List<JournalController.Ghost>();

	// Token: 0x040005B9 RID: 1465
	private List<string> evidenceNames = new List<string>();

	// Token: 0x040005BA RID: 1466
	[SerializeField]
	private Text evidence1Text;

	// Token: 0x040005BB RID: 1467
	private int evidence1Index;

	// Token: 0x040005BC RID: 1468
	[SerializeField]
	private Text evidence2Text;

	// Token: 0x040005BD RID: 1469
	private int evidence2Index;

	// Token: 0x040005BE RID: 1470
	[SerializeField]
	private Text evidence3Text;

	// Token: 0x040005BF RID: 1471
	private int evidence3Index;

	// Token: 0x040005C0 RID: 1472
	[SerializeField]
	private Text ghostTypeText;

	// Token: 0x040005C1 RID: 1473
	private int ghostTypeIndex;

	// Token: 0x020004AF RID: 1199
	public enum evidenceType
	{
		// Token: 0x0400223A RID: 8762
		None,
		// Token: 0x0400223B RID: 8763
		EMF,
		// Token: 0x0400223C RID: 8764
		SpiritBox,
		// Token: 0x0400223D RID: 8765
		Fingerprints,
		// Token: 0x0400223E RID: 8766
		GhostOrb,
		// Token: 0x0400223F RID: 8767
		GhostWritingBook,
		// Token: 0x04002240 RID: 8768
		Temperature
	}

	// Token: 0x020004B0 RID: 1200
	private struct Ghost
	{
		// Token: 0x04002241 RID: 8769
		public GhostTraits.Type type;

		// Token: 0x04002242 RID: 8770
		public string localisedName;

		// Token: 0x04002243 RID: 8771
		public JournalController.evidenceType evidence1;

		// Token: 0x04002244 RID: 8772
		public JournalController.evidenceType evidence2;

		// Token: 0x04002245 RID: 8773
		public JournalController.evidenceType evidence3;
	}
}
