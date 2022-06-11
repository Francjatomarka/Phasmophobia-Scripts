using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class Serialisation
{
	// Token: 0x06000139 RID: 313 RVA: 0x00009854 File Offset: 0x00007A54
	public static byte[] SerialiseStruct<T>(T structToSerialise)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xmlSerializer.Serialize(xmlTextWriter, structToSerialise);
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
		if (Serialisation.isVerbose)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Serialising ",
				typeof(T),
				" to a byte array,  ",
				memoryStream.ToArray().Length,
				" bytes"
			}));
		}
		return memoryStream.ToArray();
	}

	// Token: 0x0600013A RID: 314 RVA: 0x000098EC File Offset: 0x00007AEC
	public static string SerialiseStructToString<T>(T structToSerialise)
	{
		return new UTF8Encoding().GetString(Serialisation.SerialiseStruct<T>(structToSerialise));
	}

	// Token: 0x0600013B RID: 315 RVA: 0x00009900 File Offset: 0x00007B00
	public static T DeserialiseStruct<T>(byte[] xmlString)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		MemoryStream stream = new MemoryStream(xmlString);
		if (Serialisation.isVerbose)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Deserialising object of type ",
				typeof(T),
				". Given array is ",
				xmlString.Length,
				" bytes long."
			}));
		}
		T result;
		try
		{
			result = (T)((object)xmlSerializer.Deserialize(stream));
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			result = default(T);
		}
		return result;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x000099A0 File Offset: 0x00007BA0
	public static T DeserialiseStruct<T>(string xmlString)
	{
		if (Serialisation.isVerbose)
		{
			Debug.Log("Deserialising using the string method, now passing control to the byte converter.");
		}
		return Serialisation.DeserialiseStruct<T>(new UTF8Encoding().GetBytes(xmlString));
	}

	// Token: 0x0400018C RID: 396
	public static bool isVerbose;
}
