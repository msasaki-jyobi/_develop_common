using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "FrameAttackChangeData", menuName = "develop_common / FrameInfo / FrameAttackChangeData")]
    public class FrameAttackChangeData : ScriptableObject
    {
        [Header("変更するダメージアクション(FrameRateから参照）")]
        public GameObject ChangeDamageActions;
    }
}
