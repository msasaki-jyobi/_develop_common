using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class AnimatorStateSample : MonoBehaviour
    {
        public AnimatorStateController controller;

        public string Alpha1KeyPlayState = "State1";
        public string Alpha1FinishNextState = "State3";
        [Space(20)]
        public string Alpha2KeyPlayAdditiveState = "GetUp";
        public float Alpha2KeyPlayAdditivefadeLength = 0;
        [Space(20)]
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
                controller.StatePlay(Alpha1KeyPlayState, DebugStatePlayType, DebugStateReset);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                controller.AnimatorLayerPlay(1, Alpha2KeyPlayAdditiveState, Alpha2KeyPlayAdditivefadeLength);
            }
        }

        public void OnFinishMotionHandle(string motion, bool isLoop)
        {
            if(!isLoop)
            {
                if (motion == Alpha2KeyPlayAdditiveState)
                    controller.StatePlay(Alpha1FinishNextState, DebugStatePlayType, DebugStateReset);
            }
        }
    }

}
