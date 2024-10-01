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


        // �c�[���o�[�Ƀ{�^���쐬
        [MenuItem("Tools / ShapeCheck/ Shape Check", false, 1)]
        private static void ShowWindow()
        {
            ShapeCheck window = GetWindow<ShapeCheck>();
            window.titleContent = new GUIContent("ShapeCheckWindow");
        }

        private void OnGUI()
        {
            // ���͉\�ȃe�L�X�g�t�B�[���h��ݒu
            //GUILayout.(bonePath);
            SkinnedMeshRenderer = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("_ShapeController_", SkinnedMeshRenderer, typeof(SkinnedMeshRenderer), true);
            ShapeData = (BlendShapeData)EditorGUILayout.ObjectField("_ShapeData_", ShapeData, typeof(BlendShapeData), true);

            // �c�ɔz�u
            using (new EditorGUILayout.VerticalScope())
            {
                // �{�^���𐶐�
                if (GUILayout.Button("SetShape"))
                {
                    //// �I������Ă���I�u�W�F�N�g���擾
                    //GameObject obj = Selection.activeGameObject;

                    //// �p�X���擾
                    //// https://amagamina.jp/blog/gameobject-fullpath/
                    //string path = obj.name;
                    //var parent = obj.transform.parent;
                    //while (parent)
                    //{
                    //    path = $"{parent.name}/{path}";
                    //    parent = parent.parent;
                    //}
                    //// ���O�ɏo��
                    //Debug.Log(path);
                    //// �e�L�X�g�t�B�[���h�ɏo��
                    ////bonePath = path;
                    //// �N���b�v�{�[�h�ɏo��
                    //EditorGUIUtility.systemCopyBuffer = path;

                    Mesh mesh = SkinnedMeshRenderer.sharedMesh;
                    int shapeLength = mesh.blendShapeCount; // Shape�̐����擾

                    // �O��̃V�F�C�v���c��ׁA���Z�b�g���Ă���
                    for (int j = 0; j < shapeLength; j++)
                    {
                        SkinnedMeshRenderer.SetBlendShapeWeight(j, 0);
                    }

                    for (int i = 0; i < shapeLength; i++)
                    {
                        // ���b�V������V�F�C�v�̖��O���擾
                        string shapeName = mesh.GetBlendShapeName(i);

                        for (int x = 0; x < ShapeData.BlendShapeList.Count; x++)
                        {
                            // �V�F�C�v������v����Ȃ�
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