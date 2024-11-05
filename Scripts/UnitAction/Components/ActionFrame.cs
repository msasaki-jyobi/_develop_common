using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace develop_common
{
    [AddComponentMenu("ActionFrame：フレーム情報")]
    public class ActionFrame : MonoBehaviour
    {
        public List<FrameInfo> FrameInfo = new List<FrameInfo>();
    }
}