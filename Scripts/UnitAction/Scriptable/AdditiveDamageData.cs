
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "AdditiveDamageData", menuName = "develop_common / Data / AdditiveDamageData")]
    public class AdditiveDamageData : ScriptableObject
    {
        public List<string> AdditiveDatas;
    }
}