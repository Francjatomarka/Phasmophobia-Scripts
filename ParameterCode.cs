using System;

// Token: 0x0200004B RID: 75
public class ParameterCode
{
	// Token: 0x0400022F RID: 559
	public const byte SuppressRoomEvents = 237;

	// Token: 0x04000230 RID: 560
	public const byte EmptyRoomTTL = 236;

	// Token: 0x04000231 RID: 561
	public const byte PlayerTTL = 235;

	// Token: 0x04000232 RID: 562
	public const byte EventForward = 234;

	// Token: 0x04000233 RID: 563
	[Obsolete("Use: IsInactive")]
	public const byte IsComingBack = 233;

	// Token: 0x04000234 RID: 564
	public const byte IsInactive = 233;

	// Token: 0x04000235 RID: 565
	public const byte CheckUserOnJoin = 232;

	// Token: 0x04000236 RID: 566
	public const byte ExpectedValues = 231;

	// Token: 0x04000237 RID: 567
	public const byte Address = 230;

	// Token: 0x04000238 RID: 568
	public const byte PeerCount = 229;

	// Token: 0x04000239 RID: 569
	public const byte GameCount = 228;

	// Token: 0x0400023A RID: 570
	public const byte MasterPeerCount = 227;

	// Token: 0x0400023B RID: 571
	public const byte UserId = 225;

	// Token: 0x0400023C RID: 572
	public const byte ApplicationId = 224;

	// Token: 0x0400023D RID: 573
	public const byte Position = 223;

	// Token: 0x0400023E RID: 574
	public const byte MatchMakingType = 223;

	// Token: 0x0400023F RID: 575
	public const byte GameList = 222;

	// Token: 0x04000240 RID: 576
	public const byte Secret = 221;

	// Token: 0x04000241 RID: 577
	public const byte AppVersion = 220;

	// Token: 0x04000242 RID: 578
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureNodeInfo = 210;

	// Token: 0x04000243 RID: 579
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureLocalNodeId = 209;

	// Token: 0x04000244 RID: 580
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureMasterNodeId = 208;

	// Token: 0x04000245 RID: 581
	public const byte RoomName = 255;

	// Token: 0x04000246 RID: 582
	public const byte Broadcast = 250;

	// Token: 0x04000247 RID: 583
	public const byte ActorList = 252;

	// Token: 0x04000248 RID: 584
	public const byte ActorNr = 254;

	// Token: 0x04000249 RID: 585
	public const byte PlayerProperties = 249;

	// Token: 0x0400024A RID: 586
	public const byte CustomEventContent = 245;

	// Token: 0x0400024B RID: 587
	public const byte Data = 245;

	// Token: 0x0400024C RID: 588
	public const byte Code = 244;

	// Token: 0x0400024D RID: 589
	public const byte GameProperties = 248;

	// Token: 0x0400024E RID: 590
	public const byte Properties = 251;

	// Token: 0x0400024F RID: 591
	public const byte TargetActorNr = 253;

	// Token: 0x04000250 RID: 592
	public const byte ReceiverGroup = 246;

	// Token: 0x04000251 RID: 593
	public const byte Cache = 247;

	// Token: 0x04000252 RID: 594
	public const byte CleanupCacheOnLeave = 241;

	// Token: 0x04000253 RID: 595
	public const byte Group = 240;

	// Token: 0x04000254 RID: 596
	public const byte Remove = 239;

	// Token: 0x04000255 RID: 597
	public const byte PublishUserId = 239;

	// Token: 0x04000256 RID: 598
	public const byte Add = 238;

	// Token: 0x04000257 RID: 599
	public const byte Info = 218;

	// Token: 0x04000258 RID: 600
	public const byte ClientAuthenticationType = 217;

	// Token: 0x04000259 RID: 601
	public const byte ClientAuthenticationParams = 216;

	// Token: 0x0400025A RID: 602
	public const byte JoinMode = 215;

	// Token: 0x0400025B RID: 603
	public const byte ClientAuthenticationData = 214;

	// Token: 0x0400025C RID: 604
	public const byte MasterClientId = 203;

	// Token: 0x0400025D RID: 605
	public const byte FindFriendsRequestList = 1;

	// Token: 0x0400025E RID: 606
	public const byte FindFriendsResponseOnlineList = 1;

	// Token: 0x0400025F RID: 607
	public const byte FindFriendsResponseRoomIdList = 2;

	// Token: 0x04000260 RID: 608
	public const byte LobbyName = 213;

	// Token: 0x04000261 RID: 609
	public const byte LobbyType = 212;

	// Token: 0x04000262 RID: 610
	public const byte LobbyStats = 211;

	// Token: 0x04000263 RID: 611
	public const byte Region = 210;

	// Token: 0x04000264 RID: 612
	public const byte UriPath = 209;

	// Token: 0x04000265 RID: 613
	public const byte WebRpcParameters = 208;

	// Token: 0x04000266 RID: 614
	public const byte WebRpcReturnCode = 207;

	// Token: 0x04000267 RID: 615
	public const byte WebRpcReturnMessage = 206;

	// Token: 0x04000268 RID: 616
	public const byte CacheSliceIndex = 205;

	// Token: 0x04000269 RID: 617
	public const byte Plugins = 204;

	// Token: 0x0400026A RID: 618
	public const byte NickName = 202;

	// Token: 0x0400026B RID: 619
	public const byte PluginName = 201;

	// Token: 0x0400026C RID: 620
	public const byte PluginVersion = 200;

	// Token: 0x0400026D RID: 621
	public const byte ExpectedProtocol = 195;

	// Token: 0x0400026E RID: 622
	public const byte CustomInitData = 194;

	// Token: 0x0400026F RID: 623
	public const byte EncryptionMode = 193;

	// Token: 0x04000270 RID: 624
	public const byte EncryptionData = 192;

	// Token: 0x04000271 RID: 625
	public const byte RoomOptionFlags = 191;
}
