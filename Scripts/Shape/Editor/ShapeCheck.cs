using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace develop_common
{
    public class ShapeCheck : EditorWindow
    {
        public SkinnedMeshRenderer SkinnedMeshRenderer;
        public BlendShapeData ShapeData;


        // ツールバーにボタン作成
        [MenuItem("Tools / ShapeCheck/ Shape Check", false, 1)]
        private static void ShowWindow()
        {
            ShapeCheck window = GetWindow<ShapeCheck>();
            window.titleContent = new GUIContent("ShapeCheckWindow");
        }

        private void OnGUI()
        {
            // 入力可能なテキストフィールドを設置
            //GUILayout.(bonePath);
            SkinnedMeshRenderer = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("_ShapeController_", SkinnedMeshRenderer, typeof(SkinnedMeshRenderer), true);
            ShapeData = (BlendShapeData)EditorGUILayout.ObjectField("_ShapeData_", ShapeData, typeof(BlendShapeData), true);

            // 縦に配置
            using (new EditorGUILayout.VerticalScope())
            {
                // ボタンを生成
                if (GUILayout.Button("SetShape"))
                {
                    //// 選択されているオブジェクトを取得
                    //GameObject obj = Selection.activeGameObject;

                    //// パスを取得
                    //// https://amagamina.jp/blog/gameobject-fullpath/
                    //string path = obj.name;
                    //var parent = obj.transform.parent;
                    //while (parent)
                    //{
                    //    path = $"{parent.name}/{path}";
                    //    parent = parent.parent;
                    //}
                    //// ログに出力
                    //Debug.Log(path);
                    //// テキストフィールドに出力
                    ////bonePath = path;
                    //// クリップボードに出力
                    //EditorGUIUtility.systemCopyBuffer = path;

                    Mesh mesh = SkinnedMeshRenderer.sharedMesh;
                    int shapeLength = mesh.blendShapeCount; // Shapeの数を取得

                    // 前回のシェイプが残る為、リセットしておく
                    for (int j = 0; j < shapeLength; j++)
                    {
                        SkinnedMeshRenderer.SetBlendShapeWeight(j, 0);
                    }

                    for (int i = 0; i < shapeLength; i++)
                    {
                        // メッシュからシェイプの名前を取得
                        string shapeName = mesh.GetBlendShapeName(i);

                        for (int x = 0; x < ShapeData.BlendShapeList.Count; x++)
                        {
                            // シェイプ名が一致するなら
                            if (shapeName == ShapeData.BlendShapeList[x].ShapeName)
                            {
                                SkinnedMeshRenderer.SetBlendShapeWeight(i, ShapeData.BlendShapeList[x].ShapeValue);
                            }
                        }
                    }
                };
            }
        }

    }

}
