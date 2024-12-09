using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace develop_common
{
    public class ShapeManager : MonoBehaviour
    {
        public string CostumeName = "：デフォ";
        public List<ShapeInstanceInfo> ShapeInstances = new List<ShapeInstanceInfo>();

        public void ChangeKeyNameShape(string keyName)
        {
            foreach (ShapeInstanceInfo info in ShapeInstances)
            {
                string targetInfoKeyName = info.KeyName;
                string targetkeyName = keyName;
                if (info.IsCostumeName) // KayNameの検知にTargetNameの付与が必要な場合
                    targetkeyName += $"{CostumeName}"; // KeyName + TargetNameにする

                Debug.Log($"targetInfoKeyName:{targetInfoKeyName}, targetkeyName:{targetkeyName}");

                if (info.KeyName == targetkeyName)
                {
                    ChangeShape(info.SkinMesh, info.blendShapeData);
                }
            }
        }

        public void ChangeShape(SkinnedMeshRenderer meshRenderer, develop_common.BlendShapeData blendShapeData)
        {
            // meshの最大数を取得
            var mesh = meshRenderer.sharedMesh;
            var blendValues = blendShapeData.BlendShapeList;
            int shapeAllLength = mesh.blendShapeCount;

            //表情のシェイプに変更が必要か全て確認する
            for (int i = 0; i < shapeAllLength; i++)
            {
                // 表情に必要なシェイプの値リストを確認していく 22, 45, 67 ...
                for (int j = 0; j < blendValues.Count; j++)
                {
                    // 今確認しているBodyのシェイプと一致するシェイプがあれば
                    if (mesh.GetBlendShapeName(i) == blendValues[j].ShapeName)
                    {
                        // Lerpの値をシェイプキーに格納
                        meshRenderer.SetBlendShapeWeight(i, blendValues[j].ShapeValue);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class ShapeInstanceInfo
    {
        public string KeyName;
        public bool IsCostumeName; // KeyNameにTargetNameの指定が必要
        public SkinnedMeshRenderer SkinMesh;
        public develop_common.BlendShapeData blendShapeData;
    }
}
