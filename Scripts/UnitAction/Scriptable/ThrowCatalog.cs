using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "ThrowCatalog", menuName = "Catalog / ThrowCatalog")]
    public class ThrowCatalog : ScriptableObject
    {
        public List<ThrowInfo> Throws = new List<ThrowInfo>();
    }
}
