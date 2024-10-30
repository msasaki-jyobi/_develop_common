using System.Collections;
using UnityEngine;

namespace develop_common
{
    [AddComponentMenu("ActionStart：開始時")]
    public class ActionStart : MonoBehaviour
    {
        [Header("開始時：モーション関連")]
        // 再生モーション
        public string MotionName;
        // ApplyRootMotion
        public bool IsApplyRootMotion;
        // モーション繰り返しの有無
        public EStatePlayType StatePlayType;
        // 同じモーションでも繰り返し行うか
        public bool IsStateReset;
        [Header("開始時：UnitStatus変更")]
        // 開始時に切り替える状態
        public EUnitStatus SetStartStatus;
        [Header("開始時：Velocity Reset")]
        // Velocity Reset
        public bool IsResetVelocity;

        // レイヤー用
        [Header("レイヤー指定がある場合")]
        public int AnimatorLayer = 0;
        public float WeightValue = 1;
        public float WeightTime = 0.5f;
    }
}