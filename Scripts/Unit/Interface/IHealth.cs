using _develop_common;
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
        void TakeDamage(GameObject damageAction, bool isPull, int totalDamage, bool InitRandomCamera = false, List<string> bodyNames = default);
        void Heal(float amount);
        void ChangeStatus(EUnitStatus status);
        bool IsInvisible { get; }

    }

}
