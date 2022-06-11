using System;
using UnityEngine;
using Photon.Pun;

// Token: 0x0200005C RID: 92
[RequireComponent(typeof(CharacterController))]
public class PickupController : MonoBehaviour, IPunObservable
{
	// Token: 0x060001EA RID: 490 RVA: 0x0000D64C File Offset: 0x0000B84C
	private void Awake()
	{
		PhotonView component = base.gameObject.GetComponent<PhotonView>();
		if (component != null)
		{
			this.isControllable = component.IsMine;
			if (this.AssignAsTagObject)
			{
				component.Owner.TagObject = base.gameObject;
			}
		}
		this.moveDirection = base.transform.TransformDirection(Vector3.forward);
		this._animation = base.GetComponent<Animation>();
		if (!this._animation)
		{
			Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");
		}
		if (!this.idleAnimation)
		{
			this._animation = null;
			Debug.Log("No idle animation found. Turning off animations.");
		}
		if (!this.walkAnimation)
		{
			this._animation = null;
			Debug.Log("No walk animation found. Turning off animations.");
		}
		if (!this.runAnimation)
		{
			this._animation = null;
			Debug.Log("No run animation found. Turning off animations.");
		}
		if (!this.jumpPoseAnimation && this.canJump)
		{
			this._animation = null;
			Debug.Log("No jump animation found and the character has canJump enabled. Turning off animations.");
		}
	}

	// Token: 0x060001EB RID: 491 RVA: 0x0000D790 File Offset: 0x0000B990
	private void Update()
	{
		if (this.isControllable)
		{
			if (Input.GetButtonDown("Jump"))
			{
				this.lastJumpButtonTime = Time.time;
			}
			this.UpdateSmoothedMovementDirection();
			this.ApplyGravity();
			this.ApplyJumping();
			Vector3 vector = this.moveDirection * this.moveSpeed + new Vector3(0f, this.verticalSpeed, 0f) + this.inAirVelocity;
			vector *= Time.deltaTime;
			CharacterController component = base.GetComponent<CharacterController>();
			this.collisionFlags = component.Move(vector);
		}
		if (this.remotePosition != Vector3.zero)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.remotePosition, Time.deltaTime * this.RemoteSmoothing);
		}
		this.velocity = (base.transform.position - this.lastPos) * 25f;
		if (this._animation)
		{
			if (this._characterState == PickupCharacterState.Jumping)
			{
				if (!this.jumpingReachedApex)
				{
					this._animation[this.jumpPoseAnimation.name].speed = this.jumpAnimationSpeed;
					this._animation[this.jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
					this._animation.CrossFade(this.jumpPoseAnimation.name);
				}
				else
				{
					this._animation[this.jumpPoseAnimation.name].speed = -this.landAnimationSpeed;
					this._animation[this.jumpPoseAnimation.name].wrapMode = WrapMode.ClampForever;
					this._animation.CrossFade(this.jumpPoseAnimation.name);
				}
			}
			else
			{
				if (this._characterState == PickupCharacterState.Idle)
				{
					this._animation.CrossFade(this.idleAnimation.name);
				}
				else if (this._characterState == PickupCharacterState.Running)
				{
					this._animation[this.runAnimation.name].speed = this.runMaxAnimationSpeed;
					if (this.isControllable)
					{
						this._animation[this.runAnimation.name].speed = Mathf.Clamp(this.velocity.magnitude, 0f, this.runMaxAnimationSpeed);
					}
					this._animation.CrossFade(this.runAnimation.name);
				}
				else if (this._characterState == PickupCharacterState.Trotting)
				{
					this._animation[this.walkAnimation.name].speed = this.trotMaxAnimationSpeed;
					if (this.isControllable)
					{
						this._animation[this.walkAnimation.name].speed = Mathf.Clamp(this.velocity.magnitude, 0f, this.trotMaxAnimationSpeed);
					}
					this._animation.CrossFade(this.walkAnimation.name);
				}
				else if (this._characterState == PickupCharacterState.Walking)
				{
					this._animation[this.walkAnimation.name].speed = this.walkMaxAnimationSpeed;
					if (this.isControllable)
					{
						this._animation[this.walkAnimation.name].speed = Mathf.Clamp(this.velocity.magnitude, 0f, this.walkMaxAnimationSpeed);
					}
					this._animation.CrossFade(this.walkAnimation.name);
				}
				if (this._characterState != PickupCharacterState.Running)
				{
					this._animation[this.runAnimation.name].time = 0f;
				}
			}
		}
		if (this.IsGrounded() && this.DoRotate)
		{
			base.transform.rotation = Quaternion.LookRotation(this.moveDirection);
		}
		if (this.IsGrounded())
		{
			this.lastGroundedTime = Time.time;
			this.inAirVelocity = Vector3.zero;
			if (this.jumping)
			{
				this.jumping = false;
				base.SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
			}
		}
		this.lastPos = base.transform.position;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0000DBA0 File Offset: 0x0000BDA0
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext((byte)this._characterState);
			return;
		}
		bool flag = this.remotePosition == Vector3.zero;
		this.remotePosition = (Vector3)stream.ReceiveNext();
		this._characterState = (PickupCharacterState)((byte)stream.ReceiveNext());
		if (flag)
		{
			base.transform.position = this.remotePosition;
		}
	}

	// Token: 0x060001ED RID: 493 RVA: 0x0000DC24 File Offset: 0x0000BE24
	private void UpdateSmoothedMovementDirection()
	{
		Transform transform = Camera.main.transform;
		bool flag = this.IsGrounded();
		Vector3 vector = transform.TransformDirection(Vector3.forward);
		vector.y = 0f;
		vector = vector.normalized;
		Vector3 a = new Vector3(vector.z, 0f, -vector.x);
		float axisRaw = Input.GetAxisRaw("Vertical");
		float axisRaw2 = Input.GetAxisRaw("Horizontal");
		if (axisRaw < -0.2f)
		{
			this.movingBack = true;
		}
		else
		{
			this.movingBack = false;
		}
		bool flag2 = this.isMoving;
		this.isMoving = (Mathf.Abs(axisRaw2) > 0.1f || Mathf.Abs(axisRaw) > 0.1f);
		Vector3 vector2 = axisRaw2 * a + axisRaw * vector;
		if (flag)
		{
			this.lockCameraTimer += Time.deltaTime;
			if (this.isMoving != flag2)
			{
				this.lockCameraTimer = 0f;
			}
			if (vector2 != Vector3.zero)
			{
				if (this.moveSpeed < this.walkSpeed * 0.9f && flag)
				{
					this.moveDirection = vector2.normalized;
				}
				else
				{
					this.moveDirection = Vector3.RotateTowards(this.moveDirection, vector2, this.rotateSpeed * 0.017453292f * Time.deltaTime, 1000f);
					this.moveDirection = this.moveDirection.normalized;
				}
			}
			float t = this.speedSmoothing * Time.deltaTime;
			float num = Mathf.Min(vector2.magnitude, 1f);
			this._characterState = PickupCharacterState.Idle;
			if ((Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift)) && this.isMoving)
			{
				num *= this.runSpeed;
				this._characterState = PickupCharacterState.Running;
			}
			else if (Time.time - this.trotAfterSeconds > this.walkTimeStart)
			{
				num *= this.trotSpeed;
				this._characterState = PickupCharacterState.Trotting;
			}
			else if (this.isMoving)
			{
				num *= this.walkSpeed;
				this._characterState = PickupCharacterState.Walking;
			}
			this.moveSpeed = Mathf.Lerp(this.moveSpeed, num, t);
			if (this.moveSpeed < this.walkSpeed * 0.3f)
			{
				this.walkTimeStart = Time.time;
				return;
			}
		}
		else
		{
			if (this.jumping)
			{
				this.lockCameraTimer = 0f;
			}
			if (this.isMoving)
			{
				this.inAirVelocity += vector2.normalized * Time.deltaTime * this.inAirControlAcceleration;
			}
		}
	}

	// Token: 0x060001EE RID: 494 RVA: 0x0000DEA4 File Offset: 0x0000C0A4
	private void ApplyJumping()
	{
		if (this.lastJumpTime + this.jumpRepeatTime > Time.time)
		{
			return;
		}
		if (this.IsGrounded() && this.canJump && Time.time < this.lastJumpButtonTime + this.jumpTimeout)
		{
			this.verticalSpeed = this.CalculateJumpVerticalSpeed(this.jumpHeight);
			base.SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x060001EF RID: 495 RVA: 0x0000DF08 File Offset: 0x0000C108
	private void ApplyGravity()
	{
		if (this.isControllable)
		{
			if (this.jumping && !this.jumpingReachedApex && this.verticalSpeed <= 0f)
			{
				this.jumpingReachedApex = true;
				base.SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
			}
			if (this.IsGrounded())
			{
				this.verticalSpeed = 0f;
				return;
			}
			this.verticalSpeed -= this.gravity * Time.deltaTime;
		}
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x0000DF7A File Offset: 0x0000C17A
	private float CalculateJumpVerticalSpeed(float targetJumpHeight)
	{
		return Mathf.Sqrt(2f * targetJumpHeight * this.gravity);
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000DF8F File Offset: 0x0000C18F
	private void DidJump()
	{
		this.jumping = true;
		this.jumpingReachedApex = false;
		this.lastJumpTime = Time.time;
		this.lastJumpButtonTime = -10f;
		this._characterState = PickupCharacterState.Jumping;
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0000DFBC File Offset: 0x0000C1BC
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		float y = hit.moveDirection.y;
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000DFD0 File Offset: 0x0000C1D0
	public float GetSpeed()
	{
		return this.moveSpeed;
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000DFD8 File Offset: 0x0000C1D8
	public bool IsJumping()
	{
		return this.jumping;
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0000DFE0 File Offset: 0x0000C1E0
	public bool IsGrounded()
	{
		return (this.collisionFlags & CollisionFlags.Below) > CollisionFlags.None;
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0000DFED File Offset: 0x0000C1ED
	public Vector3 GetDirection()
	{
		return this.moveDirection;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000DFF5 File Offset: 0x0000C1F5
	public bool IsMovingBackwards()
	{
		return this.movingBack;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000DFFD File Offset: 0x0000C1FD
	public float GetLockCameraTimer()
	{
		return this.lockCameraTimer;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000E005 File Offset: 0x0000C205
	public bool IsMoving()
	{
		return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0000E02D File Offset: 0x0000C22D
	public bool HasJumpReachedApex()
	{
		return this.jumpingReachedApex;
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0000E035 File Offset: 0x0000C235
	public bool IsGroundedWithTimeout()
	{
		return this.lastGroundedTime + this.groundedTimeout > Time.time;
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0000E04B File Offset: 0x0000C24B
	public void Reset()
	{
		base.gameObject.tag = "Player";
	}

	// Token: 0x04000218 RID: 536
	public AnimationClip idleAnimation;

	// Token: 0x04000219 RID: 537
	public AnimationClip walkAnimation;

	// Token: 0x0400021A RID: 538
	public AnimationClip runAnimation;

	// Token: 0x0400021B RID: 539
	public AnimationClip jumpPoseAnimation;

	// Token: 0x0400021C RID: 540
	public float walkMaxAnimationSpeed = 0.75f;

	// Token: 0x0400021D RID: 541
	public float trotMaxAnimationSpeed = 1f;

	// Token: 0x0400021E RID: 542
	public float runMaxAnimationSpeed = 1f;

	// Token: 0x0400021F RID: 543
	public float jumpAnimationSpeed = 1.15f;

	// Token: 0x04000220 RID: 544
	public float landAnimationSpeed = 1f;

	// Token: 0x04000221 RID: 545
	private Animation _animation;

	// Token: 0x04000222 RID: 546
	public PickupCharacterState _characterState;

	// Token: 0x04000223 RID: 547
	public float walkSpeed = 2f;

	// Token: 0x04000224 RID: 548
	public float trotSpeed = 4f;

	// Token: 0x04000225 RID: 549
	public float runSpeed = 6f;

	// Token: 0x04000226 RID: 550
	public float inAirControlAcceleration = 3f;

	// Token: 0x04000227 RID: 551
	public float jumpHeight = 0.5f;

	// Token: 0x04000228 RID: 552
	public float gravity = 20f;

	// Token: 0x04000229 RID: 553
	public float speedSmoothing = 10f;

	// Token: 0x0400022A RID: 554
	public float rotateSpeed = 500f;

	// Token: 0x0400022B RID: 555
	public float trotAfterSeconds = 3f;

	// Token: 0x0400022C RID: 556
	public bool canJump;

	// Token: 0x0400022D RID: 557
	private float jumpRepeatTime = 0.05f;

	// Token: 0x0400022E RID: 558
	private float jumpTimeout = 0.15f;

	// Token: 0x0400022F RID: 559
	private float groundedTimeout = 0.25f;

	// Token: 0x04000230 RID: 560
	private float lockCameraTimer;

	// Token: 0x04000231 RID: 561
	private Vector3 moveDirection = Vector3.zero;

	// Token: 0x04000232 RID: 562
	private float verticalSpeed;

	// Token: 0x04000233 RID: 563
	private float moveSpeed;

	// Token: 0x04000234 RID: 564
	private CollisionFlags collisionFlags;

	// Token: 0x04000235 RID: 565
	private bool jumping;

	// Token: 0x04000236 RID: 566
	private bool jumpingReachedApex;

	// Token: 0x04000237 RID: 567
	private bool movingBack;

	// Token: 0x04000238 RID: 568
	private bool isMoving;

	// Token: 0x04000239 RID: 569
	private float walkTimeStart;

	// Token: 0x0400023A RID: 570
	private float lastJumpButtonTime = -10f;

	// Token: 0x0400023B RID: 571
	private float lastJumpTime = -1f;

	// Token: 0x0400023C RID: 572
	private Vector3 inAirVelocity = Vector3.zero;

	// Token: 0x0400023D RID: 573
	private float lastGroundedTime;

	// Token: 0x0400023E RID: 574
	private Vector3 velocity = Vector3.zero;

	// Token: 0x0400023F RID: 575
	private Vector3 lastPos;

	// Token: 0x04000240 RID: 576
	private Vector3 remotePosition;

	// Token: 0x04000241 RID: 577
	public bool isControllable;

	// Token: 0x04000242 RID: 578
	public bool DoRotate = true;

	// Token: 0x04000243 RID: 579
	public float RemoteSmoothing = 5f;

	// Token: 0x04000244 RID: 580
	public bool AssignAsTagObject = true;
}
