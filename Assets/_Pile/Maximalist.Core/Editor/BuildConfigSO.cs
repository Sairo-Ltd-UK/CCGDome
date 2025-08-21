#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.Build.Profile;
using UnityEditor;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class BuildProfileConfig
{
	public string profileName;
	public BuildType buildType = BuildType.Development;
	public bool shouldBumpVersion = true;
	[Space]
	public string appID;
	public string description = "Auto-generated build";
	public string branchName = "beta";
	public string contentRoot = "content";
	public string buildOutput = "build_output";
	[Space]
	// List of depot configurations
	public List<DepotConfig> depots = new List<DepotConfig>();
}

[System.Serializable]
public class DepotConfig
{
	public string depotID;
	public string depotContentRoot = ""; // Path to content folder for this depot
	[Space]
	public List<string> fileExclusions = new List<string>(); // Files to exclude from the depot
}

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
				// Check if this profile already exists in the config
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

#endif
