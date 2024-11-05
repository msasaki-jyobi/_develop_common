using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class ActionGrap : MonoBehaviour
    {
        [Tooltip("ダメージ側が再生する投げられモーション. わざわざActionDataにするのも手間なのでモーションだけ指定する仕様. 技の途中のダメージ処理については、攻撃側がFrameDataでダメージ判定をOnにする")]
        public string GrapClip;
        [Tooltip("再生時に実行する 座標/回転の調整値")]
        public Vector3 OffsetPos;
        public Vector3 OffsetRot;
    }
}
