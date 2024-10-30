using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(menuName = "develop_common / WordData")]
    public class ShapeWordData : ScriptableObject
    {
        [Multiline]
        public string WordData;
        public List<string> NotWardData = new List<string>();
    }
}