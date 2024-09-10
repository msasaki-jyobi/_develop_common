using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace develop_common
{


    public class ActionBase : MonoBehaviour
    {
        // 再生モーション
        public string MotionName;
        // 再生モーションのレート
        public int MotionLate = 30;
        // ApplyRootMotion
        public bool ApplyRootMotion;
        // モーション繰り返しの有無
        public EStatePlayType StatePlayType;
        // 同じモーションでも繰り返し行うか
        public bool StateReset;

        // 条件チェック
        // 実行後に行うなど…
        // 投げ技相手どうするなど…
    }
}