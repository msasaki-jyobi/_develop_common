using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    // IHealth インターフェース
    public interface IHealth
    {
        EUnitType UnitType { get; }
        float CurrentHealth { get; }
        void TakeDamage(DamageValue damageValue);
        void Heal(float amount);

    }

}
