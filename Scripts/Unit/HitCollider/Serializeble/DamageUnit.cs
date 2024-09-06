using System;
using System.Collections;
using UnityEngine;

namespace GameSet.Common
{
    [Serializable]
    public class DamageUnit
    {
        // オブジェクト
        public GameObject UnitObject;
        // ヒット回数
        public int HitCount = default;
        // タイマー
        public float HitTimer = default;
    }
}