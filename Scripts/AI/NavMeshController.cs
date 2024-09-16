using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace develop_common
{
    public class NavMeshController : MonoBehaviour
    {
        public GameObject Target;
        public NavMeshAgent Agent;
        public GameObject Eye;

        public float NormalSpeed = 3.5f;
        public float HuntSpeed = 3.5f;

        public bool IsDistance; // �w�苗���ȓ����K�v
        public float distance = 5f;
        public bool IsLook; // ���F���K�v

        private void Start()
        {
            if(Target == null)
            Target = GameObject.Find("Player");

            // ���a10m�ȓ��̜p�j�ꏊ�������擾
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (Target == null || Agent == null) return;  // Target �܂��� Agent ���Ȃ��ꍇ�͉������Ȃ�

            bool check = true;
            if (IsDistance) // �������`�F�b�N
                check = check && AIFunction.TargetDistance(Target, gameObject) <= distance;
            if (IsLook) // ���F���`�F�b�N
                check = check && AIFunction.GetRayTarget(Eye, Target, rayHeight: 0.5f);

            if (check)
            {
                // �^�[�Q�b�g��ǐՂ��邽�߂̈ړ����J�n
                Agent.speed = HuntSpeed;  // �ǐ՗p�̃X�s�[�h�ɐݒ�
                Agent.SetDestination(Target.transform.position);  // �^�[�Q�b�g�Ɍ������Ĉړ�
            }
            else
            {
                // �^�[�Q�b�g��ǂ�Ȃ��ꍇ�A�ʏ�̓���ɖ߂�i�K�v�ł���Ύ����j
                Agent.speed = NormalSpeed;  // �ʏ�̑��x�ɖ߂�

                // ���a10m�ȓ��̒T���ꏊ�Ɍ�����



            }
        }

    }
}