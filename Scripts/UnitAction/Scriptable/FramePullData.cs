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
        public string MotionName; // ���s���郂�[�V�����ꗗ
        public Vector3 PullPos; // ��]�l
        public List<Vector3> PullRots; // ��]�l
    }
}