using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class ActionRePlay : MonoBehaviour
    {
        [Tooltip("ヒット時に、再生するActionData. ActionGrapがあるDataならヒットした相手にモーションを実行させる")]
        public GameObject RePlayAction;
    }
}