using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSet.Common
{
    public enum EUnitType
    {
        Enemy,
        Player,
        Other
    }
    // PlayerHealth クラス
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private AnimatorStateController _animatorStateController;

        [SerializeField]
        private EUnitType unitType = EUnitType.Player;
        public EUnitType UnitType => unitType;
        [field: SerializeField] public float CurrentHealth { get; private set; } = 100f;


        public void TakeDamage(DamageValue damageValue)
        {
            CurrentHealth -= damageValue.Amount * damageValue.WeightDiff;
            if (damageValue.DamageAction.TryGetComponent<ActionBase>(out var actionBase))
            {
                _animatorStateController.ChangeMotion(actionBase.MotionName, actionBase.MotionLate, actionBase.StatePlayType, actionBase.StateReset, actionBase.ApplyRootMotion);

            }


            if (CurrentHealth <= 0)
            {
                Debug.Log("Player is dead");
                // プレイヤーが死亡する処理
            }
        }

        public void Heal(float amount)
        {
            CurrentHealth += amount;
            // 最大値を超えないようにする
        }
    }

}
