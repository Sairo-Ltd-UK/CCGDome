using UnityEditor.Build.Profile;
using UnityEditor.Callbacks;
using UnityEditor;
using Debug = UnityEngine.Debug;

public class MyBuildPostprocessor
{
	[PostProcessBuild(1)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{
		BuildProfile _buildProfile = BuildProfile.GetActiveBuildProfile();
		BuildConfigSO _configSO = LoadConfig();
		BuildProfileConfig _config = _configSO.configs.Find(c => c.profileName == _buildProfile.name);

		if (_config == null || _configSO == null)
		{
			Debug.LogWarning($"No config found for build profile {_buildProfile.name}");
			return;
		}

		_configSO.versionAsset.IterationBuildVersion();
	}


	private static BuildConfigSO LoadConfig()
	{
		return AssetDatabase.LoadAssetAtPath<BuildConfigSO>("Assets/_Maximalist/Settings/BuildConfigSO.asset");
	}
}
