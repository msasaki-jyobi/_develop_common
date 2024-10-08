using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    // IHealth インターフェース
    public interface IHealth
    {
        EUnitType UnitType { get; }
        EUnitStatus UnitStatus { get; }
        int CurrentHealth { get; }
        int MaxHealth { get; }
        void TakeDamage(DamageValue damageValue = null);
        void Heal(float amount);
        void ChangeStatus(EUnitStatus status);

    }

}
