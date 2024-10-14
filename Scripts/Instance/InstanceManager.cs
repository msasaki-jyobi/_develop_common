using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class InstanceManager : SingletonMonoBehaviour<InstanceManager>
    {
        public UnitVoice UnitVoice;
        public UnitActionLoader UnitActionLoader;
    }
}