using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace develop_common
{
    [Serializable]
    public class UIStateInfo
    {
        public string StateName;
        public List<GameObject> UIContents = new List<GameObject>();
        public UnityEvent PlayEvent;
    }
}
