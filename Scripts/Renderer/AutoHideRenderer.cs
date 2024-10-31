using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{

    public class AutoHideRenderer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Renderer>().enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
