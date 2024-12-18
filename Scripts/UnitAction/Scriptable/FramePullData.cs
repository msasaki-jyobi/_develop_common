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
        public string BodyKeyName; // �Œ艻����Ώ�Body
        public Vector3 PullPos; // ��]�l
        public bool RandomRotX;
        public bool RandomRotY;
        public bool RandomRotZ;
        public List<Vector3> PullRots; // ��]�l

        public bool IsRandomCamera;
    }
}
