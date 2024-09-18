using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [Serializable]
    public class UIStateInfo
    {
        public string StateName;
        public List<GameObject> UIContents = new List<GameObject>();
    }
}
