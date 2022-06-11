using System;
using System.Collections.Generic;

// Token: 0x0200009A RID: 154
public class AuthenticationValues
{
	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000321 RID: 801 RVA: 0x00013754 File Offset: 0x00011954
	// (set) Token: 0x06000322 RID: 802 RVA: 0x0001375C File Offset: 0x0001195C
	public CustomAuthenticationType AuthType
	{
		get
		{
			return this.authType;
		}
		set
		{
			this.authType = value;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000323 RID: 803 RVA: 0x00013765 File Offset: 0x00011965
	// (set) Token: 0x06000324 RID: 804 RVA: 0x0001376D File Offset: 0x0001196D
	public string AuthGetParameters { get; set; }

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000325 RID: 805 RVA: 0x00013776 File Offset: 0x00011976
	// (set) Token: 0x06000326 RID: 806 RVA: 0x0001377E File Offset: 0x0001197E
	public object AuthPostData { get; private set; }

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000327 RID: 807 RVA: 0x00013787 File Offset: 0x00011987
	// (set) Token: 0x06000328 RID: 808 RVA: 0x0001378F File Offset: 0x0001198F
	public string Token { get; set; }

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000329 RID: 809 RVA: 0x00013798 File Offset: 0x00011998
	// (set) Token: 0x0600032A RID: 810 RVA: 0x000137A0 File Offset: 0x000119A0
	public string UserId { get; set; }

	// Token: 0x0600032B RID: 811 RVA: 0x000137A9 File Offset: 0x000119A9
	public AuthenticationValues()
	{
	}

	// Token: 0x0600032C RID: 812 RVA: 0x000137BC File Offset: 0x000119BC
	public AuthenticationValues(string userId)
	{
		this.UserId = userId;
	}

	// Token: 0x0600032D RID: 813 RVA: 0x000137D6 File Offset: 0x000119D6
	public virtual void SetAuthPostData(string stringData)
	{
		this.AuthPostData = (string.IsNullOrEmpty(stringData) ? null : stringData);
	}

	// Token: 0x0600032E RID: 814 RVA: 0x000137EA File Offset: 0x000119EA
	public virtual void SetAuthPostData(byte[] byteData)
	{
		this.AuthPostData = byteData;
	}

	// Token: 0x0600032F RID: 815 RVA: 0x000137EA File Offset: 0x000119EA
	public virtual void SetAuthPostData(Dictionary<string, object> dictData)
	{
		this.AuthPostData = dictData;
	}

	// Token: 0x06000330 RID: 816 RVA: 0x000137F4 File Offset: 0x000119F4
	public virtual void AddAuthParameter(string key, string value)
	{
		string text = string.IsNullOrEmpty(this.AuthGetParameters) ? "" : "&";
		this.AuthGetParameters = string.Format("{0}{1}{2}={3}", new object[]
		{
			this.AuthGetParameters,
			text,
			Uri.EscapeDataString(key),
			Uri.EscapeDataString(value)
		});
	}

	// Token: 0x06000331 RID: 817 RVA: 0x00013850 File Offset: 0x00011A50
	public override string ToString()
	{
		return string.Format("AuthenticationValues UserId: {0}, GetParameters: {1} Token available: {2}", this.UserId, this.AuthGetParameters, this.Token != null);
	}

	// Token: 0x04000442 RID: 1090
	private CustomAuthenticationType authType = CustomAuthenticationType.None;
}
