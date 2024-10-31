using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "FramePullData", menuName = "develop_common / FrameInfo / FramePullData")]
    public class FramePullData : ScriptableObject
    {
        public PullData PullDatas;
    }
    [Serializable]
    public class PullData
    {
        public string BodyKeyName; // 固定化する対象Body
        public string MotionName; // 実行するモーション一覧
        public Vector3 PullPos; // 回転値
        public List<Vector3> PullRots; // 回転値
    }
}
