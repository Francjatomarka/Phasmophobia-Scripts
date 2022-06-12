using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class JournalController : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.keyAmount = 0;
		this.CreateGhosts();
		this.CreateEvidence();
	}

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

	public GhostTraits.Type GetGhostType()
	{
		GhostTraits.Type result = GhostTraits.Type.none;
		if (this.ghostTypeIndex != 0)
		{
			result = this.values[this.ghostTypeIndex].type;
		}
		return result;
	}

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

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private AudioSource openSource;

	[SerializeField]
	private AudioClip openClip;

	[SerializeField]
	private AudioClip closeClip;

	[HideInInspector]
	public bool isOpen;

	[SerializeField]
	private bool isVRJournal;

	[SerializeField]
	private GameObject[] pages;

	private int index;

	[SerializeField]
	private Image[] photos;

	[SerializeField]
	private Text[] photosNames;

	private int photosAmount;

	private int resWidth = 384;

	private int resHeight = 216;

	[HideInInspector]
	public PhotonView view;

	[Header("Keys Page")]
	private int keyAmount;

	[SerializeField]
	private Text Key1Text;

	[SerializeField]
	private Text Key2Text;

	[SerializeField]
	private Text Key3Text;

	[SerializeField]
	private Text Key4Text;

	[SerializeField]
	private Text Key5Text;

	[SerializeField]
	private Text Key6Text;

	[Header("Evidence Page")]
	private List<JournalController.Ghost> values = new List<JournalController.Ghost>();

	private List<JournalController.Ghost> ghosts = new List<JournalController.Ghost>();

	private List<string> evidenceNames = new List<string>();

	[SerializeField]
	private Text evidence1Text;

	private int evidence1Index;

	[SerializeField]
	private Text evidence2Text;

	private int evidence2Index;

	[SerializeField]
	private Text evidence3Text;

	private int evidence3Index;

	[SerializeField]
	private Text ghostTypeText;

	private int ghostTypeIndex;

	public enum evidenceType
	{
		None,
		EMF,
		SpiritBox,
		Fingerprints,
		GhostOrb,
		GhostWritingBook,
		Temperature
	}

	private struct Ghost
	{
		public GhostTraits.Type type;

		public string localisedName;

		public JournalController.evidenceType evidence1;

		public JournalController.evidenceType evidence2;

		public JournalController.evidenceType evidence3;
	}
}

