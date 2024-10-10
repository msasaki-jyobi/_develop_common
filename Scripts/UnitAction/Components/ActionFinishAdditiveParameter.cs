using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [AddComponentMenu("ActionFinishAdditiveParameter：終了時付与")]
    public class ActionFinishAdditiveParameter : MonoBehaviour 
    {
        [Header("終了時：付与する値")]
        public List<AdditiveParameter> FinishAdditiveParameters = new List<AdditiveParameter>();
    }
}