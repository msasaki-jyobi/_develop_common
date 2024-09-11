using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    // IHealth インターフェース
    public interface IHealth
    {
        EUnitType UnitType { get; }
        int CurrentHealth { get; }
        int MaxHealth { get; }
        void TakeDamage(DamageValue damageValue);
        void Heal(float amount);

    }

}
