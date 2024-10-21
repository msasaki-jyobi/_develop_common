using UnityEditor;
using UnityEngine;

public class FBXAnimationProcessor : EditorWindow
{
    [MenuItem("Tools/FBX Animation Processor")]
    public static void ShowWindow()
    {
        GetWindow<FBXAnimationProcessor>("FBX Animation Processor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Process Selected FBX Animations"))
        {
            ProcessSelectedFBXAnimations();
        }
    }

    private void ProcessSelectedFBXAnimations()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj is GameObject gameObject)
            {
                string assetPath = AssetDatabase.GetAssetPath(gameObject);
                ModelImporter modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;

                if (modelImporter != null)
                {
                    ModelImporterClipAnimation[] clipAnimations = modelImporter.defaultClipAnimations;

                    for (int i = 0; i < clipAnimations.Length; i++)
                    {
                        clipAnimations[i].loopTime = true;
                        clipAnimations[i].lockRootRotation = true;
                        clipAnimations[i].lockRootPositionXZ = false;

                        clipAnimations[i].lockRootHeightY = true;
                        clipAnimations[i].keepOriginalPositionY = false;
                        clipAnimations[i].heightFromFeet = true;
                        clipAnimations[i].heightOffset = 0f;
                    }

                    modelImporter.clipAnimations = clipAnimations;
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                }
            }
        }
    }
}
