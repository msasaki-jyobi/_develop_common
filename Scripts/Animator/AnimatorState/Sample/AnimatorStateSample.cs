using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSet.Common
{
    public class AnimatorStateSample : MonoBehaviour
    {
        public AnimatorStateController controller;

        public string Alpha1KeyPlayState = "State1";
        public string Alpha2KeyPlayState = "State2";
        public string Alpha2FinishNextState = "State3";

        public EStatePlayType DebugStatePlayType;
        public bool DebugStateReset;

        void Start()
        {
            controller.FinishMotionEvent += OnFinishMotionHandle;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                controller.ChangeMotion(Alpha1KeyPlayState, 30f, DebugStatePlayType, DebugStateReset);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                controller.ChangeMotion(Alpha2KeyPlayState, 30f, DebugStatePlayType, DebugStateReset);
            }
        }

        public void OnFinishMotionHandle(string motion)
        {
            if (motion == Alpha2KeyPlayState)
                controller.ChangeMotion(Alpha2FinishNextState, 30, DebugStatePlayType, DebugStateReset);
        }
    }

}
