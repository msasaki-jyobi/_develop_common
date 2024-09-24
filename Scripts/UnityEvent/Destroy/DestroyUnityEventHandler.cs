using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace develop_common
{
    public class DestroyHandler : MonoBehaviour
    {
        public UnityEvent DestroyUnityEvent;
        private void OnDestroy()
        {
            DestroyUnityEvent?.Invoke();
        }
    }
}
