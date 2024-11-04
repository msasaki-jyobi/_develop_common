using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{


    public class CursolManager : SingletonMonoBehaviour<CursolManager>
    {
        [SerializeField] private bool _isStartCursolLock;

        public bool IsSlowXKey;
        public float SlowTime = 0.2f;

        private void Start()
        {
            if (_isStartCursolLock)
                OnVisibleCursol(false);
        }

        private void Update()
        {
            if (IsSlowXKey)
                if (Input.GetKeyDown(KeyCode.X))
                    Time.timeScale = Time.timeScale == 1 ? SlowTime : 1;
        }

        public void OnVisibleCursol(bool visible)
        {
            Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = visible;
        }
    }
}
