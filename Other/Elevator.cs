using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Token: 0x02000023 RID: 35
public class Elevator : MonoBehaviour
{
	// Token: 0x060000E3 RID: 227 RVA: 0x000071D0 File Offset: 0x000053D0
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

	// Token: 0x060000E4 RID: 228 RVA: 0x0000755C File Offset: 0x0000575C
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

	// Token: 0x060000E5 RID: 229 RVA: 0x000075BC File Offset: 0x000057BC
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

	// Token: 0x060000E6 RID: 230 RVA: 0x00007B58 File Offset: 0x00005D58
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

	// Token: 0x060000E7 RID: 231 RVA: 0x00007BC7 File Offset: 0x00005DC7
	private void SlowDownStart()
	{
		this.SlowDown = true;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00007BD0 File Offset: 0x00005DD0
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

	// Token: 0x060000E9 RID: 233 RVA: 0x00007BDF File Offset: 0x00005DDF
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

	// Token: 0x060000EA RID: 234 RVA: 0x00007BF0 File Offset: 0x00005DF0
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

	// Token: 0x060000EB RID: 235 RVA: 0x00007C64 File Offset: 0x00005E64
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

	// Token: 0x060000EC RID: 236 RVA: 0x00007CCC File Offset: 0x00005ECC
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

	// Token: 0x060000ED RID: 237 RVA: 0x00007DA4 File Offset: 0x00005FA4
	private void BellSoundPlay()
	{
		this.SoundFX.clip = this.Bell;
		this.SoundFX.loop = false;
		this.SoundFX.volume = this.BellVolume;
		this.SoundFX.pitch = 1f;
		this.SoundFX.Play();
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00007DFC File Offset: 0x00005FFC
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

	// Token: 0x060000EF RID: 239 RVA: 0x00007E98 File Offset: 0x00006098
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

	// Token: 0x060000F0 RID: 240 RVA: 0x00007F04 File Offset: 0x00006104
	private void TargetElvOpening()
	{
		this.TextsUpdate();
		this.TargetElvAnim[this.AnimName].normalizedTime = 0f;
		this.TargetElvAnim[this.AnimName].speed = this.DoorsAnimSpeed;
		this.TargetElvAnim.Play();
		this.ButtonsReset();
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00007F60 File Offset: 0x00006160
	private void DoorsOpening()
	{
		this.TargetFloor = 0;
		this.TextsUpdate();
		this.DoorsAnim[this.AnimName].normalizedTime = 0f;
		this.DoorsAnim[this.AnimName].speed = this.DoorsAnimSpeed;
		this.DoorsAnim.Play();
		this.ButtonsReset();
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00007FC3 File Offset: 0x000061C3
	private void DoorsClosingTimer()
	{
		if (this.DoorsAnim[this.AnimName].speed > 0f)
		{
			base.Invoke("DoorsClosing", this.CloseDelay);
			this.isOpen = true;
			Elevator.Moving = false;
		}
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00008000 File Offset: 0x00006200
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

	// Token: 0x060000F4 RID: 244 RVA: 0x0000807C File Offset: 0x0000627C
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

	// Token: 0x060000F5 RID: 245 RVA: 0x000080D0 File Offset: 0x000062D0
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

	// Token: 0x040000D9 RID: 217
	private bool inTrigger;

	// Token: 0x040000DA RID: 218
	private Rigidbody Player;

	// Token: 0x040000DB RID: 219
	private Transform PlayerCam;

	// Token: 0x040000DC RID: 220
	[Tooltip("Type your Player's tag here.")]
	public string PlayerTag = "Player";

	// Token: 0x040000DD RID: 221
	private Animation DoorsAnim;

	// Token: 0x040000DE RID: 222
	[Tooltip("Speed multiplier of the doors opening and closing. 1 is default speed.")]
	public float DoorsAnimSpeed = 1f;

	// Token: 0x040000DF RID: 223
	[Tooltip("How fast the elevator 'passes' one floor. The time in seconds.")]
	public float OneFloorTime = 1.5f;

	// Token: 0x040000E0 RID: 224
	private float OpenDelay = 1f;

	// Token: 0x040000E1 RID: 225
	[Tooltip("How long the doors are open. The time in seconds.")]
	public float CloseDelay = 4f;

	// Token: 0x040000E2 RID: 226
	private bool isOpen;

	// Token: 0x040000E3 RID: 227
	private string AnimName = "ElevatorDoorsAnim_open";

	// Token: 0x040000E4 RID: 228
	private string InputFloor = "";

	// Token: 0x040000E5 RID: 229
	private int TargetFloor;

	// Token: 0x040000E6 RID: 230
	[Tooltip("The floor, where this elevator is placed. Set an unique value for each elevator.")]
	public int CurrentFloor;

	// Token: 0x040000E7 RID: 231
	private int FloorCount;

	// Token: 0x040000E8 RID: 232
	[HideInInspector]
	public int ElevatorFloor;

	// Token: 0x040000E9 RID: 233
	private List<GameObject> Elevators = new List<GameObject>();

	// Token: 0x040000EA RID: 234
	private Elevator[] ElevatorsScripts;

	// Token: 0x040000EB RID: 235
	private Animation TargetElvAnim;

	// Token: 0x040000EC RID: 236
	private TextMesh TargetElvTextInside;

	// Token: 0x040000ED RID: 237
	private TextMesh TargetElvTextOutside;

	// Token: 0x040000EE RID: 238
	public TextMesh TextOutside;

	// Token: 0x040000EF RID: 239
	public TextMesh TextInside;

	// Token: 0x040000F0 RID: 240
	[Tooltip("If set to true, the Reflection Probe inside the elevator will be updated every frame, when the player near or inside the elevator. Can impact performance.")]
	public bool UpdateReflectionEveryFrame;

	// Token: 0x040000F1 RID: 241
	private bool isReflectionProbe = true;

	// Token: 0x040000F2 RID: 242
	private MeshRenderer ElevatorOpenButton;

	// Token: 0x040000F3 RID: 243
	private MeshRenderer ElevatorGoBtn;

	// Token: 0x040000F4 RID: 244
	private List<MeshRenderer> ElevatorNumericButtons = new List<MeshRenderer>();

	// Token: 0x040000F5 RID: 245
	private AudioSource SoundFX;

	// Token: 0x040000F6 RID: 246
	private AudioSource TargetSoundFX;

	// Token: 0x040000F7 RID: 247
	private bool SpeedUp;

	// Token: 0x040000F8 RID: 248
	private bool SlowDown;

	// Token: 0x040000F9 RID: 249
	private static bool Moving;

	// Token: 0x040000FA RID: 250
	private bool isPlayer;

	// Token: 0x040000FB RID: 251
	private float PlayerHeight;

	// Token: 0x040000FC RID: 252
	private bool isRigidbodyCharacter;

	// Token: 0x040000FD RID: 253
	private ReflectionProbe probe;

	// Token: 0x040000FE RID: 254
	[Header("Sound Effects settings")]
	public AudioClip Bell;

	// Token: 0x040000FF RID: 255
	[Range(0f, 1f)]
	public float BellVolume = 1f;

	// Token: 0x04000100 RID: 256
	public AudioClip DoorsOpen;

	// Token: 0x04000101 RID: 257
	[Range(0f, 1f)]
	public float DoorsOpenVolume = 1f;

	// Token: 0x04000102 RID: 258
	public AudioClip DoorsClose;

	// Token: 0x04000103 RID: 259
	[Range(0f, 1f)]
	public float DoorsCloseVolume = 1f;

	// Token: 0x04000104 RID: 260
	public AudioClip ElevatorMove;

	// Token: 0x04000105 RID: 261
	[Range(0f, 1f)]
	public float ElevatorMoveVolume = 1f;

	// Token: 0x04000106 RID: 262
	public AudioClip ElevatorBtn;

	// Token: 0x04000107 RID: 263
	[Range(0f, 1f)]
	public float ElevatorBtnVolume = 1f;

	// Token: 0x04000108 RID: 264
	public AudioClip ElevatorError;

	// Token: 0x04000109 RID: 265
	[Range(0f, 1f)]
	public float ElevatorErrorVolume = 1f;

	// Token: 0x0400010A RID: 266
	private AudioSource BtnSoundFX;

	// Token: 0x0400010B RID: 267
	private bool ElvFound;

	// Token: 0x0400010C RID: 268
	private ElevatorManager _elevatorManager;

	// Token: 0x0400010D RID: 269
	private GameObject ElevatorsParent;
}
