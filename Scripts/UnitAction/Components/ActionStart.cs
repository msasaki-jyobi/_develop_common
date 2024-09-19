using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class ActionStart : MonoBehaviour
    {
        [Header("開始時：モーション関連")]
        // 再生モーション
        public string MotionName;
        // 再生モーションのレート
        public int MotionLate = 30;
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
        [Header("開始時：操作不可能にする")]
        // 操作不可
        public bool IsNotInputReader = true;
    }
}