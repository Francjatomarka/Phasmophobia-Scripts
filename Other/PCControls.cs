using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PCControls : MonoBehaviour
{
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	private void Start()
	{
		if ((this.view.IsMine || !PhotonNetwork.InRoom) && MainManager.instance)
		{
			this.LoadControlOverrides();
		}
	}

	public void StoreControlOverrides()
	{
		PCControls.BindingWrapperClass bindingWrapperClass = new PCControls.BindingWrapperClass();
		foreach (InputActionMap inputActionMap in this.control.actionMaps)
		{
			foreach (InputBinding inputBinding in inputActionMap.bindings)
			{
				if (!string.IsNullOrEmpty(inputBinding.overridePath))
				{
					bindingWrapperClass.bindingList.Add(new PCControls.BindingSerializable(inputBinding.id.ToString(), inputBinding.overridePath));
				}
			}
		}
		PlayerPrefs.SetString("ControlOverrides", JsonUtility.ToJson(bindingWrapperClass));
		PlayerPrefs.Save();
	}

	public void LoadControlOverrides()
	{
		if (PlayerPrefs.HasKey("ControlOverrides"))
		{
			PCControls.BindingWrapperClass bindingWrapperClass = JsonUtility.FromJson(PlayerPrefs.GetString("ControlOverrides"), typeof(PCControls.BindingWrapperClass)) as PCControls.BindingWrapperClass;
			Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
			foreach (PCControls.BindingSerializable bindingSerializable in bindingWrapperClass.bindingList)
			{
				dictionary.Add(new Guid(bindingSerializable.id), bindingSerializable.path);
			}
			foreach (InputActionMap inputActionMap in this.control.actionMaps)
			{
				ReadOnlyArray<InputBinding> bindings = inputActionMap.bindings;
				for (int i = 0; i < bindings.Count; i++)
				{
					string overridePath;
					if (dictionary.TryGetValue(bindings[i].id, out overridePath))
					{
						inputActionMap.ApplyBindingOverride(i, new InputBinding
						{
							overridePath = overridePath
						});
					}
				}
			}
		}
	}

	public void ResetKeybindings(InputActionAsset control)
	{
		foreach (InputActionMap actionMap in control.actionMaps)
		{
			actionMap.RemoveAllBindingOverrides();
		}
		PlayerPrefs.DeleteKey("ControlOverrides");
	}

	public InputActionAsset control;

	private PhotonView view;

	[Serializable]
	private class BindingWrapperClass
	{
		public List<PCControls.BindingSerializable> bindingList = new List<PCControls.BindingSerializable>();
	}

	[Serializable]
	private struct BindingSerializable
	{
		public BindingSerializable(string bindingId, string bindingPath)
		{
			this.id = bindingId;
			this.path = bindingPath;
		}

		public string id;

		public string path;
	}
}

