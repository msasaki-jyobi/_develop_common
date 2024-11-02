using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "FrameAttackChangeData", menuName = "develop_common / FrameInfo / FrameAttackChangeData")]
    public class FrameAttackChangeData : ScriptableObject
    {
        [Header("�ύX����_���[�W�A�N�V����(FrameRate����Q�Ɓj")]
        public GameObject ChangeDamageActions;
    }
}