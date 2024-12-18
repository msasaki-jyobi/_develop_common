using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace develop_common
{
    public class AutoChangeActive : MonoBehaviour
    {
        public UnitEnableController UnitEnableController;
        public GameObject TargetObject;
        [Header("一つでもアクティブならON(ReverseならOff) すべて非アクティブならOff(ReverseならON）")]
        public bool IsReverse;
        public List<GameObject> ActiveCheckObjects = new List<GameObject>();
        public bool IsNotChangeBattle;

        [Header("自動")]
        public ReactiveProperty<bool> IsBattle = new ReactiveProperty<bool>(); // ON:バトル中は、一つでもアクティブならONは動作しない
        //public bool IsBattle
        //{
        //    get => isBattle;
        //    set
        //    {
        //        Debug.Log($"IsBattle changed from {isBattle} to {value}");
        //        isBattle = value;
        //    }
        //}
        //private bool isBattle;

        private float _initTimer = 3;
        private bool _init;

        private  void Start()
        {
            //await UniTask.Delay(2000);
        }

        private void Init()
        {
            IsBattle
             .Subscribe((x) =>
             {
                 if (x)
                     Destroy(this);
             });

            UnitEnableController.OnChangeSetActiveEvent += OnChangeEnableObjectHandler;
            OnChangeEnableObjectHandler();
        }

        private void Update()
        {
            if (_initTimer > 0)
                _initTimer -= Time.deltaTime;

            if (!_init && _initTimer < 0)
            {
                _init = true;

                Init();
            }
        }

        private void OnDestroy()
        {
            if (UnitEnableController != null)
                UnitEnableController.OnChangeSetActiveEvent -= OnChangeEnableObjectHandler;
        }

        //private void OnEnable()
        //{
        //    OnChangeEnableObjectHandler();
        //}

        public void OnChangeEnableObjectHandler(GameObject target = null, bool active = false)
        {
            if (IsBattle.Value)
            {
                Debug.Log($"IsBattle is true, skipping changes., {gameObject.name}");
                return;
            }
            else
            {
                // 一つでもアクティブならON
                foreach (var obj in ActiveCheckObjects)
                {
                    if ((obj.activeSelf && !IsReverse) || (obj.activeInHierarchy && IsReverse))
                    {
                        Debug.Log($"Activating TargetObject, {gameObject.name}");
                        //UnitEnableController.OnChangeActiveObject(TargetObject, !IsReverse); // Unity落ちるレベルで重くなる
                        TargetObject.SetActive(!IsReverse);
                        return;
                    }
                }

                Debug.Log($"Deactivating TargetObject, {gameObject.name}");
                TargetObject.SetActive(IsReverse);
                //UnitEnableController.OnChangeActiveObject(TargetObject, IsReverse); // Unity落ちるレベルで重くなる
            }
        }

    }
}