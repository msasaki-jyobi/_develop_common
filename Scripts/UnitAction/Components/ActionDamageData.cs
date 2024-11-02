
using System.Collections;
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
        [Header("部位別のヒット上限")]
        public EDamageType DamageType;

        [Header("追加Addtive")]
        public bool IsAddAddtive;
        [Tooltip("None:ActionStartの設定モーションが参照される")]public AnimationClip AddAdditiveMotion;
    }
}