using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class ActionStart : MonoBehaviour
    {
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

        // 開始時に切り替える状態
        public EUnitStatus SetStartStatus;
    }
}