using System;

// Token: 0x0200005C RID: 92
public enum DisconnectCause
{
	// Token: 0x040002EF RID: 751
	DisconnectByServerUserLimit = 1042,
	// Token: 0x040002F0 RID: 752
	ExceptionOnConnect = 1023,
	// Token: 0x040002F1 RID: 753
	DisconnectByServerTimeout = 1041,
	// Token: 0x040002F2 RID: 754
	DisconnectByServerLogic = 1043,
	// Token: 0x040002F3 RID: 755
	Exception = 1026,
	// Token: 0x040002F4 RID: 756
	InvalidAuthentication = 32767,
	// Token: 0x040002F5 RID: 757
	MaxCcuReached = 32757,
	// Token: 0x040002F6 RID: 758
	InvalidRegion = 32756,
	// Token: 0x040002F7 RID: 759
	SecurityExceptionOnConnect = 1022,
	// Token: 0x040002F8 RID: 760
	DisconnectByClientTimeout = 1040,
	// Token: 0x040002F9 RID: 761
	InternalReceiveException = 1039,
	// Token: 0x040002FA RID: 762
	AuthenticationTicketExpired = 32753
}
