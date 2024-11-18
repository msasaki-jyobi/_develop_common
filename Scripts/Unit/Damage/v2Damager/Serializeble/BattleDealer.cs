using _develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class BattleDealer : MonoBehaviour
    {
        public develop_common.UnitComponents UnitComponents;
        [Header("攻撃を行うHitCollider")]
        public List<AttackBodyInfo> AttackBodyInfos = new List<AttackBodyInfo>();

        public void SetAttack(string attackBodyName, float lifeTime, GameObject damageAction, bool isPull = false, PullData pullData = default)
        {
            foreach(AttackBodyInfo info in AttackBodyInfos)
            {
                LogManager.Instance.AddLog(gameObject, $"XX IsPull:{isPull}, IsChangeDamage:{pullData}", 1);
                if (info.AttackBodyName == attackBodyName)
                {
                    info.HitCollider.AttackLifeTime = lifeTime; // 攻撃判定時間上書き
                    info.HitCollider.DamageAction = damageAction; // ダメージモーション上書き
                    info.HitCollider.IsPull = isPull; // 固定化の有無
                    info.HitCollider.PullData = pullData; // 固定化の情報
                    info.HitCollider.AttakerActionLoader = UnitComponents.UnitActionLoader; // 攻撃者のActionLoader
                    info.HitCollider.AttackerUnitType = UnitComponents.UnitActionLoader.UnitType; // 攻撃者のUnitType
                }
            }
        }
        public GameObject GetAttack(string attackBodyName)
        {
            foreach (AttackBodyInfo info in AttackBodyInfos)
            {
                if (info.AttackBodyName == attackBodyName)
                {
                    return info.HitCollider.gameObject;
                }
            }
            return null;
        }
    }
}