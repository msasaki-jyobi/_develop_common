using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [AddComponentMenu("ActionFinish：終了時")]
    public class ActionFinish : MonoBehaviour
    {
        [Header("終了時：UnitStatus変更")]
        // Status Change
        public EUnitStatus SetFinishStatus;
        [Header("終了時：次のActionData")]
        // Next Action
        public GameObject NextActionData;
    }
}
