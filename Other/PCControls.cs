using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

// Token: 0x02000163 RID: 355
public class PCControls : MonoBehaviour
{
	// Token: 0x06000A0A RID: 2570 RVA: 0x0003D56D File Offset: 0x0003B76D
	private void Awake()
	{
		this.view = base.GetComponent<PhotonView>();
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x0003D57B File Offset: 0x0003B77B
	private void Start()
	{
		if ((this.view.IsMine || !PhotonNetwork.InRoom) && MainManager.instance)
		{
			this.LoadControlOverrides();
		}
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x0003D5B4 File Offset: 0x0003B7B4
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

	// Token: 0x06000A0D RID: 2573 RVA: 0x0003D690 File Offset: 0x0003B890
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

	// Token: 0x06000A0E RID: 2574 RVA: 0x0003D7BC File Offset: 0x0003B9BC
	public void ResetKeybindings(InputActionAsset control)
	{
		foreach (InputActionMap actionMap in control.actionMaps)
		{
			actionMap.RemoveAllBindingOverrides();
		}
		PlayerPrefs.DeleteKey("ControlOverrides");
	}

	// Token: 0x04000A44 RID: 2628
	public InputActionAsset control;

	// Token: 0x04000A45 RID: 2629
	private PhotonView view;

	// Token: 0x020004ED RID: 1261
	[Serializable]
	private class BindingWrapperClass
	{
		// Token: 0x04002341 RID: 9025
		public List<PCControls.BindingSerializable> bindingList = new List<PCControls.BindingSerializable>();
	}

	// Token: 0x020004EE RID: 1262
	[Serializable]
	private struct BindingSerializable
	{
		// Token: 0x060025B4 RID: 9652 RVA: 0x000B55FC File Offset: 0x000B37FC
		public BindingSerializable(string bindingId, string bindingPath)
		{
			this.id = bindingId;
			this.path = bindingPath;
		}

		// Token: 0x04002342 RID: 9026
		public string id;

		// Token: 0x04002343 RID: 9027
		public string path;
	}
}
