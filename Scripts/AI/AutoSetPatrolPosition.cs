using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class AutoSetPatrolPosition : MonoBehaviour
    {
        public NavMeshController NavMeshController;
        public List<GameObject> PatrolPositions = new List<GameObject>();

        private void Start()
        {
            foreach (GameObject obj in PatrolPositions) 
            {
                NavMeshController.PatrolPositions.Add(obj);
            }
        }

    }
}