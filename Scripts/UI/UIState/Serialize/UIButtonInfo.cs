using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace develop_common
{
    [Serializable]
    public class UIButtonInfo
    {
        public string InteractableStateName;
        public List<Button> EnableButtons;
    }
}