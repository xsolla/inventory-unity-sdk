﻿using UnityEditor;
using UnityEngine;
using Xsolla.PayStation;
using Xsolla.Store;

namespace Xsolla.Core
{
	public partial class XsollaSettingsEditor : UnityEditor.Editor
	{
		private bool InventorySDKSettings()
		{
			var changed = false;
			using (new EditorGUILayout.VerticalScope("box"))
			{
				GUILayout.Label("Inventory SDK Settings", EditorStyles.boldLabel);
				var projectId = EditorGUILayout.TextField(new GUIContent("Project ID"),  XsollaSettings.StoreProjectId);
				if (projectId != XsollaSettings.StoreProjectId)
				{
					XsollaSettings.StoreProjectId = projectId;
					changed = true;
				}
				var webStoreUrl = EditorGUILayout.TextField(new GUIContent("Web Store URL"),  XsollaSettings.WebStoreUrl);
				if (webStoreUrl != XsollaSettings.WebStoreUrl)
				{
					XsollaSettings.WebStoreUrl = webStoreUrl;
					changed = true;
				}
			}
			EditorGUILayout.Space();
			
			return changed;
		}
	}
}

