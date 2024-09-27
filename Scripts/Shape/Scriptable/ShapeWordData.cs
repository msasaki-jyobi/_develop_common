using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(menuName = "Shape / WordData")]
    public class ShapeWordData : ScriptableObject
    {
        [Multiline]
        public string WordData;
        public List<string> NotWardData = new List<string>();
    }
}