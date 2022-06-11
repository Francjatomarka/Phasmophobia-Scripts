using System;

// Token: 0x02000047 RID: 71
public class ErrorCode
{
	// Token: 0x040001F6 RID: 502
	public const int Ok = 0;

	// Token: 0x040001F7 RID: 503
	public const int OperationNotAllowedInCurrentState = -3;

	// Token: 0x040001F8 RID: 504
	[Obsolete("Use InvalidOperation.")]
	public const int InvalidOperationCode = -2;

	// Token: 0x040001F9 RID: 505
	public const int InvalidOperation = -2;

	// Token: 0x040001FA RID: 506
	public const int InternalServerError = -1;

	// Token: 0x040001FB RID: 507
	public const int InvalidAuthentication = 32767;

	// Token: 0x040001FC RID: 508
	public const int GameIdAlreadyExists = 32766;

	// Token: 0x040001FD RID: 509
	public const int GameFull = 32765;

	// Token: 0x040001FE RID: 510
	public const int GameClosed = 32764;

	// Token: 0x040001FF RID: 511
	[Obsolete("No longer used, cause random matchmaking is no longer a process.")]
	public const int AlreadyMatched = 32763;

	// Token: 0x04000200 RID: 512
	public const int ServerFull = 32762;

	// Token: 0x04000201 RID: 513
	public const int UserBlocked = 32761;

	// Token: 0x04000202 RID: 514
	public const int NoRandomMatchFound = 32760;

	// Token: 0x04000203 RID: 515
	public const int GameDoesNotExist = 32758;

	// Token: 0x04000204 RID: 516
	public const int MaxCcuReached = 32757;

	// Token: 0x04000205 RID: 517
	public const int InvalidRegion = 32756;

	// Token: 0x04000206 RID: 518
	public const int CustomAuthenticationFailed = 32755;

	// Token: 0x04000207 RID: 519
	public const int AuthenticationTicketExpired = 32753;

	// Token: 0x04000208 RID: 520
	public const int PluginReportedError = 32752;

	// Token: 0x04000209 RID: 521
	public const int PluginMismatch = 32751;

	// Token: 0x0400020A RID: 522
	public const int JoinFailedPeerAlreadyJoined = 32750;

	// Token: 0x0400020B RID: 523
	public const int JoinFailedFoundInactiveJoiner = 32749;

	// Token: 0x0400020C RID: 524
	public const int JoinFailedWithRejoinerNotFound = 32748;

	// Token: 0x0400020D RID: 525
	public const int JoinFailedFoundExcludedUserId = 32747;

	// Token: 0x0400020E RID: 526
	public const int JoinFailedFoundActiveJoiner = 32746;

	// Token: 0x0400020F RID: 527
	public const int HttpLimitReached = 32745;

	// Token: 0x04000210 RID: 528
	public const int ExternalHttpCallFailed = 32744;

	// Token: 0x04000211 RID: 529
	public const int SlotError = 32742;

	// Token: 0x04000212 RID: 530
	public const int InvalidEncryptionParameters = 32741;
}
