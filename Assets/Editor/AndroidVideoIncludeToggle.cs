#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

public class AndroidVideoIncludeToggle :
    IPreprocessBuildWithReport,
    IPostprocessBuildWithReport,
    IActiveBuildTargetChanged
{
    public int callbackOrder => 0;
    private const string TargetGroupName = "AndroidVideo";

    private bool originalInclude;
    private BundledAssetGroupSchema schema;

    // Called when you start a build
    public void OnPreprocessBuild(BuildReport report)
    {
        SetIncludeForPlatform(report.summary.platform, rememberOriginal: true);
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        if (schema != null)
        {
            schema.IncludeInBuild = originalInclude;
            EditorUtility.SetDirty(schema);
            AssetDatabase.SaveAssets();
        }
    }

    // Called when you switch active build target in Build Settings
    public int callbackOrderForTargetChanged => 0;
    public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
    {
        SetIncludeForPlatform(newTarget, rememberOriginal: false);
    }

    private void SetIncludeForPlatform(BuildTarget target, bool rememberOriginal)
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (!settings) return;

        var group = settings.FindGroup(TargetGroupName);
        if (!group) return;

        schema = group.GetSchema<BundledAssetGroupSchema>();
        if (!schema) return;

        if (rememberOriginal)
            originalInclude = schema.IncludeInBuild;

        schema.IncludeInBuild = (target == BuildTarget.Android);
        EditorUtility.SetDirty(schema);
        AssetDatabase.SaveAssets();

        Debug.Log($"[AndroidVideoIncludeToggle] Group '{TargetGroupName}' IncludeInBuild set to {schema.IncludeInBuild} for {target}");
    }
}
#endif
