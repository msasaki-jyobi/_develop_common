using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
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
        private EUnitType _unitType = EUnitType.Player;
        public EUnitType UnitType => _unitType;

        [SerializeField]
        private EUnitStatus _unitStatus = EUnitStatus.Ready;
        public EUnitStatus UnitStatus => _unitStatus;

        [field: SerializeField] public int CurrentHealth { get; private set; } = 50;
        public int MaxHealth { get; private set; } = 50;
        public void TakeDamage(DamageValue damageValue)
        {
         


            if (CurrentHealth <= 0)
            {
                Debug.Log("Player is dead");
                // プレイヤーが死亡する処理
            }
        }

        public void Heal(float amount)
        {
            CurrentHealth += (int)amount;
            // 最大値を超えないようにする
        }
    }

}
