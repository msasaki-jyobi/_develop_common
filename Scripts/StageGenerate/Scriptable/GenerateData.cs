using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "GenerateData", menuName = "develop_common / StageGenerateData")]
    public class GenerateData : ScriptableObject
    {
        public List<GameObject> Prefabs = new List<GameObject>();
    }

}
