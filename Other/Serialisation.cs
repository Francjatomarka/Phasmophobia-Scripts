using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Serialisation
{
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

	public static string SerialiseStructToString<T>(T structToSerialise)
	{
		return new UTF8Encoding().GetString(Serialisation.SerialiseStruct<T>(structToSerialise));
	}

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

	public static T DeserialiseStruct<T>(string xmlString)
	{
		if (Serialisation.isVerbose)
		{
			Debug.Log("Deserialising using the string method, now passing control to the byte converter.");
		}
		return Serialisation.DeserialiseStruct<T>(new UTF8Encoding().GetBytes(xmlString));
	}

	public static bool isVerbose;
}

