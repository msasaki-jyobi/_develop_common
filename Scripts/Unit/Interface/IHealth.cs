using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace develop_common
{
    // IHealth インターフェース
    public interface IHealth
    {
        EUnitType UnitType { get; }
        ReactiveProperty<EUnitStatus> UnitStatus { get; }
        int CurrentHealth { get; }
        int MaxHealth { get; }
        void TakeDamage(DamageValue damageValue = null);
        void Heal(float amount);
        void ChangeStatus(EUnitStatus status);

    }

}
