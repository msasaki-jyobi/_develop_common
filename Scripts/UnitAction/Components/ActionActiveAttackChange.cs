using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common 
{
    [AddComponentMenu("ActionActiveAttackChange：攻撃判定のDamageActionを変更する")]
    public class ActionActiveAttackChange : MonoBehaviour
    {
        [Header("変更するダメージアクション(FrameRateから参照）")]
        public List<GameObject> ChangeDamageActions = new List<GameObject>();
    }
}