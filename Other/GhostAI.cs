using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class GhostAI : MonoBehaviourPunCallbacks
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
		this.ghostInfo = base.GetComponent<GhostInfo>();
		this.ghostAudio = base.GetComponent<GhostAudio>();
		this.agent = base.GetComponent<NavMeshAgent>();
		this.ghostInteraction = base.GetComponent<GhostInteraction>();
		this.sanityDrainer = base.GetComponent<SanityDrainer>();
		this.canAttack = true;
		this.defaultSpeed = this.agent.speed;
		this.sanityDrainer.enabled = false;
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>())
		{
			this.myRends.Add(renderer);
			renderer.enabled = false;
		}
	}

	private void Start()
	{
		LevelController.instance.currentGhost = this;
		if (!this.view.IsMine)
		{
			this.agent.enabled = false;
		}
		if (this.view.IsMine)
		{
			this.ChangeState(GhostAI.States.idle, null, null);
		}
		//base.StartCoroutine(this.SpawnDelay());
	}

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
		if (PhotonNetwork.IsMasterClient)
		{
			this.agent.enabled = true;
			this.ChangeState(GhostAI.States.favouriteRoom, null, null);
		}
	}

	private IEnumerator SpawnDelay()
	{
		yield return new WaitForSeconds(2f);
		if (PhotonNetwork.IsMasterClient)
		{
			this.view.RPC("SyncGhostType", RpcTarget.AllBufferedViaServer, new object[]
			{
				this.ghostInfo.ghostTraits.ghostType.ToString()
			});
		}
		yield break;
	}

	public IEnumerator StartHuntingTimer()
	{
		this.canEnterHuntingMode = false;
		yield return new WaitForSeconds(25f);
		this.canEnterHuntingMode = true;
		yield break;
	}

	[PunRPC]
	private void SyncGhostType(string name)
	{
		if (this.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Banshee)
		{
			if (GameController.instance.playersData.Count == PhotonNetwork.CurrentRoom.PlayerCount)
			{
				this.SetInitialBansheeTarget();
				return;
			}
			GameController.instance.OnAllPlayersConnected.AddListener(new UnityAction(this.SetInitialBansheeTarget));
		}
	}

	private void Update()
	{
        if (test)
        {
			test = !test;
			Hunting(true);
		}
		if (this.view.IsMine && this.stateMachine != null && !GameController.instance.isLoadingBackToMenu)
		{
			this.stateMachine.ExecuteStateUpdate();
		}
		if (this.isHunting)
		{
			this.appearTimer -= Time.deltaTime;
			if (this.appearTimer < 0f)
			{
				this.FlashAppear(UnityEngine.Random.Range(0.08f, 0.3f));
				this.appearTimer = ((this.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Phantom) ? UnityEngine.Random.Range(1f, 2f) : UnityEngine.Random.Range(0.3f, 1f));
			}
		}
	}

	public void ChangeState(GhostAI.States state, PhotonObjectInteract obj = null, PhotonObjectInteract[] objects = null)
	{
		if (!this.view.IsMine)
		{
			return;
		}
		this.state = state;
		switch (state)
		{
			case GhostAI.States.idle:
				this.stateMachine.ChangeState(new IdleState(this));
				return;
			case GhostAI.States.wander:
				this.stateMachine.ChangeState(new WanderState(this, this.agent));
				return;
			case GhostAI.States.hunting:
				this.stateMachine.ChangeState(new HuntingState(this, this.agent, this.view));
				return;
			case GhostAI.States.favouriteRoom:
				this.stateMachine.ChangeState(new FavouriteRoomState(this, this.agent));
				return;
			case GhostAI.States.light:
				this.stateMachine.ChangeState(new LightState(this, this.ghostInteraction, obj));
				return;
			case GhostAI.States.door:
				this.stateMachine.ChangeState(new DoorState(this, this.ghostInteraction, this.ghostInfo, obj));
				return;
			case GhostAI.States.throwing:
				this.stateMachine.ChangeState(new ThrowingState(this, this.ghostInteraction, obj));
				return;
			case GhostAI.States.fusebox:
				this.stateMachine.ChangeState(new FuseBoxState(this, this.ghostInteraction));
				return;
			case GhostAI.States.appear:
				this.stateMachine.ChangeState(new AppearState(this, this.agent));
				return;
			case GhostAI.States.doorKnock:
				this.stateMachine.ChangeState(new DoorKnockState(this, this.ghostInteraction));
				return;
			case GhostAI.States.windowKnock:
				this.stateMachine.ChangeState(new WindowKnockState(this, this.ghostInteraction, obj));
				return;
			case GhostAI.States.carAlarm:
				this.stateMachine.ChangeState(new CarAlarmState(this, this.ghostInteraction));
				return;
			case GhostAI.States.radio:
				this.stateMachine.ChangeState(new RadioState(this, this.ghostInteraction));
				return;
			case GhostAI.States.flicker:
				this.stateMachine.ChangeState(new LightFlickerState(this, this.ghostInteraction));
				return;
			case GhostAI.States.lockDoor:
				this.stateMachine.ChangeState(new DoorLockState(this, this.ghostInteraction, obj));
				return;
			case GhostAI.States.cctv:
				this.stateMachine.ChangeState(new CCTVState(this, this.ghostInteraction, obj));
				return;
			case GhostAI.States.randomEvent:
				this.RandomEvent();
				return;
			case GhostAI.States.GhostAbility:
				this.PerformGhostAbility(obj, objects);
				return;
			case GhostAI.States.killPlayer:
				this.stateMachine.ChangeState(new KillPlayerState(this, this.agent, this.view));
				return;
			case GhostAI.States.sink:
				this.stateMachine.ChangeState(new SinkState(this, this.ghostInteraction, obj));
				return;
			case GhostAI.States.sound:
				this.stateMachine.ChangeState(new SoundState(this, this.ghostInteraction, obj));
				return;
			case GhostAI.States.painting:
				this.stateMachine.ChangeState(new PaintingState(this, this.ghostInteraction, obj));
				return;
			case GhostAI.States.mannequin:
				this.stateMachine.ChangeState(new MannequinState(this, this.ghostInteraction, obj));
				return;
			case GhostAI.States.teleportObject:
				this.stateMachine.ChangeState(new TeleportObjectState(this, this.ghostInteraction, obj));
				return;
			case GhostAI.States.animationObject:
				this.stateMachine.ChangeState(new AnimationObjectState(this, this.ghostInteraction, obj));
				return;
			default:
				Debug.LogError("There is no AI switch statement for state: " + state);
				return;
		}
	}

	public void RandomEvent()
	{
		switch (UnityEngine.Random.Range(0, 4))
		{
			case 0:
				this.stateMachine.ChangeState(new GhostEvent_1(this, this.ghostInteraction));
				return;
			case 1:
				this.stateMachine.ChangeState(new GhostEvent_2(this, this.ghostInteraction));
				return;
			case 2:
				this.stateMachine.ChangeState(new GhostEvent_3(this, this.ghostInteraction));
				return;
			case 3:
				this.stateMachine.ChangeState(new GhostEvent_4(this, this.ghostInteraction));
				return;
			default:
				Debug.LogError("Failed: trying to start a random Ghost event that has not been setup.");
				return;
		}
	}

	private void PerformGhostAbility(PhotonObjectInteract obj = null, PhotonObjectInteract[] objects = null)
	{
		switch (this.ghostInfo.ghostTraits.ghostType)
		{
			case GhostTraits.Type.Spirit:
			case GhostTraits.Type.Mare:
			case GhostTraits.Type.Revenant:
				break;
			case GhostTraits.Type.Wraith:
				this.stateMachine.ChangeState(new WraithPower(this, this.ghostInteraction, this.agent));
				return;
			case GhostTraits.Type.Phantom:
				this.stateMachine.ChangeState(new PhantomPower(this, this.ghostInteraction, this.agent));
				return;
			case GhostTraits.Type.Poltergeist:
				this.stateMachine.ChangeState(new PoltergeistPower(this, this.ghostInteraction, this.mask, objects));
				return;
			case GhostTraits.Type.Banshee:
				this.stateMachine.ChangeState(new BansheePower(this, this.ghostInteraction, this.ghostAudio, this.agent, this.mask));
				return;
			case GhostTraits.Type.Jinn:
				this.stateMachine.ChangeState(new JinnPower(this, this.ghostInteraction));
				break;
			default:
				return;
		}
	}

	public IEnumerator ResetRigidbody(Rigidbody rigid, Door door = null)
	{
		yield return new WaitForSeconds(3f);
		rigid.isKinematic = true;
		if (door)
		{
			door.UnGrabbedDoor();
		}
		yield break;
	}

	public IEnumerator StopGhostFromHunting()
	{
		this.canAttack = false;
		yield return new WaitForSeconds((float)((this.ghostInfo.ghostTraits.ghostType == GhostTraits.Type.Spirit) ? 180 : 90));
		this.canAttack = true;
		yield break;
	}

	public IEnumerator StopHuntingFortime()
	{
		if (!this.isHunting)
		{
			yield return null;
		}
		this.smudgeSticksUsed = true;
		yield return new WaitForSeconds(1f);
		this.agent.SetDestination(base.transform.position);
		yield return new WaitForSeconds(5f);
		this.smudgeSticksUsed = false;
		yield break;
	}

	public IEnumerator TemporarilyStopWander()
	{
		this.canWander = false;
		yield return new WaitForSeconds(90f);
		this.canWander = true;
		yield break;
	}

	[PunRPC]
	private void Hunting(bool isHunting)
	{
		this.isHunting = isHunting;
		if (isHunting)
		{
			for (int i = 0; i < LevelController.instance.fuseBox.switches.Count; i++)
			{
				LevelController.instance.fuseBox.switches[i].StartBlinking();
			}
			for (int j = 0; j < GameController.instance.playersData.Count; j++)
			{
				if (GameController.instance.playersData[j].player.pcFlashlight != null)
				{
					GameController.instance.playersData[j].player.pcFlashlight.TurnBlinkOnOrOff(true);
				}
			}
			this.canEnterHuntingMode = false;
			return;
		}
		for (int k = 0; k < LevelController.instance.fuseBox.switches.Count; k++)
		{
			LevelController.instance.fuseBox.switches[k].StopBlinking();
		}
		for (int l = 0; l < LevelController.instance.torches.Count; l++)
		{
			if (LevelController.instance.torches[l] != null)
			{
				LevelController.instance.torches[l].TurnBlinkOff();
			}
		}
		for (int m = 0; m < GameController.instance.playersData.Count; m++)
		{
			if (GameController.instance.playersData[m].player.pcFlashlight != null)
			{
				GameController.instance.playersData[m].player.pcFlashlight.TurnBlinkOnOrOff(false);
			}
		}
		this.ghostAudio.TurnOnOrOffAppearSource(false);
		this.ghostAudio.PlayOrStopAppearSource(false);
		this.UnAppear(false);
		base.StartCoroutine(this.StartHuntingTimer());
		if (!GameController.instance.myPlayer.player.isDead)
		{
			DailyChallengesController.Instance.ChangeChallengeProgression(ChallengeType.SurviveHunting, 1);
		}
	}

	[PunRPC]
	private void MakeGhostAppear(bool appear, int randNum)
	{
		if (!appear && !this.ghostIsAppeared)
		{
			return;
		}
		this.ghostIsAppeared = appear;
		foreach (Renderer renderer in this.myRends)
		{
			renderer.enabled = appear;
		}
		if (!GameController.instance.myPlayer.player.isDead)
		{
			this.sanityDrainer.enabled = appear;
		}
		if (appear)
		{
			if (!GameController.instance.myPlayer.player.isDead && randNum == 1)
			{
				for (int i = 0; i < this.myRends.Count; i++)
				{
					this.myRends[i].shadowCastingMode = ShadowCastingMode.ShadowsOnly;
				}
				return;
			}
		}
		else
		{
			for (int j = 0; j < this.myRends.Count; j++)
			{
				this.myRends[j].shadowCastingMode = ShadowCastingMode.On;
			}
		}
	}

	public void UnAppear(bool isEvent)
	{
		this.view.RPC("MakeGhostAppear", RpcTarget.All, new object[]
		{
			false,
			UnityEngine.Random.Range(0, isEvent ? 2 : 3)
		});
	}

	public void Appear(bool isEvent)
	{
		this.view.RPC("MakeGhostAppear", RpcTarget.All, new object[]
		{
			true,
			UnityEngine.Random.Range(0, isEvent ? 2 : 3)
		});
	}

	public void FlashAppear(float timer)
	{
		if (!GameController.instance.myPlayer.player.isDead)
		{
			base.StartCoroutine(this.Flash(timer));
		}
	}

	private IEnumerator Flash(float timer)
	{
		this.ghostIsAppeared = true;
		this.ghostAudio.PlayOrStopAppearSource(true);
		foreach (Renderer renderer in this.myRends)
		{
			renderer.enabled = true;
		}
		yield return new WaitForSeconds(timer);
		this.ghostIsAppeared = false;
		this.ghostAudio.PlayOrStopAppearSource(false);
		using (List<Renderer>.Enumerator enumerator = this.myRends.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Renderer renderer2 = enumerator.Current;
				renderer2.enabled = false;
			}
			yield break;
		}
		yield break;
	}

	public void JinnPowerDistanceCheck()
	{
		this.view.RPC("JinnPowerDistanceCheckNetworked", RpcTarget.All, Array.Empty<object>());
	}

	[PunRPC]
	private void JinnPowerDistanceCheckNetworked()
	{
		if (Vector3.Distance(base.transform.position, GameController.instance.myPlayer.player.transform.position) < 3f)
		{
			GameController.instance.myPlayer.player.insanity += 25f;
		}
	}

	public void SetInitialBansheeTarget()
	{
		this.view.RPC("SetBansheeTargetNetworked", RpcTarget.AllBufferedViaServer, new object[]
		{
			GameController.instance.playersData[UnityEngine.Random.Range(0, GameController.instance.playersData.Count)].actorID
		});
	}

	public void SetNewBansheeTarget()
	{
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (!GameController.instance.playersData[i].player.isDead)
			{
				this.view.RPC("SetBansheeTargetNetworked", RpcTarget.AllBufferedViaServer, new object[]
				{
					GameController.instance.playersData[i].actorID
				});
				return;
			}
		}
	}

	[PunRPC]
	private void SetBansheeTargetNetworked(int actorID)
	{
		for (int i = 0; i < GameController.instance.playersData.Count; i++)
		{
			if (GameController.instance.playersData[i].actorID == actorID)
			{
				this.bansheeTarget = GameController.instance.playersData[i].player;
				return;
			}
		}
	}

	public void ActivateGhostParticleEvidence()
	{
		this.ghostParticle.GetComponent<ParticleSystemRenderer>().enabled = true;
		this.ghostParticle.Play();
	}

	public void ChasingPlayer(bool isChasing)
	{
		this.view.RPC("SyncChasingPlayer", RpcTarget.All, new object[]
		{
			isChasing
		});
	}

	[PunRPC]
	private void SyncChasingPlayer(bool isChasing)
	{
		this.isChasingPlayer = isChasing;
	}

	private StateMachine stateMachine = new StateMachine();

	public GhostAI.States state;

	[HideInInspector]
	public PhotonView view;

	public Animator anim;

	[HideInInspector]
	public GhostInfo ghostInfo;

	[HideInInspector]
	public NavMeshAgent agent;

	[HideInInspector]
	public GhostAudio ghostAudio;

	[HideInInspector]
	public GhostInteraction ghostInteraction;

	public GhostActivity ghostActivity;

	[Header("Appear State")]
	[HideInInspector]
	public List<Renderer> myRends = new List<Renderer>();

	[HideInInspector]
	public bool ghostIsAppeared;

	[HideInInspector]
	public SanityDrainer sanityDrainer;

	[Header("Hunting State")]
	public LayerMask mask;

	public Transform raycastPoint;

	[HideInInspector]
	public bool isChasingPlayer;

	[HideInInspector]
	public float defaultSpeed;

	public bool canEnterHuntingMode = true;

	private float appearTimer = 1f;

	public bool isHunting;

	[HideInInspector]
	public bool canAttack = true;

	[HideInInspector]
	public bool smudgeSticksUsed;

	[HideInInspector]
	public bool canWander;

	[Header("Banshee")]
	[HideInInspector]
	public Player bansheeTarget;

	[SerializeField]
	private ParticleSystem ghostParticle;

	[HideInInspector]
	public Player playerToKill;

	public bool test;

	public enum States
	{
		idle,
		wander,
		hunting,
		favouriteRoom,
		light,
		door,
		throwing,
		fusebox,
		appear,
		doorKnock,
		windowKnock,
		carAlarm,
		radio,
		flicker,
		lockDoor,
		cctv,
		randomEvent,
		GhostAbility,
		killPlayer,
		sink,
		sound,
		painting,
		mannequin,
		teleportObject,
		animationObject
	}
}

