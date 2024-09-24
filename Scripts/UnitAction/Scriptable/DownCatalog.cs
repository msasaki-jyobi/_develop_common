using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "DownCatalog", menuName = "Catalog / DownCatalog")]
    public class DownCatalog : ScriptableObject
    {
        public string PrimaryIDName;
        public List<DownInfo> Downs = new List<DownInfo>();
    }
}
