using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveLoadScene : MonoBehaviour
{
    public List<string> SceneNames = new List<string>();
    void Start()
    {
       foreach (var sName in SceneNames) 
       {
            SceneManager.LoadScene(sName, LoadSceneMode.Additive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
