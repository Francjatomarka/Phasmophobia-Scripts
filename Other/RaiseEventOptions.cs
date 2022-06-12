using System;

public class RaiseEventOptions
{
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

	public static readonly RaiseEventOptions Default = new RaiseEventOptions();

	public EventCaching CachingOption;

	public byte InterestGroup;

	public int[] TargetActors;

	public ReceiverGroup Receivers;

	public byte SequenceChannel;

	public bool ForwardToWebhook;

	public bool Encrypt;
}

