using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;

// Token: 0x02000110 RID: 272
public class EVPRecorder : MonoBehaviour
{
	// Token: 0x0600077C RID: 1916 RVA: 0x0002C6B2 File Offset: 0x0002A8B2
	private void Awake()
	{
		this.noise.gameObject.SetActive(false);
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x0002C6C8 File Offset: 0x0002A8C8
	private void Start()
	{
		if (MainManager.instance)
		{
			//base.gameObject.SetActive(false);
			//return;
		}
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
		if(SpeechRecognitionController.instance != null)
        {
			SpeechRecognitionController.instance.AddEVPRecorder(this);
		}
		if (!XRDevice.isPresent && GameController.instance != null)
		{
			if (GameController.instance.myPlayer == null)
			{
				GameController.instance.OnLocalPlayerSpawned.AddListener(new UnityAction(this.OnPlayerSpawned));
				return;
			}
			this.OnPlayerSpawned();
		}
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0002C754 File Offset: 0x0002A954
	private void Update()
	{
		if (this.isOn)
		{
			this.scanTimer -= Time.deltaTime;
			if (this.scanTimer < 0f)
			{
				if (this.currentFMChannel >= 110f)
				{
					this.isAddingFM = false;
				}
				else if (this.currentFMChannel <= 85f)
				{
					this.isAddingFM = true;
				}
				if (this.isAddingFM)
				{
					this.currentFMChannel += 0.1f;
				}
				else
				{
					this.currentFMChannel -= 0.1f;
				}
				if (this.hasAnswered)
				{
					this.fmText.text = this.currentFMChannel.ToString("0.0") + "fm";
				}
				this.scanTimer = 0.1f;
			}
			if(GameController.instance != null)
			{
				if (Vector3.Distance(base.transform.position, GameController.instance.myPlayer.player.headObject.transform.position) < 5f)
				{
					this.responseTimer += Time.deltaTime;
					if (this.responseTimer > 15f)
					{
						this.ResponseCheck();
						this.responseTimer = 0f;
					}
				}
			}
		}
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x0002C89E File Offset: 0x0002AA9E
	private IEnumerator FailCheck()
	{
		yield return new WaitForSeconds(1f);
		if (!this.hasAnswered)
		{
			this.fmText.text = LocalisationSystem.GetLocalisedValue("SpiritBox_Error");
		}
		yield return new WaitForSeconds(1f);
		this.hasAnswered = true;
		yield break;
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x0002C8B0 File Offset: 0x0002AAB0
	private void ResponseCheck()
	{
		if (this.soundSource.isPlaying)
		{
			return;
		}
		if (LevelController.instance.currentPlayerRoom == null || LevelController.instance.currentGhostRoom == null)
		{
			return;
		}
		if (LevelController.instance.currentPlayerRoom.floorType != LevelController.instance.currentGhostRoom.floorType)
		{
			return;
		}
		if (LevelController.instance.currentPlayerRoom != LevelController.instance.currentGhostRoom)
		{
			return;
		}
		if (LevelController.instance.currentGhostRoom.playersInRoom.Count > 1 && LevelController.instance.currentGhost.ghostInfo.ghostTraits.isShy)
		{
			return;
		}
		if (!this.IsCorrectGhostType())
		{
			return;
		}
		if (LevelController.instance.fuseBox.isOn)
		{
			for (int i = 0; i < LevelController.instance.currentPlayerRoom.lightSwitches.Count; i++)
			{
				if (LevelController.instance.currentPlayerRoom.lightSwitches[i].isOn)
				{
					return;
				}
			}
		}
		if (UnityEngine.Random.Range(0, 2) == 1)
		{
			this.LocationAnswer();
			return;
		}
		this.AgeAnswer();
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x0002C9FD File Offset: 0x0002ABFD
	private void Use()
	{
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("NetworkUse", RpcTarget.All, new object[]
			{
				7104444
			});
			return;
		}
		NetworkUse(7104444);
		
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x0002CA28 File Offset: 0x0002AC28
	public void TurnOff()
	{
		if (this.isOn)
		{
			if (PhotonNetwork.InRoom)
			{
				this.view.RPC("NetworkUse", RpcTarget.All, new object[]
				{
				7104444
				});
				return;
			}
			NetworkUse(7104444);
		}
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0002CA5C File Offset: 0x0002AC5C
	[PunRPC]
	private void NetworkUse(int actorID)
	{
		if(actorID == 7104444)
        {
			this.isOn = !this.isOn;
			if (this.isOn)
			{
				this.loopSource.Play();
				this.fmText.text = this.currentFMChannel.ToString("0.0");
				this.noise.gameObject.SetActive(true);
				return;
			}
			this.noise.gameObject.SetActive(false);
			this.loopSource.Stop();
			this.fmText.text = "";
			return;
		}
		this.isOn = !this.isOn;
		if (this.isOn)
		{
			this.loopSource.outputAudioMixerGroup = SoundController.instance.GetPlayersAudioGroup(actorID);
			this.soundSource.outputAudioMixerGroup = SoundController.instance.GetPlayersAudioGroup(actorID);
			this.loopSource.Play();
			this.fmText.text = this.currentFMChannel.ToString("0.0");
			this.noise.gameObject.SetActive(true);
			return;
		}
		this.noise.gameObject.SetActive(false);
		this.loopSource.Stop();
		this.fmText.text = "";
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x0002CB10 File Offset: 0x0002AD10
	private bool IsCorrectGhostType()
	{
		return LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Spirit || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Poltergeist || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Jinn || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Wraith || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Mare || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Demon || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Oni;
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x0002CBEC File Offset: 0x0002ADEC
	public void SetupKeywords()
	{
		Question item = new Question
		{
			questions = new List<string>
			{
				LocalisationSystem.GetLocalisedValue("Q_What do you want"),
				LocalisationSystem.GetLocalisedValue("Q_Why are you here"),
				LocalisationSystem.GetLocalisedValue("Q_Do you want to hurt us"),
				LocalisationSystem.GetLocalisedValue("Q_Are you angry"),
				LocalisationSystem.GetLocalisedValue("Q_Do you want us here"),
				LocalisationSystem.GetLocalisedValue("Q_Shall we leave"),
				LocalisationSystem.GetLocalisedValue("Q_Should we leave"),
				LocalisationSystem.GetLocalisedValue("Q_Do you want us to leave"),
				LocalisationSystem.GetLocalisedValue("Q_What should we do"),
				LocalisationSystem.GetLocalisedValue("Q_Can we help"),
				LocalisationSystem.GetLocalisedValue("Q_Are you friendly"),
				LocalisationSystem.GetLocalisedValue("Q_What are you")
			},
			questionType = Question.QuestionType.difficulty
		};
		this.questions.Add(item);
		item = new Question
		{
			questions = new List<string>
			{
				LocalisationSystem.GetLocalisedValue("Q_Where are you"),
				LocalisationSystem.GetLocalisedValue("Q_Are you close"),
				LocalisationSystem.GetLocalisedValue("Q_Can you show yourself"),
				LocalisationSystem.GetLocalisedValue("Q_Give us a sign"),
				LocalisationSystem.GetLocalisedValue("Q_Let us know you are here"),
				LocalisationSystem.GetLocalisedValue("Q_Show yourself"),
				LocalisationSystem.GetLocalisedValue("Q_Can you talk"),
				LocalisationSystem.GetLocalisedValue("Q_Speak to us"),
				LocalisationSystem.GetLocalisedValue("Q_Are you here"),
				LocalisationSystem.GetLocalisedValue("Q_Are you with us"),
				LocalisationSystem.GetLocalisedValue("Q_Anybody with us"),
				LocalisationSystem.GetLocalisedValue("Q_Is anyone here"),
				LocalisationSystem.GetLocalisedValue("Q_Anybody in the room"),
				LocalisationSystem.GetLocalisedValue("Q_Anybody here"),
				LocalisationSystem.GetLocalisedValue("Q_Is there a spirit here"),
				LocalisationSystem.GetLocalisedValue("Q_Is there a Ghost here"),
				LocalisationSystem.GetLocalisedValue("Q_What is your location")
			},
			questionType = Question.QuestionType.location
		};
		this.questions.Add(item);
		item = new Question
		{
			questions = new List<string>
			{
				LocalisationSystem.GetLocalisedValue("Q_Are you a girl"),
				LocalisationSystem.GetLocalisedValue("Q_Are you a boy"),
				LocalisationSystem.GetLocalisedValue("Q_Are you male"),
				LocalisationSystem.GetLocalisedValue("Q_Are you female"),
				LocalisationSystem.GetLocalisedValue("Q_Who are you"),
				LocalisationSystem.GetLocalisedValue("Q_What are you"),
				LocalisationSystem.GetLocalisedValue("Q_Who is this"),
				LocalisationSystem.GetLocalisedValue("Q_Who are we talking to"),
				LocalisationSystem.GetLocalisedValue("Q_Who am I talking to"),
				LocalisationSystem.GetLocalisedValue("Q_Hello"),
				LocalisationSystem.GetLocalisedValue("Q_What is your name"),
				LocalisationSystem.GetLocalisedValue("Q_Can you give me your name"),
				LocalisationSystem.GetLocalisedValue("Q_What is your gender"),
				LocalisationSystem.GetLocalisedValue("Q_What gender"),
				LocalisationSystem.GetLocalisedValue("Q_Are you male or female"),
				LocalisationSystem.GetLocalisedValue("Q_Are you a man"),
				LocalisationSystem.GetLocalisedValue("Q_Are you a woman")
			},
			questionType = Question.QuestionType.gender
		};
		this.questions.Add(item);
		item = new Question
		{
			questions = new List<string>
			{
				LocalisationSystem.GetLocalisedValue("Q_How old are you"),
				LocalisationSystem.GetLocalisedValue("Q_How young are you"),
				LocalisationSystem.GetLocalisedValue("Q_What is your age"),
				LocalisationSystem.GetLocalisedValue("Q_When were you born"),
				LocalisationSystem.GetLocalisedValue("Q_Are you a child"),
				LocalisationSystem.GetLocalisedValue("Q_Are you old"),
				LocalisationSystem.GetLocalisedValue("Q_Are you young")
			},
			questionType = Question.QuestionType.age
		};
		this.questions.Add(item);
		for (int i = 0; i < this.questions.Count; i++)
		{
			for (int j = 0; j < this.questions[i].questions.Count; j++)
			{
				SpeechRecognitionController.instance.AddKeyword(this.questions[i].questions[j]);
			}
		}
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x0002D04C File Offset: 0x0002B24C
	public void OnPhraseRecognized(string args)
	{
		this.hasAnswered = false;
		base.StartCoroutine(this.FailCheck());
		if (!this.isOn)
		{
			return;
		}
		if (this.soundSource.isPlaying)
		{
			return;
		}
		if (!XRDevice.isPresent && PlayerPrefs.GetInt("localPushToTalkValue") == 0 && PhotonNetwork.CurrentRoom.PlayerCount > 1 && !this.voipKeyIsPressed)
		{
			return;
		}
		if (LevelController.instance.currentPlayerRoom == null || LevelController.instance.currentGhostRoom == null)
		{
			return;
		}
		if (LevelController.instance.currentPlayerRoom.floorType != LevelController.instance.currentGhostRoom.floorType)
		{
			return;
		}
		if (LevelController.instance.currentPlayerRoom != LevelController.instance.currentGhostRoom && Vector3.Distance(base.transform.position, LevelController.instance.currentGhost.transform.position) > 3f)
		{
			return;
		}
		if (LevelController.instance.currentGhostRoom.playersInRoom.Count > 1 && LevelController.instance.currentGhost.ghostInfo.ghostTraits.isShy)
		{
			return;
		}
		if (!this.IsCorrectGhostType())
		{
			return;
		}
		if (LevelController.instance.fuseBox.isOn)
		{
			for (int i = 0; i < LevelController.instance.currentPlayerRoom.lightSwitches.Count; i++)
			{
				if (LevelController.instance.currentPlayerRoom.lightSwitches[i].isOn)
				{
					return;
				}
			}
		}
		this.hasAnswered = true;
		for (int j = 0; j < this.questions.Count; j++)
		{
			for (int k = 0; k < this.questions[j].questions.Count; k++)
			{
				if (args == this.questions[j].questions[k])
				{
					if (this.questions[j].questionType == Question.QuestionType.difficulty)
					{
						return;
					}
					if (this.questions[j].questionType == Question.QuestionType.age)
					{
						this.AgeAnswer();
						return;
					}
					if (this.questions[j].questionType == Question.QuestionType.location)
					{
						this.LocationAnswer();
						return;
					}
				}
			}
		}
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x0002D274 File Offset: 0x0002B474
	[PunRPC]
	private void PlayDifficultySound(int index)
	{
		this.soundSource.clip = this.difficultyAnswerClips[index];
		this.soundSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.soundSource.Play();
		DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.SpiritBoxResponse, 1);
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x0002D2D4 File Offset: 0x0002B4D4
	[PunRPC]
	private void PlayLocationSound(int index)
	{
		this.soundSource.clip = this.locationAnswerClips[index];
		this.soundSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.soundSource.Play();
		DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.SpiritBoxResponse, 1);
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x0002D334 File Offset: 0x0002B534
	[PunRPC]
	private void PlayAboutSound(int index)
	{
		this.soundSource.clip = this.aboutAnswerClips[index];
		this.soundSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.soundSource.Play();
		DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.SpiritBoxResponse, 1);
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x0002D391 File Offset: 0x0002B591
	public void PlayTrailerSound()
	{
		this.soundSource.clip = this.trailerSoundClip;
		this.soundSource.Play();
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x0002D3B0 File Offset: 0x0002B5B0
	private void LocationAnswer()
	{
		if (!SpeechRecognitionController.instance.hasSaidGhostsName && !GameController.instance.isTutorial && UnityEngine.Random.Range(0, 3) < 2 && PlayerPrefs.GetInt("isYoutuberVersion") == 0)
		{
			return;
		}
		if (Vector3.Distance(GameController.instance.myPlayer.player.headObject.transform.position, LevelController.instance.currentGhost.transform.position) < 4f)
		{
			this.view.RPC("PlayLocationSound", RpcTarget.All, new object[]
			{
				UnityEngine.Random.Range(0, 3)
			});
			return;
		}
		this.view.RPC("PlayLocationSound", RpcTarget.All, new object[]
		{
			UnityEngine.Random.Range(3, 6)
		});
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x0002D478 File Offset: 0x0002B678
	private void AgeAnswer()
	{
		if (!SpeechRecognitionController.instance.hasSaidGhostsName && !GameController.instance.isTutorial && UnityEngine.Random.Range(0, 3) < 2 && PlayerPrefs.GetInt("isYoutuberVersion") == 0)
		{
			return;
		}
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			if (LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostAge < 5)
			{
				this.view.RPC("PlayAboutSound", RpcTarget.All, new object[]
				{
					1
				});
				return;
			}
			if (LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostAge < 21)
			{
				this.view.RPC("PlayAboutSound", RpcTarget.All, new object[]
				{
					5
				});
				return;
			}
			this.view.RPC("PlayAboutSound", RpcTarget.All, new object[]
			{
				0
			});
			return;
		}
		else
		{
			if (LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostAge >= 21)
			{
				this.view.RPC("PlayAboutSound", RpcTarget.All, new object[]
				{
					7
				});
				return;
			}
			if (UnityEngine.Random.Range(0, 1) == 1)
			{
				this.view.RPC("PlayAboutSound", RpcTarget.All, new object[]
				{
					9
				});
				return;
			}
			this.view.RPC("PlayAboutSound", RpcTarget.All, new object[]
			{
				2
			});
			return;
		}
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0002D5E8 File Offset: 0x0002B7E8
	private void GenderAnswer()
	{
		if (!SpeechRecognitionController.instance.hasSaidGhostsName && !GameController.instance.isTutorial && UnityEngine.Random.Range(0, 3) < 2 && PlayerPrefs.GetInt("isYoutuberVersion") == 0)
		{
			return;
		}
		if (LevelController.instance.currentGhost.ghostInfo.ghostTraits.isMale)
		{
			if (LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostAge < 21)
			{
				this.view.RPC("PlayAboutSound", RpcTarget.All, new object[]
				{
					8
				});
				return;
			}
			this.view.RPC("PlayAboutSound", RpcTarget.All, new object[]
			{
				3
			});
			return;
		}
		else
		{
			if (LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostAge < 21)
			{
				this.view.RPC("PlayAboutSound", RpcTarget.All, new object[]
				{
					4
				});
				return;
			}
			this.view.RPC("PlayAboutSound", RpcTarget.All, new object[]
			{
				6
			});
			return;
		}
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x0002D6FC File Offset: 0x0002B8FC
	private void OnPlayerSpawned()
	{
		if (!XRDevice.isPresent)
		{
			base.Invoke("PlayInputDelay", 5f);
		}
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x0002D718 File Offset: 0x0002B918
	private void PlayInputDelay()
	{
		if (PlayerPrefs.GetInt("localPushToTalkValue") == 0)
		{
			if(GameController.instance != null)
            {
				GameController.instance.myPlayer.player.playerInput.actions["LocalPushToTalk"].started += delegate (InputAction.CallbackContext _)
				{
					this.PushToTalkStarted();
				};
				GameController.instance.myPlayer.player.playerInput.actions["LocalPushToTalk"].canceled += delegate (InputAction.CallbackContext _)
				{
					this.PushToTalkStopped();
				};
			}
		}
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x0002D79C File Offset: 0x0002B99C
	private void OnDisable()
	{
		if (PlayerPrefs.GetInt("localPushToTalkValue") == 0 && !XRDevice.isPresent && GameController.instance && GameController.instance.myPlayer != null && GameController.instance.myPlayer.player.playerInput)
		{
			GameController.instance.myPlayer.player.playerInput.actions["LocalPushToTalk"].started -= delegate(InputAction.CallbackContext _)
			{
				this.PushToTalkStarted();
			};
			GameController.instance.myPlayer.player.playerInput.actions["LocalPushToTalk"].canceled -= delegate(InputAction.CallbackContext _)
			{
				this.PushToTalkStopped();
			};
		}
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x0002D863 File Offset: 0x0002BA63
	public void PushToTalkStarted()
	{
		this.voipKeyIsPressed = true;
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x0002D86C File Offset: 0x0002BA6C
	public void PushToTalkStopped()
	{
		this.voipKeyIsPressed = false;
	}

	// Token: 0x0400078C RID: 1932
	[SerializeField]
	private Text fmText;

	// Token: 0x0400078D RID: 1933
	public AudioSource loopSource;

	// Token: 0x0400078E RID: 1934
	public AudioSource soundSource;

	// Token: 0x0400078F RID: 1935
	[SerializeField]
	private AudioClip[] difficultyAnswerClips = new AudioClip[0];

	// Token: 0x04000790 RID: 1936
	[SerializeField]
	private AudioClip[] locationAnswerClips = new AudioClip[0];

	// Token: 0x04000791 RID: 1937
	[SerializeField]
	private AudioClip[] aboutAnswerClips = new AudioClip[0];

	// Token: 0x04000792 RID: 1938
	[SerializeField]
	private PhotonView view;

	// Token: 0x04000793 RID: 1939
	[SerializeField]
	private Noise noise;

	// Token: 0x04000794 RID: 1940
	[SerializeField]
	private PhotonObjectInteract photonInteract;

	// Token: 0x04000795 RID: 1941
	private bool isOn;

	// Token: 0x04000796 RID: 1942
	private float scanTimer = 0.1f;

	// Token: 0x04000797 RID: 1943
	private float currentFMChannel = 100f;

	// Token: 0x04000798 RID: 1944
	private bool isAddingFM;

	// Token: 0x04000799 RID: 1945
	private List<Question> questions = new List<Question>();

	// Token: 0x0400079A RID: 1946
	private List<string> yesQuestions = new List<string>();

	// Token: 0x0400079B RID: 1947
	private List<string> noQuestions = new List<string>();

	// Token: 0x0400079C RID: 1948
	private bool voipKeyIsPressed;

	// Token: 0x0400079D RID: 1949
	private float responseTimer = 15f;

	// Token: 0x0400079E RID: 1950
	[SerializeField]
	private AudioClip trailerSoundClip;

	// Token: 0x0400079F RID: 1951
	private bool hasAnswered = true;
}
