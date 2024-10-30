using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common 
{
    [AddComponentMenu("ActionActiveAttackBody：攻撃判定のタイミング管理")]
    public class ActionActiveAttackBody : MonoBehaviour
    {
        [Header("攻撃がONになるコライダーの名称")]
        public List<string> AttackBodyNames;
        [Header("判定が残る時間")]
        public float AttackLifeTime; // 判定が残る時間
        [Header("ダメージアクション")]
        public GameObject DamageAction; // ダメージアクション
        [Header("Pull（固定化) Frameから自動上書きあり")]
        public bool IsPull;
        public PullData PullData;
    }
}