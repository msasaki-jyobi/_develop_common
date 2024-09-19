using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class ActionFinish : MonoBehaviour
    {
        [Header("終了時：UnitStatus変更")]
        // Status Change
        public EUnitStatus SetFinishStatus;
        [Header("終了時：次のActionData")]
        // Next Action
        public GameObject NextActionData;
        [Header("終了時：操作可能にする")]
        // IsPlay
        public bool IsActiveInputReader = true;



    }

}
