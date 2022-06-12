using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using Photon.Pun;

public class EVPRecorder : MonoBehaviour
{
	private void Awake()
	{
		this.noise.gameObject.SetActive(false);
	}

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

	private bool IsCorrectGhostType()
	{
		return LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Spirit || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Poltergeist || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Jinn || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Wraith || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Mare || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Demon || LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Oni;
	}

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

	[PunRPC]
	private void PlayDifficultySound(int index)
	{
		this.soundSource.clip = this.difficultyAnswerClips[index];
		this.soundSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.soundSource.Play();
		DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.SpiritBoxResponse, 1);
	}

	[PunRPC]
	private void PlayLocationSound(int index)
	{
		this.soundSource.clip = this.locationAnswerClips[index];
		this.soundSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.soundSource.Play();
		DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.SpiritBoxResponse, 1);
	}

	[PunRPC]
	private void PlayAboutSound(int index)
	{
		this.soundSource.clip = this.aboutAnswerClips[index];
		this.soundSource.outputAudioMixerGroup = SoundController.instance.GetFloorAudioSnapshot(base.transform.position.y);
		this.soundSource.Play();
		DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.SpiritBoxResponse, 1);
	}

	public void PlayTrailerSound()
	{
		this.soundSource.clip = this.trailerSoundClip;
		this.soundSource.Play();
	}

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

	private void OnPlayerSpawned()
	{
		if (!XRDevice.isPresent)
		{
			base.Invoke("PlayInputDelay", 5f);
		}
	}

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

	public void PushToTalkStarted()
	{
		this.voipKeyIsPressed = true;
	}

	public void PushToTalkStopped()
	{
		this.voipKeyIsPressed = false;
	}

	[SerializeField]
	private Text fmText;

	public AudioSource loopSource;

	public AudioSource soundSource;

	[SerializeField]
	private AudioClip[] difficultyAnswerClips = new AudioClip[0];

	[SerializeField]
	private AudioClip[] locationAnswerClips = new AudioClip[0];

	[SerializeField]
	private AudioClip[] aboutAnswerClips = new AudioClip[0];

	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private Noise noise;

	[SerializeField]
	private PhotonObjectInteract photonInteract;

	private bool isOn;

	private float scanTimer = 0.1f;

	private float currentFMChannel = 100f;

	private bool isAddingFM;

	private List<Question> questions = new List<Question>();

	private List<string> yesQuestions = new List<string>();

	private List<string> noQuestions = new List<string>();

	private bool voipKeyIsPressed;

	private float responseTimer = 15f;

	[SerializeField]
	private AudioClip trailerSoundClip;

	private bool hasAnswered = true;
}

