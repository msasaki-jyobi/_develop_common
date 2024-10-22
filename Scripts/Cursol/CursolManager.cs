using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursolManager : SingletonMonoBehaviour<CursolManager>
{
    [SerializeField] private bool _isStartCursolLock;

    private void Start()
    {
        if (_isStartCursolLock)
            OnVisibleCursol(false);
    }

    public void OnVisibleCursol(bool visible)
    {
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;
    }
}
