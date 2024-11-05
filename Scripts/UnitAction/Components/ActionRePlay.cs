using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class ActionRePlay : MonoBehaviour
    {
        [Tooltip("ヒット時に、再生するActionData. ActionGrapがあるDataならヒットした相手にモーションを実行させる")]
        public GameObject RePlayAction;
        public List<GameObject> RePlayActions = new List<GameObject>();
    }
}