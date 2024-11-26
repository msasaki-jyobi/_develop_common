using develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDestroy : MonoBehaviour
{
    public List<string> HitDestroyTags = new List<string>() { "Unit", "Wall", "Default" };
    public GameObject DestroyEffect;
    public AudioClip DestroyAudioClip;
    public ClipData DestroyClipData;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnHit(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHit(other.gameObject);
    }

    private void OnHit(GameObject hit)
    {
        foreach(var tag in HitDestroyTags) 
        { 
            if(hit.tag == tag)
            {
                Destroy(gameObject);
                UtilityFunction.PlayEffect(gameObject, DestroyEffect);
                develop_common.AudioManager.Instance.PlayOneShotClipData(DestroyClipData);
                develop_common.AudioManager.Instance.PlayOneShot(DestroyAudioClip, EAudioType.Se);
                return;
            }
        }
    }
}
