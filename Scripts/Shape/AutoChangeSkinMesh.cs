using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class AutoChangeSkinMesh : MonoBehaviour
    {
        public UnitEnableController UnitEnableController;
        [Header("一つでもアクティブならskinOff すべて非アクティブならSkinOn")]
        public List<GameObject> ActiveCheckObjects = new List<GameObject>();

        [Header("Nullなら自身を取得")]
        public SkinnedMeshRenderer TargetMesh; // nullなら自動取得


        [SerializeField] private bool IsDebug;

        private async void Start()
        {
            if (TargetMesh == null)
            {
                TargetMesh = GetComponent<SkinnedMeshRenderer>();
                await UniTask.Delay(1);
            }

            UnitEnableController.OnChangeSetActiveEvent += OnChangeEnableObjectHandler;
            OnChangeEnableObjectHandler();
        }

        private void OnEnable()
        {
            OnChangeEnableObjectHandler();
        }

        public void OnChangeEnableObjectHandler(GameObject target = null, bool active = false)
        {
            // 一つでもアクティブならskinOff
            foreach (var obj in ActiveCheckObjects)
            {
                if (obj.activeInHierarchy)
                {
                    TargetMesh.enabled = false;
                    return;
                }
            }

            if (TargetMesh == null)
                TargetMesh = GetComponent<SkinnedMeshRenderer>();

            // すべて非アクティブならskinOn
            TargetMesh.enabled = true;
        }

    }
}