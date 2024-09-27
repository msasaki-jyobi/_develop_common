#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Animations;

namespace develop_common
{
    public class AnimatorStateWindow : EditorWindow
    {
        private GameObject selectedObject;
        private AnimatorStateController animatorStateController;

        [MenuItem("Tools/Write Animator State")]
        public static void ShowWindow()
        {
            GetWindow<AnimatorStateWindow>("Write Animator State");
        }

        void OnGUI()
        {
            GUILayout.Label("AnimatorStateControllerを持つオブジェクトをクリックし、現在の選択を取得 をクリック", EditorStyles.boldLabel);
            GUILayout.Label("1. Get All State をクリック", EditorStyles.label);

            // 現在選択中のオブジェクトを表示
            if (selectedObject != null)
            {
                GUILayout.Label("選択中のオブジェクト: " + selectedObject.name);
            }
            else
            {
                GUILayout.Label("選択中のオブジェクト: なし");
            }

            // 選択したオブジェクトがAnimatorStateControllerを持っているか確認
            if (GUILayout.Button("現在の選択を取得"))
            {
                selectedObject = Selection.activeGameObject;

                if (selectedObject != null)
                {
                    animatorStateController = selectedObject.GetComponent<AnimatorStateController>();
                    if (animatorStateController == null)
                    {
                        EditorUtility.DisplayDialog("エラー", "選択したオブジェクトにAnimatorStateControllerがありません。", "OK");
                        selectedObject = null;
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("エラー", "オブジェクトが選択されていません。", "OK");
                }
            }

            // "Get All State" ボタンを描画
            if (GUILayout.Button("Get All State"))
            {
                GetAllStates();
            }
        }

        void GetAllStates()
        {
            if (animatorStateController == null)
            {
                EditorUtility.DisplayDialog("エラー", "AnimatorStateControllerが選択されていません。", "OK");
                return;
            }

            Animator animator = selectedObject.GetComponent<Animator>();
            if (animator == null)
            {
                EditorUtility.DisplayDialog("エラー", "選択したオブジェクトにAnimatorがありません。", "OK");
                return;
            }

            List<string> stateNames = GetAllAnimatorStates(animator);

            // 取得したステート名をAnimatorStateControllerのStatesに書き込み
            animatorStateController.AllStates.Clear();  // 既存のステートをクリア
            animatorStateController.AllStates.AddRange(stateNames);

            EditorUtility.DisplayDialog("成功", "ステート名を取得し、AnimatorStateControllerに書き込みました。", "OK");
        }

        List<string> GetAllAnimatorStates(Animator animator)
        {
            List<string> stateNames = new List<string>();

            AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;

            if (controller != null)
            {
                foreach (var layer in controller.layers)
                {
                    foreach (var state in layer.stateMachine.states)
                    {
                        stateNames.Add(state.state.name);
                    }
                }
            }

            return stateNames;
        }
    }
}
#endif
