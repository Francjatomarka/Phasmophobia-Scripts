using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Photon.Pun;

public class OuijaBoard : MonoBehaviour
{
	private void Awake()
	{
		this.noise = base.GetComponentInChildren<Noise>();
		this.startPosition = base.transform.localPosition;
		this.markerDestination = this.startPosition;
		this.source = this.marker.GetComponent<AudioSource>();
		this.view = base.GetComponent<PhotonView>();
		this.photonInteract = base.GetComponent<PhotonObjectInteract>();
		this.rend = base.GetComponent<Renderer>();
		this.noise.gameObject.SetActive(false);
	}

	private void Start()
	{
		this.photonInteract.AddUseEvent(new UnityAction(this.Use));
		this.SetupKeywords();
		if(SpeechRecognitionController.instance != null)
        {
			SpeechRecognitionController.instance.AddOuijaBoard(this);
		}
		if (PlayerPrefs.GetInt("localPushToTalkValue") == 0 && GameController.instance != null)
		{
			if (GameController.instance.myPlayer == null)
			{
				GameController.instance.OnLocalPlayerSpawned.AddListener(new UnityAction(this.OnPlayerSpawned));
			}
			else
			{
				this.OnPlayerSpawned();
			}
		}
		if (PhotonNetwork.IsMasterClient && UnityEngine.Random.Range(0, 2) == 1 && PlayerPrefs.GetInt("isYoutuberVersion") == 0)
		{
			base.Invoke("DisableDelay", 10f);
		}
	}

	private void DisableDelay()
	{
		this.view.RPC("DisableOuijaBoard", RpcTarget.AllBuffered, Array.Empty<object>());
	}

	[PunRPC]
	private void DisableOuijaBoard()
	{
		base.gameObject.SetActive(false);
	}

	public void SetupKeywords()
	{
		Question item = new Question
		{
			questions = new List<string>
			{
				LocalisationSystem.GetLocalisedValue("Ouija_Victim1"),
				LocalisationSystem.GetLocalisedValue("Ouija_Victim2"),
				LocalisationSystem.GetLocalisedValue("Ouija_Victim3"),
				LocalisationSystem.GetLocalisedValue("Ouija_Victim4"),
				LocalisationSystem.GetLocalisedValue("Ouija_Victim5"),
				LocalisationSystem.GetLocalisedValue("Ouija_Victim6"),
				LocalisationSystem.GetLocalisedValue("Ouija_Victim7"),
				LocalisationSystem.GetLocalisedValue("Ouija_Victim8")
			},
			answerType = Question.AnswerType.victim
		};
		this.questions.Add(item);
		item = new Question
		{
			questions = new List<string>
			{
				LocalisationSystem.GetLocalisedValue("Ouija_Age1"),
				LocalisationSystem.GetLocalisedValue("Ouija_Age2"),
				LocalisationSystem.GetLocalisedValue("Ouija_Age3"),
				LocalisationSystem.GetLocalisedValue("Ouija_Age4")
			},
			answerType = Question.AnswerType.age
		};
		this.questions.Add(item);
		item = new Question
		{
			questions = new List<string>
			{
				LocalisationSystem.GetLocalisedValue("Ouija_Dead1"),
				LocalisationSystem.GetLocalisedValue("Ouija_Dead2"),
				LocalisationSystem.GetLocalisedValue("Ouija_Dead3"),
				LocalisationSystem.GetLocalisedValue("Ouija_Dead4"),
				LocalisationSystem.GetLocalisedValue("Ouija_Dead5")
			},
			answerType = Question.AnswerType.dead
		};
		this.questions.Add(item);
		item = new Question
		{
			questions = new List<string>
			{
				LocalisationSystem.GetLocalisedValue("Ouija_RoomAmount1"),
				LocalisationSystem.GetLocalisedValue("Ouija_RoomAmount2"),
				LocalisationSystem.GetLocalisedValue("Ouija_RoomAmount3"),
				LocalisationSystem.GetLocalisedValue("Ouija_RoomAmount4"),
				LocalisationSystem.GetLocalisedValue("Ouija_RoomAmount5"),
				LocalisationSystem.GetLocalisedValue("Ouija_RoomAmount6"),
				LocalisationSystem.GetLocalisedValue("Ouija_RoomAmount7"),
				LocalisationSystem.GetLocalisedValue("Ouija_RoomAmount8"),
				LocalisationSystem.GetLocalisedValue("Ouija_RoomAmount9")
			},
			answerType = Question.AnswerType.roomAmount
		};
		this.questions.Add(item);
		item = new Question
		{
			questions = new List<string>
			{
				LocalisationSystem.GetLocalisedValue("Ouija_Location1"),
				LocalisationSystem.GetLocalisedValue("Ouija_Location2"),
				LocalisationSystem.GetLocalisedValue("Ouija_Location3"),
				LocalisationSystem.GetLocalisedValue("Ouija_Location4"),
				LocalisationSystem.GetLocalisedValue("Ouija_Location5"),
				LocalisationSystem.GetLocalisedValue("Ouija_Location6"),
				LocalisationSystem.GetLocalisedValue("Ouija_Location7"),
				LocalisationSystem.GetLocalisedValue("Ouija_Location8"),
				LocalisationSystem.GetLocalisedValue("Ouija_Location9")
			},
			answerType = Question.AnswerType.location
		};
		this.questions.Add(item);
		if(SpeechRecognitionController.instance != null)
        {
			for (int i = 0; i < this.questions.Count; i++)
			{
				for (int j = 0; j < this.questions[i].questions.Count; j++)
				{
					SpeechRecognitionController.instance.AddKeyword(this.questions[i].questions[j]);
				}
			}
			for (int k = 0; k < this.yesOrNoQuestions.Count; k++)
			{
				for (int l = 0; l < this.yesOrNoQuestions[k].questions.Count; l++)
				{
					SpeechRecognitionController.instance.AddKeyword(this.yesOrNoQuestions[k].questions[l]);
				}
			}
		}
	}

	private void Update()
	{
		if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (this.lettersList.Count > 0)
		{
			if (this.markerDestination == this.startPosition)
			{
				this.markerDestination = this.GetLetterPosition(this.lettersList[0]);
			}
			if (Vector3.Distance(this.marker.localPosition, this.markerDestination) < 0.1f && !this.reachedDestination)
			{
				this.reachedDestination = true;
				base.StartCoroutine(this.GetNewLetter());
			}
			this.marker.localPosition = Vector3.MoveTowards(this.marker.localPosition, this.markerDestination, 4f * Time.deltaTime * UnityEngine.Random.Range(0f, 2f));
			return;
		}
		if (this.source.isPlaying)
		{
            if (PhotonNetwork.InRoom)
            {
				this.view.RPC("Sound", RpcTarget.All, new object[]
				{
					false
				});
				return;
			}
			Sound(false);
		}
	}

	private IEnumerator GetNewLetter()
	{
		if (PhotonNetwork.InRoom)
        {
			this.view.RPC("Sound", RpcTarget.All, new object[]
			{
				false
			});
		}
		else
        {
			Sound(false);
        }
		
		yield return new WaitForSeconds(1f);
		if (PhotonNetwork.InRoom)
        {
			this.view.RPC("Sound", RpcTarget.All, new object[]
			{
				true
			});
        }
        else
        {
			Sound(true);
        }
		this.lettersList.RemoveAt(0);
		if (this.lettersList.Count > 0)
		{
			this.markerDestination = this.GetLetterPosition(this.lettersList[0]);
		}
		else
		{
			this.markerDestination = this.startPosition;
		}
		this.reachedDestination = false;
		yield break;
	}

	private void Use()
	{
        if (PhotonNetwork.InRoom)
        {
			this.view.RPC("OuijaBoardNetworkedUse", RpcTarget.All, Array.Empty<object>());
			return;
		}
		OuijaBoardNetworkedUse();
	}

	[PunRPC]
	private void OuijaBoardNetworkedUse()
	{
		this.inUse = !this.inUse;
		if (this.inUse)
		{
			this.rend.material.EnableKeyword("_EMISSION");
			return;
		}
		this.rend.material.DisableKeyword("_EMISSION");
	}

	[PunRPC]
	private void Sound(bool on)
	{
		if (on)
		{
			this.source.Play();
			this.noise.gameObject.SetActive(true);
			return;
		}
		this.source.Stop();
		this.noise.gameObject.SetActive(false);
	}

	public void OnPhraseRecognized(string args)
	{
		if (!this.inUse)
		{
			return;
		}
		if (LevelController.instance != null & LevelController.instance.currentPlayerRoom == null || LevelController.instance.currentGhostRoom == null)
		{
			return;
		}
		if (LevelController.instance != null && LevelController.instance.currentPlayerRoom == LevelController.instance.outsideRoom)
		{
			return;
		}
		if (GameController.instance != null && Vector3.Distance(base.transform.position, GameController.instance.myPlayer.player.headObject.transform.position) > 5f)
		{
			return;
		}
		if (!XRDevice.isPresent && PlayerPrefs.GetInt("localPushToTalkValue") == 0 && PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount > 1 && !this.voipKeyIsPressed)
		{
			return;
		}
		if (UnityEngine.Random.Range(0, 2) == 1 && PlayerPrefs.GetInt("isYoutuberVersion") == 0)
		{
			for (int i = 0; i < LevelController.instance.currentPlayerRoom.lightSwitches.Count; i++)
			{
				LevelController.instance.currentPlayerRoom.lightSwitches[i].view.RPC("FlickerNetworked", RpcTarget.All, Array.Empty<object>());
			}
			if (SetupPhaseController.instance.isSetupPhase)
			{
				SetupPhaseController.instance.ForceEnterHuntingPhase();
			}
			GameController.instance.myPlayer.player.insanity += 50f;
			this.Use();
			return;
		}
		for (int j = 0; j < this.questions.Count; j++)
		{
			for (int k = 0; k < this.questions[j].questions.Count; k++)
			{
				if (args == this.questions[j].questions[k])
				{
					switch (this.questions[j].answerType)
					{
						case Question.AnswerType.victim:
							this.view.RPC("VictimAnswer", PhotonNetwork.MasterClient, Array.Empty<object>());
							break;
						case Question.AnswerType.dead:
							this.view.RPC("DeadAnswer", PhotonNetwork.MasterClient, Array.Empty<object>());
							break;
						case Question.AnswerType.roomAmount:
							this.view.RPC("RoomAnswer", PhotonNetwork.MasterClient, Array.Empty<object>());
							break;
						case Question.AnswerType.location:
							this.view.RPC("LocationAnswer", PhotonNetwork.MasterClient, Array.Empty<object>());
							break;
						case Question.AnswerType.age:
							this.view.RPC("AgeAnswer", PhotonNetwork.MasterClient, Array.Empty<object>());
							break;
					}
					if (LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType != GhostTraits.Type.Demon)
					{
						GameController.instance.myPlayer.player.insanity += UnityEngine.Random.Range(5f, 10f);
					}
					return;
				}
			}
		}
		for (int l = 0; l < this.yesOrNoQuestions.Count; l++)
		{
			for (int m = 0; m < this.yesOrNoQuestions[l].questions.Count; m++)
			{
				if (args == this.yesOrNoQuestions[l].questions[m])
				{
					if (UnityEngine.Random.Range(0, 3) == 2)
					{
						this.view.RPC("MaybeAnswer", PhotonNetwork.MasterClient, Array.Empty<object>());
					}
					else
					{
						GhostInfo ghostInfo = LevelController.instance.currentGhost.ghostInfo;
						if (this.yesOrNoQuestions[l].questionType == YesNoMaybeQuestion.QuestionType.location)
						{
							if (Vector3.Distance(ghostInfo.transform.position, base.transform.position) < 3f)
							{
								this.view.RPC("NoAnswer", PhotonNetwork.MasterClient, Array.Empty<object>());
							}
							else
							{
								this.view.RPC("YesAnswer", PhotonNetwork.MasterClient, Array.Empty<object>());
							}
						}
						else
						{
							this.view.RPC("MaybeAnswer", PhotonNetwork.MasterClient, Array.Empty<object>());
						}
					}
					if (LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostType != GhostTraits.Type.Demon)
					{
						GameController.instance.myPlayer.player.insanity += UnityEngine.Random.Range(5f, 10f);
					}
					return;
				}
			}
		}
	}

	[PunRPC]
	private void VictimAnswer()
	{
		this.Answer(LevelController.instance.currentGhost.ghostInfo.ghostTraits.victim.firstName);
	}

	[PunRPC]
	private void LocationAnswer()
	{
		this.Answer(LevelController.instance.currentGhostRoom.roomName.ToString());
	}

	[PunRPC]
	private void RoomAnswer()
	{
		this.Answer((this.GetCurrentRoom().playersInRoom.Count + ((this.GetCurrentRoom() == LevelController.instance.currentGhostRoom) ? 1 : 0)).ToString());
	}

	[PunRPC]
	private void YesAnswer()
	{
		this.Answer("yes");
	}

	[PunRPC]
	private void NoAnswer()
	{
		this.Answer("no");
	}

	[PunRPC]
	private void MaybeAnswer()
	{
		this.Answer("maybe");
	}

	[PunRPC]
	private void AgeAnswer()
	{
		this.Answer(LevelController.instance.currentGhost.ghostInfo.ghostTraits.ghostAge.ToString());
	}

	[PunRPC]
	private void DeadAnswer()
	{
		this.Answer(LevelController.instance.currentGhost.ghostInfo.ghostTraits.deathLength.ToString());
	}

	public void Answer(string msg)
	{
		DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.OuijaBoardResponse, 1);
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		//LevelController.instance.currentGhost.ghostInteraction.CreateInteractionEMF(base.transform.position);
		this.lettersList.Clear();
		this.view.RPC("Sound", RpcTarget.All, new object[]
		{
			true
		});
		msg = msg.ToLower();
		msg = msg.Replace(" ", "");
		if (msg == "yes" || msg == "no" || msg == "maybe")
		{
			this.lettersList.Add(msg);
			return;
		}
		foreach (char c in msg.ToCharArray())
		{
			this.lettersList.Add(c.ToString());
		}
	}

	private Vector3 GetLetterPosition(string letter)
	{
		if (letter == "4")
		{
			return this.letterPositions[29].localPosition;
		}
		else if (letter == "5")
		{
			return this.letterPositions[30].localPosition;
		}
		else if (letter == "6")
		{
			return this.letterPositions[31].localPosition;
		}
		else if (letter == "7")
		{
			return this.letterPositions[32].localPosition;
		}
		else if (letter == "0")
		{
			return this.letterPositions[35].localPosition;
		}
		else if (letter == "1")
		{
			return this.letterPositions[26].localPosition;
		}
		else if (letter == "9")
		{
			return this.letterPositions[34].localPosition;
		}
		else if (letter == "2")
		{
			return this.letterPositions[27].localPosition;
		}
		else if (letter == "3")
		{
			return this.letterPositions[28].localPosition;
		}
		else if (letter == "yes")
		{
			return this.letterPositions[36].localPosition;
		}
		else if (letter == "8")
		{
			return this.letterPositions[33].localPosition;
		}
		else if (letter == "d")
		{
			return this.letterPositions[3].localPosition;
		}
		else if (letter == "e")
		{
			return this.letterPositions[4].localPosition;
		}
		else if (letter == "no")
		{
			return this.letterPositions[37].localPosition;
		}
		else if (letter == "g")
		{
			return this.letterPositions[6].localPosition;
		}
		else if (letter == "maybe")
		{
			return this.letterPositions[38].localPosition;
		}
		else if (letter == "c")
		{
			return this.letterPositions[2].localPosition;
		}
		else if (letter == "a")
		{
			return this.letterPositions[0].localPosition;
		}
		else if (letter == "f")
		{
			return this.letterPositions[5].localPosition;
		}
		else if (letter == "m")
		{
			return this.letterPositions[12].localPosition;
		}
		else if (letter == "b")
		{
			return this.letterPositions[1].localPosition;
		}
		else if (letter == "n")
		{
			return this.letterPositions[13].localPosition;
		}
		else if (letter == "o")
		{
			return this.letterPositions[14].localPosition;
		}
		else if (letter == "l")
		{
			return this.letterPositions[11].localPosition;
		}
		else if (letter == "h")
		{
			return this.letterPositions[7].localPosition;
		}
		else if (letter == "i")
		{
			return this.letterPositions[8].localPosition;
		}
		else if (letter == "u")
		{
			return this.letterPositions[20].localPosition;
		}
		else if (letter == "j")
		{
			return this.letterPositions[9].localPosition;
		}
		else if (letter == "k")
		{
			return this.letterPositions[10].localPosition;
		}
		else if (letter == "w")
		{
			return this.letterPositions[22].localPosition;
		}
		else if (letter == "t")
		{
			return this.letterPositions[19].localPosition;
		}
		else if (letter == "p")
		{
			return this.letterPositions[15].localPosition;
		}
		else if (letter == "q")
		{
			return this.letterPositions[16].localPosition;
		}
		else if (letter == "v")
		{
			return this.letterPositions[21].localPosition;
		}
		else if (letter == "r")
		{
			return this.letterPositions[17].localPosition;
		}
		else if (letter == "s")
		{
			return this.letterPositions[18].localPosition;
		}
		else if (letter == "z")
		{
			return this.letterPositions[25].localPosition;
		}
		else if (letter == "x")
		{
			return this.letterPositions[23].localPosition;
		}
		else if (letter == "y")
		{
			return this.letterPositions[24].localPosition;
		}
		Debug.LogError(string.Concat(new object[]
		{
			"Letter ",
			letter,
			" could not be found on ",
			this
		}));
		return this.letterPositions[38].localPosition;
	}

	private LevelRoom GetCurrentRoom()
	{
		LevelRoom[] array = UnityEngine.Object.FindObjectsOfType<LevelRoom>();
		LevelRoom levelRoom = array[0];
		for (int i = 0; i < array.Length; i++)
		{
			if (Vector3.Distance(base.transform.position, array[i].transform.position) < Vector3.Distance(base.transform.position, levelRoom.transform.position))
			{
				levelRoom = array[i];
			}
		}
		return levelRoom;
	}

	private void OnPlayerSpawned()
	{
		if (!XRDevice.isPresent)
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

	private void OnDisable()
	{
		if (PlayerPrefs.GetInt("localPushToTalkValue") == 0 && GameController.instance != null && GameController.instance.myPlayer != null && GameController.instance.myPlayer.player != null && GameController.instance.myPlayer.player.playerInput != null && !XRDevice.isPresent)
		{
			GameController.instance.myPlayer.player.playerInput.actions["LocalPushToTalk"].started -= delegate (InputAction.CallbackContext _)
			{
				this.PushToTalkStarted();
			};
			GameController.instance.myPlayer.player.playerInput.actions["LocalPushToTalk"].canceled -= delegate (InputAction.CallbackContext _)
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
	private List<Question> questions = new List<Question>();

	private List<YesNoMaybeQuestion> yesOrNoQuestions = new List<YesNoMaybeQuestion>();

	private bool inUse;

	public Transform marker;

	public List<Transform> letterPositions = new List<Transform>();

	private List<string> lettersList = new List<string>();

	private Vector3 markerDestination;

	private Vector3 startPosition;

	private bool reachedDestination;

	private bool voipKeyIsPressed;

	private AudioSource source;

	private PhotonView view;

	private PhotonObjectInteract photonInteract;

	private Noise noise;

	private Renderer rend;
}

