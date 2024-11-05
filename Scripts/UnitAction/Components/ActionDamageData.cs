
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
        [Tooltip("ダメージ計算に使われる")]
        public int MotionDamage;
        [Header("死亡モーション")]
        [Tooltip("死亡時に再生するモーション. 現在機能していない")]
        public GameObject DeadAction;
        [Header("ダメージボイス")]
        [Tooltip("キャラクタ＾のオブジェクトに設定されたUnitVoiceと一致するボイスを再生する")]
        public string DamageVoiceKey;
        [Header("追加Addtive")]
        [Tooltip("追加でAdditiveモーションも組み合わせて再生を行う場合はON")]
        public bool IsAddAddtive;
        [Tooltip("Additiveで再生したいモーションデータを指定.  None:ActionStartの設定モーションが参照される")]
        public AdditiveDamageData AdditiveDamageData;
        [Header("Additiveのみ再生する（OFFだと組み合わせモーション）")]
        [Tooltip("OFFだと、Startで設定されたモーション再生は行われず、再生中のモーションにAdditiveモーションだけ組み合わされる")]
        public bool IsAddAddtiveOnly;
    }
}