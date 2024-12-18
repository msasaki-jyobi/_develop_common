using System;
using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class UnitEnableController : MonoBehaviour
    {
        public event Action<GameObject, bool> OnChangeSetActiveEvent;

        public void OnChangeActiveObject(GameObject target, bool active)
        {
            target.SetActive(active);
            OnChangeSetActiveEvent?.Invoke(target, active);
        }


        //public event Action<SkinnedMeshRenderer, bool> ChangeEnableSkinEvent;

        //public void OnChangeSkinMesh(SkinnedMeshRenderer mesh, bool active)
        //{
        //    mesh.enabled = active;
        //    ChangeEnableSkinEvent?.Invoke(mesh, active);
        //}

    }
}