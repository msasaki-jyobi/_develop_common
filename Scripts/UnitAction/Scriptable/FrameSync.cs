using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "FrameSync", menuName = "develop_common / FrameInfo / FrameSync")]
    public class FrameSync : ScriptableObject
    {
        // A��B�̍��W�ɍ����悤�Ɉړ�����@�i��F�n���}�[�̕����E���Ɂj
        [Header("A��D�̈ʒu�ɍ��킹��")]
        public string AttackKeyName;
        public string DamageKeyName;

        public Vector3 WorldOffset;
    }
}