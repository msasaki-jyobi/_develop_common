using develop_tps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    private StartManager _startManager;

    [SerializeField] private TPSUnitController _tpsUnitController;
    [SerializeField] private InputEnemyAI _inputEnemyAI;


    // Start is called before the first frame update
    void Start()
    {
        _startManager = StartManager.Instance;

        if (_startManager != null)
        {
            _startManager.StartFinishEvent += OnStartFinishHandle;
            EnableChanges(false);
        }
    }

    public void EnableChanges(bool flg)
    {
        if(_tpsUnitController != null)
            _tpsUnitController.enabled = flg;
        if (_inputEnemyAI != null)
            _inputEnemyAI.enabled = flg;
    }

    private void OnStartFinishHandle()
    {
        EnableChanges(true);
    }
}
