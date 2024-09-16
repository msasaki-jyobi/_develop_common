using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace develop_common
{
    public class AIFunction : MonoBehaviour
    {
        //    /// <summary>
        //    /// �v���C���[�ւ̒ǐՂ��\���ǂ����H
        //    /// </summary>
        //    public bool LookPlayer()
        //    {
        //        float targetDistance =
        //               Vector3.Distance(transform.position, player.transform.position); // �ǐՑΏۂƂ̋����擾
        //        float angle = TargetAngle(transform, player.transform); // �ǐՑΏۂ�����p�x���擾
        //        GameObject getTarget = GetRayTarget(eye, player); // �ǐՑΏۂ�Ray���΂�

        //        bool check = true;
        //        if (!ignoreDistance) check = check && targetDistance <= chaseDistance; // �ǐՑΏۂƂ̋��������F�����ȓ����H
        //        if (!ignoreAngle) check = check && Mathf.Abs(angle) <= recognitionAngle; // �ǐՑΏۂ����F�p�x���ɂ��邩�H
        //        if (!ignoreRay) check = check && getTarget == player; // Ray���΂������ʁA�Ԃ��ꂽ�I�u�W�F�N�g���ǐՑΏۂ��H

        //        return check;
        //    }

        /// <summary>
        /// �����ƃ^�[�Q�b�g�̋������擾
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        static public float TargetDistance(GameObject self, GameObject target)
        {
            return Vector3.Distance(self.transform.position, target.transform.position);
        }

        /// <summary>
        /// �ǐՑΏۂƂ̊p�x���擾
        /// </summary>
        /// <param name="self">���g</param>
        /// <param name="target">�ǐՑΏ�</param>
        /// <returns></returns>
        static public float TargetAngle(GameObject self, GameObject target)
        {
            var diff = target.transform.position - self.transform.position;
            var axis = Vector3.Cross(self.transform.forward, diff);
            var angle = Vector3.Angle(self.transform.forward, diff)
                * (axis.y < 0 ? -1 : 1);
            return angle;
        }

        /// <summary>
        /// �ǐՑΏۂ�Ray���΂��āA�I�u�W�F�N�g�����m
        /// </summary>
        /// <param name="self">���g</param>
        /// <param name="target">�ǐՑΏ�</param>
        /// <param name="distance">ray���΂�����</param>
        /// <param name="rayHeight">Ray���΂���̍����␳(�����ɔ�΂��Ă��܂��ׁj</param>
        /// <returns></returns>
        static public GameObject GetRayTarget(GameObject self, GameObject target, float distance = 10f, float rayHeight = 1f)
        {
            // ���g����ǐՑΏۂ̕����ւq�������΂�
            Ray ray = new Ray(self.transform.position, (target.transform.position + target.transform.up * rayHeight) - self.transform.position);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);


            // 14�Ԗڂ�1�F�V�t�g���Z�q << ���g�p���āA�����l 1 ��14�r�b�g���ɃV�t�g 0x4000:�o�C�i���`��(0100 0000 0000 0000)
            int layerMask = 1 << 14;
            // ~ ���Z�q�̓r�b�g���Ƃ�NOT�i���]�j���Z ���ʁA14�Ԗڂ̃r�b�g������0�ŁA���̂��ׂẴr�b�g��1
            // 14�Ԗڂ̃��C���[����Ray�̑ΏۊO�ɂȂ�
            layerMask = ~layerMask;

            // ��������I�u�W�F�N�g�����m�����ꍇ�A�I�u�W�F�N�g��Ԃ�
            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                return hit.collider.gameObject;
            }
            return null;
        }
        /// <summary>
        /// �^�[�Q�b�g���O���ɂ��邩���m
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        static public bool TargetForward(GameObject self, GameObject target)
        {
            var check = self.transform.InverseTransformPoint(target.transform.position).z;
            if (check <= 0)
            {
                //Debug.Log("����ɂ���");
                return false;
            }
            else
            {
                //Debug.Log("�O���ɂ���");
                return true;
            }
        }


        
        /// <summary>
        /// �����̎��F�͈͓��ɂ���G�����ׂĎ擾
        /// </summary>
        /// <param name="self">���g</param>
        /// <param name="angle">���F�p�x</param>
        /// <param name="num">�擾����G�̐�</param>
        /// <param name="targetTag">�Ώۃ^�O</param>
        /// <returns></returns>
        public static List<GameObject> GetEnemyesWithinAngle(GameObject self, float angle, int num, string targetTag = "Enemy")
        {
            // �G��ێ����郊�X�g
            List<GameObject> enemiesWithinAngle = new List<GameObject>();
            // �S�Ă�Enemy�^�O�̃I�u�W�F�N�g���擾
            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag(targetTag);

            foreach (GameObject enemy in allEnemies)
            {
                // ���g�ƓG�Ƃ̊Ԃ̊p�x���v�Z
                float angleToEnemy = TargetAngle(self, enemy);

                // �p�x���w�肵���p�xA�ȓ����ǂ������`�F�b�N
                if (Mathf.Abs(angleToEnemy) <= angle)
                {
                    enemiesWithinAngle.Add(enemy);
                    //if (enemy.TryGetComponent<UnitStatus>(out var status))
                    //{
                    //    // �����Ă�Ȃ烊�X�g�ɒǉ�
                    //    if (!status.IsDead)
                    //        enemiesWithinAngle.Add(enemy);
                    //}
                }
            }

            // �G�����g�ɑ΂��鋗���Ń\�[�g
            List<GameObject> sortedEnemies = enemiesWithinAngle.OrderBy(enemy => (enemy.transform.position - self.transform.position).sqrMagnitude).ToList();

            // �w�肳�ꂽ��B�����擾
            return sortedEnemies.Take(num).ToList();
        }

        /// <summary>
        /// �������猩�ă^�[�Q�b�g�̏㉺�̊p�x���擾
        /// </summary>
        /// <param name="self">���g��GameObject</param>
        /// <param name="target">�^�[�Q�b�g��GameObject</param>
        /// <returns>�㉺�̊p�x</returns>
        public static float TargetVerticalAngle(GameObject self, GameObject target)
        {
            // �^�[�Q�b�g�ւ̃x�N�g�����v�Z
            Vector3 toTarget = target.transform.position - self.transform.position;

            // �����̑O�����x�N�g������Y�����������Đ����ʏ�̃x�N�g�����쐬
            Vector3 forwardHorizontal = self.transform.forward;
            forwardHorizontal.y = 0;

            // �����ʂ̃x�N�g���𐳋K��
            forwardHorizontal.Normalize();

            // �^�[�Q�b�g�ւ̃x�N�g���̐����������������āA�㉺�����݂̂̃x�N�g�����擾
            Vector3 toTargetVertical = toTarget - Vector3.Project(toTarget, forwardHorizontal);

            // �����̏�����ƃ^�[�Q�b�g�̏㉺�����x�N�g���Ƃ̊p�x���v�Z
            float angle = Vector3.Angle(self.transform.up, toTargetVertical) * (toTargetVertical.y < 0 ? -1 : 1);

            return angle;
        }
    }
}