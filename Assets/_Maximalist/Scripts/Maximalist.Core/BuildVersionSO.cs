using UnityEditor;
using UnityEngine;

public enum BuildType
{
	Development,
	Production
}

[CreateAssetMenu(fileName = "BuildVersion", menuName = "Build/Build Version")]
public class BuildVersionSO : ScriptableObject
{
	[Tooltip("The current version number of the game.")]

	public int majorVersion = 0;
	public int minorVersion = 0;
	public int buildVersion = 1;

	[SerializeField] private BuildType buildType = BuildType.Production;

	public BuildType BuildType 
	{ 
		get => buildType;
		set 
		{
			buildType = value;
			Debug.Log($"changing build type to [{buildType}]");
		} 
	}

	public void IterationBuildVersion()
	{
		buildVersion += 1;

#if UNITY_EDITOR

		// Construct semantic version string (Major.Minor.Build)
		string newVersion = $"{majorVersion}.{minorVersion}.{buildVersion}";
		PlayerSettings.bundleVersion = newVersion;

		// Platform-specific build numbers
		PlayerSettings.Android.bundleVersionCode = buildVersion;   // Android requires int
		PlayerSettings.iOS.buildNumber = buildVersion.ToString();  // iOS requires string

		Debug.Log($"Updated build version to {newVersion} (Android:{buildVersion}, iOS:{buildVersion})");

		EditorUtility.SetDirty(this);
		AssetDatabase.SaveAssets();

#endif


	}

}
