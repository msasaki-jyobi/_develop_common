using develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameEndController : MonoBehaviour
{
    [SerializeField] private UnitHealth _unitHealth;

    private void Start()
    {
        _unitHealth.InitDead
            .Subscribe((x) => {
                if(x)
                    GameEndManager.Instance.AddScore(); // ƒXƒRƒA‰ÁZ
            });
    }
}
