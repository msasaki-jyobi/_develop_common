
using System.Collections;
using UnityEngine;

namespace develop_common
{
    public enum EDamageType
    {
        Normal,
        Additive,
    }
    [AddComponentMenu("ActionDamageData�F���̃��[�V�����_���[�W�����s�����b���")]
    public class ActionDamageData : MonoBehaviour
    {
        [Header("���[�V�����l�i�_���[�W�j")]
        public int MotionDamage;
        [Header("���S���[�V����")]
        public GameObject DeadAction;
        [Header("���ʕʂ̃q�b�g���")]
        public EDamageType DamageType;

        [Header("�ǉ�Addtive")]
        public bool IsAddAddtive;
        [Tooltip("None:ActionStart�̐ݒ胂�[�V�������Q�Ƃ����")]public AnimationClip AddAdditiveMotion;
    }
}