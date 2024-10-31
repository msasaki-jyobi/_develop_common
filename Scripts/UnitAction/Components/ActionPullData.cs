
using develop_body;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
  
    [AddComponentMenu("ActionPullData：ダメージを実行する際に固定する情報")]
    public class ActionPullData : MonoBehaviour
    {
        public List<PullData> PullDatas = new List<PullData>();
    }
    [Serializable]
    public class PullData
    {
        public string MotionName; // 実行するモーション一覧
        public string BodyKeyName; // 固定化する対象Body
        public Vector3 PullPos; // 回転値
        public List<Vector3> PullRots; // 回転値
    }
}