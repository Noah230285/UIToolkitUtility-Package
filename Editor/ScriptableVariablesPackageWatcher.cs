using UnityEngine;
using UnityEditor;
using System.IO;

public class ScriptableVariablePackageWatcher : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
                                       string[] movedAssets, string[] movedFromPath)
    {
        const string define = "SCRIPTABLE_VARIABLES_ENABLED";
        // Get defines.
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

        foreach (string asset in importedAssets)
        {
            if (!asset.EndsWith("com.utility_essentials.scriptable_variables"))
            {
                continue;
            }
            Debug.Log("found");
            // Append only if not defined already.
            if (defines.Contains(define))
            {
                return;
            }

            // Append.
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, (defines + ";" + define));
            Debug.LogWarning("<b>" + define + "</b> added to <i>Scripting Define Symbols</i> for selected build target (" + EditorUserBuildSettings.activeBuildTarget.ToString() + ").");

            return;
        }
        foreach (string asset in deletedAssets)
        {
            if (!asset.EndsWith("com.utility_essentials.scriptable_variables"))
            {
                continue;
            }

            Debug.Log("lost");
            Debug.Log(defines);

            int i = defines.IndexOf(define);
            // Append only if not defined already.
            if (i < 0)
            {
                return;
            }
            Debug.Log("lost");

            defines = defines.Remove(i, define.Length);

            Debug.Log(defines);

            if (define[i] == ';')
            {
                defines = defines.Remove(i, 1);
            }
            Debug.Log(defines);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
            return;
        }
    }

    //static string GetPackageNameFromManifest(string manifestPath)
    //{
    //    //string manifestContent = File.ReadAllText(manifestPath);
    //    //// Extract package name from the manifest JSON content
    //    //// Example parsing logic:
    //    //JObject manifestJson = JObject.Parse(manifestContent);
    //    //string packageName = (string)manifestJson["name"];
    //    //return packageName;
    //}

    static void RemoveCompilationSymbol(string packageName)
    {
        string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        symbols = symbols.Replace(packageName, "").Replace(";;", ";");
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
    }
}