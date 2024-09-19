using develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class ActionSample : MonoBehaviour
    {
        [SerializeField] private UnitActionLoader _unitActionLoader;

        [SerializeField] private GameObject _actionDataC;
        [SerializeField] private GameObject _actionDataX;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                _unitActionLoader.LoadAction(_actionDataC);
            if (Input.GetKeyDown(KeyCode.X))
                _unitActionLoader.LoadAction(_actionDataX);
        }
    }
}


