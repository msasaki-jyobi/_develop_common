using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "DamageCatalog", menuName = "Catalog / DamageCatalog")]
    public class DamageCatalog : ScriptableObject
    {
        public int PrimaryID;
        public List<DamageInfo> Damages = new List<DamageInfo>();
    }
}
