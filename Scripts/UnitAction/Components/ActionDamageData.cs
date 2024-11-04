
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public enum EDamageType
    {
        Normal,
        Additive,
    }
    [AddComponentMenu("ActionDamageData：このモーションダメージを実行する基礎情報")]
    public class ActionDamageData : MonoBehaviour
    {
        [Header("モーション値（ダメージ）")]
        public int MotionDamage;
        [Header("死亡モーション")]
        public GameObject DeadAction;
        [Header("ダメージボイス")]
        public string DamageVoiceKey;
        [Header("追加Addtive")]
        public bool IsAddAddtive;
        [Tooltip("None:ActionStartの設定モーションが参照される")]
        public AdditiveDamageData AdditiveDamageData;
        [Header("Additiveのみ再生する（OFFだと組み合わせモーション）")]
        public bool IsAddAddtiveOnly;
    }
}