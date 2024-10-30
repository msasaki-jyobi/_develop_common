using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [AddComponentMenu("ActionPrefabInfo：生成Prefab")]
    public class ActionPrefabInfo : MonoBehaviour
    {
        public List<PrefabData> PrefabDatas = new List<PrefabData>();

        public async void CreatePrefab(int listIndex, GameObject unit)
        {
            if (PrefabDatas.Count == 0) return;
            if (PrefabDatas.Count <= listIndex) return;

            var data = PrefabDatas[listIndex];

            // 生成するAttackPointの特定
            GameObject pointObject = unit.gameObject;
            if (data.ParentKeyName != "")
                if (unit.TryGetComponent<UnitComponents>(out var unitComponents))
                {
                    var parent = unitComponents.UnitInstance.SearchObject(data.ParentKeyName);
                    if (parent != null)
                        pointObject = parent;
                }
            //// UnitBodyのアタックポイントを一つずつチェックして、一致するものをPointObjectに設定
            //if (_unitStatus.UnitBodys.Count != 0) // count0ならオブジェクトを設定
            //{
            //    foreach (var unitBody in _unitStatus.UnitBodys)
            //        if (data.BodyPoint == unitBody.BodyPoint)
            //            pointObject = unitBody.BodyObject;
            //}

            // オブジェクトの向く方向を指定
            Vector3 lookPos =
            UtilityFunction.LocalLookPos(unit.gameObject.transform, data.LocalPosition);

            // Position
            Vector3 pos = pointObject.transform.position + lookPos;

            // Instantiate
            GameObject prefab = Instantiate(data.Prefab, pos, Quaternion.identity);

            // Rotation
            GameObject rotOrigin = gameObject;
            if (data.LookType == ELookType.Camera) // カメラならカメラの向きに依存
                rotOrigin = Camera.main.gameObject;
            Vector3 rot = rotOrigin.transform.localEulerAngles + data.LocalEulerAngle;
            prefab.transform.rotation = Quaternion.Euler(rot);

            // Scale
            if (data.SetScale != Vector3.zero)
                prefab.transform.localScale = data.SetScale;

            // 効果音再生
            AudioManager.Instance.PlayOneShot(data.CreateSe, EAudioType.Se);

            // Parent
            if (data.ParentType == EParentType.SetParent) // Parent
                prefab.transform.parent = pointObject.transform;

            // ダメージの設定
            //if (prefab.TryGetComponent<DamageDealer>(out var dealer))
            //{
            //    if (unit.TryGetComponent<IHealth>(out var health))
            //        dealer.AttackUnitType = health.UnitType;

            //    if (TryGetComponent<ActionDamageValue>(out var actionDamageValue))
            //    {
            //        dealer.DamageValue.OverrideDamageValue(actionDamageValue.DamageValue);
            //        dealer.DamageValue.AttackerUnit = unit;
            //    }
            //}

            Destroy(prefab, data.DestroyTime);
        }
    }
}