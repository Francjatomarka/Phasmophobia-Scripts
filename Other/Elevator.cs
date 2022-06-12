using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Elevator : MonoBehaviour
{
	private void Awake()
	{
		if (base.GetComponentInChildren<ReflectionProbe>())
		{
			this.probe = base.GetComponentInChildren<ReflectionProbe>();
			this.probe.refreshMode = ReflectionProbeRefreshMode.OnAwake;
			this.isReflectionProbe = true;
		}
		else
		{
			this.isReflectionProbe = false;
		}
		this.isPlayer = false;
		Elevator.Moving = false;
		this.BtnSoundFX = base.GetComponent<AudioSource>();
		if (base.transform.parent != null)
		{
			this.ElevatorsParent = base.transform.parent.gameObject;
			if (this.ElevatorsParent.GetComponent<ElevatorManager>())
			{
				this._elevatorManager = this.ElevatorsParent.GetComponent<ElevatorManager>();
			}
		}
		this.SoundFX = new GameObject().AddComponent<AudioSource>();
		this.SoundFX.transform.parent = base.gameObject.transform;
		this.SoundFX.transform.position = new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y + 2.2f, base.gameObject.transform.position.z);
		this.SoundFX.gameObject.name = "SoundFX";
		this.SoundFX.playOnAwake = false;
		this.SoundFX.spatialBlend = 1f;
		this.SoundFX.minDistance = 0.1f;
		this.SoundFX.maxDistance = 10f;
		this.SoundFX.rolloffMode = AudioRolloffMode.Linear;
		this.SoundFX.priority = 256;
		this.DoorsAnim = base.gameObject.GetComponent<Animation>();
		this.AnimName = this.DoorsAnim.clip.name;
		if (GameObject.FindGameObjectWithTag(this.PlayerTag))
		{
			this.Player = GameObject.FindGameObjectWithTag(this.PlayerTag).GetComponent<Rigidbody>();
			if (this.Player.gameObject.GetComponent<CapsuleCollider>())
			{
				this.PlayerHeight = this.Player.gameObject.GetComponent<CapsuleCollider>().height / 2f;
				this.isRigidbodyCharacter = true;
				this.isPlayer = true;
			}
			else if (this.Player.gameObject.GetComponent<CharacterController>())
			{
				this.PlayerHeight = this.Player.gameObject.GetComponent<CharacterController>().height / 2f + this.Player.gameObject.GetComponent<CharacterController>().skinWidth;
				this.isRigidbodyCharacter = false;
				this.isPlayer = true;
			}
		}
		else
		{
			Debug.LogWarning("Elevator: Can't find Player. Please, check that your Player object has 'Player' tag.");
			base.enabled = false;
			this.isPlayer = false;
		}
		if (this.isPlayer)
		{
			if (this.Player.GetComponentInChildren<Camera>().transform)
			{
				this.PlayerCam = this.Player.GetComponentInChildren<Camera>().transform;
			}
			else
			{
				Debug.LogWarning("Elevator: Can't find Player's camera. Please, check that your Player have a camera parented to it.");
				base.enabled = false;
			}
		}
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Elevator"))
		{
			if (gameObject.transform.parent == base.gameObject.transform.parent && gameObject != base.gameObject)
			{
				this.Elevators.Add(gameObject);
			}
		}
		if (this._elevatorManager)
		{
			ElevatorManager elevatorManager = this._elevatorManager;
			elevatorManager.WasStarted = (UnityAction)Delegate.Combine(elevatorManager.WasStarted, new UnityAction(this.RandomInit));
			return;
		}
		Debug.LogWarning("Elevator: To use more than one elevator shaft, please create an empty gameobject in your scene, add the ElevatorManager.cs script on it and make elevators of one elevator shaft as child to this object. Repeate this for every different elevators shafts.");
	}

	private void RandomInit()
	{
		if (this._elevatorManager)
		{
			this.ElevatorFloor = this._elevatorManager.InitialFloor;
		}
		else
		{
			this.ElevatorFloor = 1;
		}
		this.TextOutside.text = this.ElevatorFloor.ToString();
		this.TextInside.text = this.ElevatorFloor.ToString();
	}

	private void Update()
	{
		if (this.inTrigger)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				foreach (RaycastHit raycastHit in Physics.RaycastAll(this.PlayerCam.position, this.PlayerCam.forward, 3f))
				{
					if (raycastHit.transform.tag == "ElevatorButtonOpen" && !this.isOpen)
					{
						this.BtnSoundFX.clip = this.ElevatorBtn;
						this.BtnSoundFX.volume = this.ElevatorBtnVolume;
						this.BtnSoundFX.Play();
						this.ElevatorOpenButton = raycastHit.transform.GetComponent<MeshRenderer>();
						this.ElevatorOpenButton.enabled = true;
						this.isOpen = true;
						base.Invoke("DoorsOpening", this.OneFloorTime * (float)Mathf.Abs(this.CurrentFloor - this.ElevatorFloor) + this.OpenDelay);
						this.FloorCount = this.ElevatorFloor;
						this.ElevatorFloor = this.CurrentFloor;
						foreach (GameObject gameObject in this.Elevators)
						{
							((Elevator)gameObject.GetComponent(typeof(Elevator))).ElevatorFloor = this.CurrentFloor;
						}
						base.StartCoroutine("FloorsCounter");
					}
					if (raycastHit.transform.tag == "ElevatorNumericButton" && !Elevator.Moving)
					{
						this.InputFloor += raycastHit.transform.name;
						raycastHit.transform.GetComponent<MeshRenderer>().enabled = true;
						this.ElevatorNumericButtons.Add(raycastHit.transform.GetComponent<MeshRenderer>());
						this.BtnSoundFX.clip = this.ElevatorBtn;
						this.BtnSoundFX.volume = this.ElevatorBtnVolume;
						this.BtnSoundFX.Play();
					}
					if (raycastHit.transform.tag == "ElevatorGoButton" && !Elevator.Moving)
					{
						if (this.InputFloor != "" && this.InputFloor.Length < 4)
						{
							if (this.InputFloor == "0-1")
							{
								this.InputFloor = "-99";
							}
							this.TargetFloor = int.Parse(this.InputFloor);
							using (List<GameObject>.Enumerator enumerator = this.Elevators.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									GameObject gameObject2 = enumerator.Current;
									Elevator elevator = (Elevator)gameObject2.GetComponent(typeof(Elevator));
									if (elevator.CurrentFloor == this.TargetFloor)
									{
										this.ElvFound = true;
										this.TargetElvAnim = gameObject2.GetComponent<Animation>();
										this.TargetElvTextInside = gameObject2.GetComponent<Elevator>().TextInside;
										this.TargetElvTextOutside = gameObject2.GetComponent<Elevator>().TextOutside;
										this.BtnSoundFX.clip = this.ElevatorBtn;
										this.BtnSoundFX.volume = this.ElevatorBtnVolume;
										this.BtnSoundFX.Play();
										this.ElevatorFloor = this.TargetFloor;
										elevator.ElevatorFloor = this.TargetFloor;
										this.FloorCount = this.CurrentFloor;
										if (this.CurrentFloor != this.ElevatorFloor)
										{
											if (elevator.isReflectionProbe && elevator.UpdateReflectionEveryFrame)
											{
												elevator.probe.RenderProbe();
											}
											base.Invoke("ElevatorGO", 1f);
											this.ElevatorGoBtn = raycastHit.transform.GetComponent<MeshRenderer>();
											this.ElevatorGoBtn.enabled = true;
											Elevator.Moving = true;
										}
										else
										{
											this.DoorsOpening();
										}
										this.InputFloor = "";
									}
								}
								goto IL_403;
							}
							goto IL_3C5;
						}
						goto IL_3C5;
						IL_403:
						if (!this.ElvFound)
						{
							this.ButtonsReset();
							this.InputFloor = "";
							this.BtnSoundFX.clip = this.ElevatorError;
							this.BtnSoundFX.volume = this.ElevatorErrorVolume;
							this.BtnSoundFX.Play();
						}
						if (this.TargetFloor != this.CurrentFloor)
						{
							this.DoorsClosing();
							goto IL_46D;
						}
						if (!this.isOpen)
						{
							this.DoorsOpening();
							goto IL_46D;
						}
						goto IL_46D;
						IL_3C5:
						this.ButtonsReset();
						this.InputFloor = "";
						this.BtnSoundFX.clip = this.ElevatorError;
						this.BtnSoundFX.volume = this.ElevatorErrorVolume;
						this.BtnSoundFX.Play();
						goto IL_403;
					}
					IL_46D:;
				}
			}
			if (this.SpeedUp)
			{
				if (this.SoundFX.volume < this.ElevatorMoveVolume)
				{
					this.SoundFX.volume += 0.9f * Time.deltaTime;
				}
				else
				{
					this.SpeedUp = false;
				}
				if (this.SoundFX.pitch < 1f)
				{
					this.SoundFX.pitch += 0.9f * Time.deltaTime;
				}
			}
			if (this.SlowDown)
			{
				if (this.SoundFX.volume > 0f)
				{
					this.SoundFX.volume -= 0.9f * Time.deltaTime;
				}
				else
				{
					this.SlowDown = false;
				}
				if (this.SoundFX.pitch > 0f)
				{
					this.SoundFX.pitch -= 0.9f * Time.deltaTime;
				}
			}
		}
	}

	private void ElevatorGO()
	{
		this.ElvFound = false;
		base.StartCoroutine("FloorsCounterInside");
		this.SoundFX.clip = this.ElevatorMove;
		this.SoundFX.loop = true;
		this.SoundFX.volume = 0f;
		this.SoundFX.pitch = 0.5f;
		this.SpeedUp = true;
		this.SoundFX.Play();
	}

	private void SlowDownStart()
	{
		this.SlowDown = true;
	}

	private IEnumerator FloorsCounterInside()
	{
		for (;;)
		{
			this.TextOutside.text = this.FloorCount.ToString();
			this.TextInside.text = this.FloorCount.ToString();
			if (this.TargetFloor - this.FloorCount == 1)
			{
				base.Invoke("SlowDownStart", this.OneFloorTime / 2f);
			}
			if (this.FloorCount - this.TargetFloor == 1)
			{
				base.Invoke("SlowDownStart", this.OneFloorTime / 2f);
			}
			if (this.TargetFloor == this.FloorCount)
			{
				break;
			}
			yield return new WaitForSeconds(this.OneFloorTime);
			if (this.CurrentFloor < this.TargetFloor)
			{
				this.FloorCount++;
			}
			if (this.CurrentFloor > this.TargetFloor)
			{
				this.FloorCount--;
			}
			if (this.FloorCount == this.TargetFloor)
			{
				this.SoundFX.Stop();
				this.TargetBellSoundPlay();
				if (!this.isRigidbodyCharacter)
				{
					this.Player.isKinematic = false;
				}
				this.Player.transform.position = new Vector3(this.Player.transform.position.x, this.TargetElvAnim.transform.position.y + this.PlayerHeight, this.Player.transform.position.z);
				if (this.isReflectionProbe && this.UpdateReflectionEveryFrame)
				{
					this.probe.refreshMode = ReflectionProbeRefreshMode.OnAwake;
					this.probe.RenderProbe();
				}
				if (!this.isRigidbodyCharacter)
				{
					this.Player.isKinematic = true;
				}
				base.Invoke("TargetElvOpening", this.OpenDelay);
			}
		}
		yield break;
		yield break;
	}

	private IEnumerator FloorsCounter()
	{
		for (;;)
		{
			this.TextOutside.text = this.FloorCount.ToString();
			this.TextInside.text = this.FloorCount.ToString();
			if (this.CurrentFloor == this.FloorCount)
			{
				break;
			}
			yield return new WaitForSeconds(this.OneFloorTime);
			if (this.CurrentFloor < this.FloorCount)
			{
				this.FloorCount--;
			}
			if (this.CurrentFloor > this.FloorCount)
			{
				this.FloorCount++;
			}
		}
		this.BellSoundPlay();
		yield break;
		yield break;
	}

	private void DoorsClosingSoundPlay()
	{
		if (this.DoorsAnim[this.AnimName].speed < 0f)
		{
			this.SoundFX.clip = this.DoorsClose;
			this.SoundFX.loop = false;
			this.SoundFX.volume = this.DoorsCloseVolume;
			this.SoundFX.pitch = 1f;
			this.SoundFX.Play();
		}
	}

	private void DoorsOpeningSoundPlay()
	{
		if (this.DoorsAnim[this.AnimName].speed > 0f)
		{
			this.SoundFX.clip = this.DoorsOpen;
			this.SoundFX.volume = this.DoorsOpenVolume;
			this.SoundFX.pitch = 1f;
			this.SoundFX.Play();
		}
	}

	private void TargetBellSoundPlay()
	{
		foreach (GameObject gameObject in this.Elevators)
		{
			if (gameObject.GetComponent<Elevator>().CurrentFloor == this.TargetFloor)
			{
				this.TargetSoundFX = gameObject.GetComponent<Elevator>().SoundFX;
				this.TargetSoundFX.clip = this.Bell;
				this.TargetSoundFX.loop = false;
				this.TargetSoundFX.volume = this.BellVolume;
				this.TargetSoundFX.pitch = 1f;
				this.SoundFX.pitch = 1f;
				this.TargetSoundFX.Play();
				this.TextsUpdate();
			}
		}
	}

	private void BellSoundPlay()
	{
		this.SoundFX.clip = this.Bell;
		this.SoundFX.loop = false;
		this.SoundFX.volume = this.BellVolume;
		this.SoundFX.pitch = 1f;
		this.SoundFX.Play();
	}

	private void TextsUpdate()
	{
		foreach (GameObject gameObject in this.Elevators)
		{
			this.TargetElvTextInside = gameObject.GetComponent<Elevator>().TextInside;
			this.TargetElvTextOutside = gameObject.GetComponent<Elevator>().TextOutside;
			this.TargetElvTextInside.text = this.ElevatorFloor.ToString();
			this.TargetElvTextOutside.text = this.ElevatorFloor.ToString();
		}
	}

	private void ButtonsReset()
	{
		foreach (MeshRenderer meshRenderer in this.ElevatorNumericButtons)
		{
			meshRenderer.enabled = false;
		}
		if (this.ElevatorGoBtn != null)
		{
			this.ElevatorGoBtn.enabled = false;
		}
	}

	private void TargetElvOpening()
	{
		this.TextsUpdate();
		this.TargetElvAnim[this.AnimName].normalizedTime = 0f;
		this.TargetElvAnim[this.AnimName].speed = this.DoorsAnimSpeed;
		this.TargetElvAnim.Play();
		this.ButtonsReset();
	}

	private void DoorsOpening()
	{
		this.TargetFloor = 0;
		this.TextsUpdate();
		this.DoorsAnim[this.AnimName].normalizedTime = 0f;
		this.DoorsAnim[this.AnimName].speed = this.DoorsAnimSpeed;
		this.DoorsAnim.Play();
		this.ButtonsReset();
	}

	private void DoorsClosingTimer()
	{
		if (this.DoorsAnim[this.AnimName].speed > 0f)
		{
			base.Invoke("DoorsClosing", this.CloseDelay);
			this.isOpen = true;
			Elevator.Moving = false;
		}
	}

	private void DoorsClosing()
	{
		if (this.isOpen)
		{
			this.DoorsAnim[this.AnimName].normalizedTime = 1f;
			this.DoorsAnim[this.AnimName].speed = -this.DoorsAnimSpeed;
			this.DoorsAnim.Play();
			this.isOpen = false;
			if (this.ElevatorOpenButton != null)
			{
				this.ElevatorOpenButton.enabled = false;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == this.Player.gameObject)
		{
			this.inTrigger = true;
			if (this.isReflectionProbe && this.UpdateReflectionEveryFrame)
			{
				this.probe.refreshMode = ReflectionProbeRefreshMode.EveryFrame;
				this.probe.RenderProbe();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject == this.Player.gameObject)
		{
			this.inTrigger = false;
			if (this.isReflectionProbe && this.UpdateReflectionEveryFrame)
			{
				this.probe.refreshMode = ReflectionProbeRefreshMode.OnAwake;
				this.probe.RenderProbe();
			}
		}
	}

	private bool inTrigger;

	private Rigidbody Player;

	private Transform PlayerCam;

	[Tooltip("Type your Player's tag here.")]
	public string PlayerTag = "Player";

	private Animation DoorsAnim;

	[Tooltip("Speed multiplier of the doors opening and closing. 1 is default speed.")]
	public float DoorsAnimSpeed = 1f;

	[Tooltip("How fast the elevator 'passes' one floor. The time in seconds.")]
	public float OneFloorTime = 1.5f;

	private float OpenDelay = 1f;

	[Tooltip("How long the doors are open. The time in seconds.")]
	public float CloseDelay = 4f;

	private bool isOpen;

	private string AnimName = "ElevatorDoorsAnim_open";

	private string InputFloor = "";

	private int TargetFloor;

	[Tooltip("The floor, where this elevator is placed. Set an unique value for each elevator.")]
	public int CurrentFloor;

	private int FloorCount;

	[HideInInspector]
	public int ElevatorFloor;

	private List<GameObject> Elevators = new List<GameObject>();

	private Elevator[] ElevatorsScripts;

	private Animation TargetElvAnim;

	private TextMesh TargetElvTextInside;

	private TextMesh TargetElvTextOutside;

	public TextMesh TextOutside;

	public TextMesh TextInside;

	[Tooltip("If set to true, the Reflection Probe inside the elevator will be updated every frame, when the player near or inside the elevator. Can impact performance.")]
	public bool UpdateReflectionEveryFrame;

	private bool isReflectionProbe = true;

	private MeshRenderer ElevatorOpenButton;

	private MeshRenderer ElevatorGoBtn;

	private List<MeshRenderer> ElevatorNumericButtons = new List<MeshRenderer>();

	private AudioSource SoundFX;

	private AudioSource TargetSoundFX;

	private bool SpeedUp;

	private bool SlowDown;

	private static bool Moving;

	private bool isPlayer;

	private float PlayerHeight;

	private bool isRigidbodyCharacter;

	private ReflectionProbe probe;

	[Header("Sound Effects settings")]
	public AudioClip Bell;

	[Range(0f, 1f)]
	public float BellVolume = 1f;

	public AudioClip DoorsOpen;

	[Range(0f, 1f)]
	public float DoorsOpenVolume = 1f;

	public AudioClip DoorsClose;

	[Range(0f, 1f)]
	public float DoorsCloseVolume = 1f;

	public AudioClip ElevatorMove;

	[Range(0f, 1f)]
	public float ElevatorMoveVolume = 1f;

	public AudioClip ElevatorBtn;

	[Range(0f, 1f)]
	public float ElevatorBtnVolume = 1f;

	public AudioClip ElevatorError;

	[Range(0f, 1f)]
	public float ElevatorErrorVolume = 1f;

	private AudioSource BtnSoundFX;

	private bool ElvFound;

	private ElevatorManager _elevatorManager;

	private GameObject ElevatorsParent;
}

