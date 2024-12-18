using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class AutoChangeShape : MonoBehaviour
    {
        public UnitEnableController UnitEnableController;
        public UnitInstance UnitInstance;
        public SkinnedMeshRenderer TargetMesh;
        public BlendShapeData OneEnableShapeData; // 1つでもアクティブなら実行するシェイプ
        public BlendShapeData AllDisableShapeData; // すべて非表示なら実行するシェイプ Shake？

        public List<GameObject> ActiveCheckObjects = new List<GameObject>();


        private async void Start()
        {
            Debug.Log($"{gameObject.name}, UnitEnable:{UnitEnableController}");
            UnitEnableController.OnChangeSetActiveEvent += OnChangeEnableObjectHandler;
            await UniTask.Delay(10);
            //OnChangeEnableObjectHandler();
        }

        private void OnDestroy()
        {
            if (UnitEnableController != null)
                UnitEnableController.OnChangeSetActiveEvent -= OnChangeEnableObjectHandler;
        }

 
        public void OnChangeEnableObjectHandler(GameObject target = null, bool active = false)
        {
            // 一つでもアクティブならBシェイプを実行

            foreach (var obj in ActiveCheckObjects)
            {
                if (obj.activeInHierarchy)
                {
                    ChangeShape(TargetMesh, OneEnableShapeData);
                    return;
                }
            }
            // すべて非アクティブならAシェイプを実行
            ChangeShape(TargetMesh, AllDisableShapeData);

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
}