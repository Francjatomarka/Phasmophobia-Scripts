using System;

// Token: 0x02000053 RID: 83
public class RaiseEventOptions
{
	// Token: 0x0600019F RID: 415 RVA: 0x0000B120 File Offset: 0x00009320
	public void Reset()
	{
		this.CachingOption = RaiseEventOptions.Default.CachingOption;
		this.InterestGroup = RaiseEventOptions.Default.InterestGroup;
		this.TargetActors = RaiseEventOptions.Default.TargetActors;
		this.Receivers = RaiseEventOptions.Default.Receivers;
		this.SequenceChannel = RaiseEventOptions.Default.SequenceChannel;
		this.ForwardToWebhook = RaiseEventOptions.Default.ForwardToWebhook;
		this.Encrypt = RaiseEventOptions.Default.Encrypt;
	}

	// Token: 0x040002B1 RID: 689
	public static readonly RaiseEventOptions Default = new RaiseEventOptions();

	// Token: 0x040002B2 RID: 690
	public EventCaching CachingOption;

	// Token: 0x040002B3 RID: 691
	public byte InterestGroup;

	// Token: 0x040002B4 RID: 692
	public int[] TargetActors;

	// Token: 0x040002B5 RID: 693
	public ReceiverGroup Receivers;

	// Token: 0x040002B6 RID: 694
	public byte SequenceChannel;

	// Token: 0x040002B7 RID: 695
	public bool ForwardToWebhook;

	// Token: 0x040002B8 RID: 696
	public bool Encrypt;
}
