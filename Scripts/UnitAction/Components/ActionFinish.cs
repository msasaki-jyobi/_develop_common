using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class ActionFinish : MonoBehaviour
    {
        [Header("�I�����FUnitStatus�ύX")]
        // Status Change
        public EUnitStatus SetFinishStatus;
        [Header("�I�����F����ActionData")]
        // Next Action
        public GameObject NextActionData;
        [Header("�I�����F����\�ɂ���")]
        // IsPlay
        public bool IsActiveInputReader = true;



    }

}