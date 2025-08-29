// ------------------------------------------------------------------------------
//  Author:      Corrin Wilson
//  Company:     Maximalist Ltd
//  Created:     1/04/2025
//
//  Copyright © 2025 Maximalist Ltd. All rights reserved.
//  This file is subject to the terms of the contract with the client.
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor.Build.Profile;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Maximalist.Builds
{
	[CreateAssetMenu(menuName = "Build/Profile Config")]
	public class BuildConfigSO : ScriptableObject
	{
		public BuildVersionSO versionAsset;
		[Space]
		public List<BuildProfileConfig> configs = new List<BuildProfileConfig>();

		[ContextMenu("Regenerate Configs from Build Profiles")]
		public void RegenerateFromBuildProfiles()
		{
			// Unity 6 Build Profiles are typically stored in this folder:
			string[] guids = AssetDatabase.FindAssets("t:BuildProfile", new[] { "Assets/Settings/Build Profiles" });

			int addedCount = 0;

			foreach (var guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				var buildProfile = AssetDatabase.LoadAssetAtPath<BuildProfile>(path);

				if (buildProfile != null)
				{
					bool alreadyExists = configs.Any(config => config.profileName == buildProfile.name);

					if (!alreadyExists)
					{
						configs.Add(new BuildProfileConfig
						{
							profileName = buildProfile.name,
							shouldBumpVersion = false,
						});

						addedCount++;
					}
				}
			}

			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
			Debug.Log($"[BuildConfigSO] Found and added {addedCount} new build profiles.");
		}
	}
}
