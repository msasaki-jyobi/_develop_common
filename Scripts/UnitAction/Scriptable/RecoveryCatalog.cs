using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [CreateAssetMenu(fileName = "RecoveryCatalog", menuName = "Catalog / RecoveryCatalog")]
    public class RecoveryCatalog : ScriptableObject
    {
        public List<RecoveryInfo> Recoverys = new List<RecoveryInfo>();
    }
}
