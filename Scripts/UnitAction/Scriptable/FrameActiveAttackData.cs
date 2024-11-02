using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "FrameActiveAttackData", menuName = "develop_common / FrameInfo / FrameActiveAttackData")]
    public class FrameActiveAttackData : ScriptableObject
    {
        [Header("�U����ON�ɂȂ�R���C�_�[�̖���")]
        public List<string> AttackBodyNames;
        [Header("���肪�c�鎞��")]
        public float AttackLifeTime; // ���肪�c�鎞��
        //[Header("�_���[�W�A�N�V����")]
        //public GameObject DamageAction; // �_���[�W�A�N�V����
        //[Header("Pull�i�Œ艻) Frame���玩���㏑������")]
        //public bool IsPull;
        //public PullData PullData;
    }
}