using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace develop_common
{
    public class ShapeManager : SingletonMonoBehaviour<ShapeManager>
    {
        public string TargetPlayerName = "";
        public List<ShapeInstanceInfo> ShapeInstances = new List<ShapeInstanceInfo>();

        public void ChangeKeyNameShape(string keyName)
        {
            foreach (ShapeInstanceInfo info in ShapeInstances)
            {
                string targetInfoKeyName = info.KeyName;
                string targetkeyName = keyName;
                if (info.IsTargetName) // KayName�̌��m��TargetName�̕t�^���K�v�ȏꍇ
                    targetkeyName += TargetPlayerName; // KeyName + TargetName�ɂ���

                Debug.Log($"targetInfoKeyName:{targetInfoKeyName}, targetkeyName:{targetkeyName}");

                if (info.KeyName == targetkeyName)
                {
                    ChangeShape(info.SkinMesh, info.blendShapeData);
                }
            }
        }

        public void ChangeShape(SkinnedMeshRenderer meshRenderer, develop_common.BlendShapeData blendShapeData)
        {
            // mesh�̍ő吔���擾
            var mesh = meshRenderer.sharedMesh;
            var blendValues = blendShapeData.BlendShapeList;
            int shapeAllLength = mesh.blendShapeCount;

            //�\��̃V�F�C�v�ɕύX���K�v���S�Ċm�F����
            for (int i = 0; i < shapeAllLength; i++)
            {
                // �\��ɕK�v�ȃV�F�C�v�̒l���X�g���m�F���Ă��� 22, 45, 67 ...
                for (int j = 0; j < blendValues.Count; j++)
                {
                    // ���m�F���Ă���Body�̃V�F�C�v�ƈ�v����V�F�C�v�������
                    if (mesh.GetBlendShapeName(i) == blendValues[j].ShapeName)
                    {
                        // Lerp�̒l���V�F�C�v�L�[�Ɋi�[
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
        public bool IsTargetName; // KeyName��TargetName�̎w�肪�K�v
        public SkinnedMeshRenderer SkinMesh;
        public develop_common.BlendShapeData blendShapeData;
    }
}