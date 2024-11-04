using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "FrameSync", menuName = "develop_common / FrameInfo / FrameSync")]
    public class FrameSync : ScriptableObject
    {
        // AをBの座標に合うように移動する　（例：ハンマーの柄を右肩に）
        [Header("AをDの位置に合わせる")]
        public string AttackKeyName;
        public string DamageKeyName;

        public Vector3 WorldOffset;
    }
}
