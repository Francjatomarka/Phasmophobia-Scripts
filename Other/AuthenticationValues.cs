using System;
using System.Collections.Generic;

public class AuthenticationValues
{
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

	// (get) Token: 0x06000323 RID: 803 RVA: 0x00013765 File Offset: 0x00011965
	// (set) Token: 0x06000324 RID: 804 RVA: 0x0001376D File Offset: 0x0001196D
	public string AuthGetParameters { get; set; }

	// (get) Token: 0x06000325 RID: 805 RVA: 0x00013776 File Offset: 0x00011976
	// (set) Token: 0x06000326 RID: 806 RVA: 0x0001377E File Offset: 0x0001197E
	public object AuthPostData { get; private set; }

	// (get) Token: 0x06000327 RID: 807 RVA: 0x00013787 File Offset: 0x00011987
	// (set) Token: 0x06000328 RID: 808 RVA: 0x0001378F File Offset: 0x0001198F
	public string Token { get; set; }

	// (get) Token: 0x06000329 RID: 809 RVA: 0x00013798 File Offset: 0x00011998
	// (set) Token: 0x0600032A RID: 810 RVA: 0x000137A0 File Offset: 0x000119A0
	public string UserId { get; set; }

	public AuthenticationValues()
	{
	}

	public AuthenticationValues(string userId)
	{
		this.UserId = userId;
	}

	public virtual void SetAuthPostData(string stringData)
	{
		this.AuthPostData = (string.IsNullOrEmpty(stringData) ? null : stringData);
	}

	public virtual void SetAuthPostData(byte[] byteData)
	{
		this.AuthPostData = byteData;
	}

	public virtual void SetAuthPostData(Dictionary<string, object> dictData)
	{
		this.AuthPostData = dictData;
	}

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

	public override string ToString()
	{
		return string.Format("AuthenticationValues UserId: {0}, GetParameters: {1} Token available: {2}", this.UserId, this.AuthGetParameters, this.Token != null);
	}

	private CustomAuthenticationType authType = CustomAuthenticationType.None;
}

