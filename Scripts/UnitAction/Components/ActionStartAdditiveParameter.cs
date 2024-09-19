using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class ActionStartAdditiveParameter : MonoBehaviour
    {
        [Header("開始時：付与する値")]
        public List<AdditiveParameter> StartAdditiveParameters = new List<AdditiveParameter>();
    }
}