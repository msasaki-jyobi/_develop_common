using _develop_common;
using Cysharp.Threading.Tasks;
using develop_body;
using develop_common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDestroy : MonoBehaviour
{
    public List<string> HitDestroyTags = new List<string>() { "Unit", "Wall", "Default" };
    public GameObject DestroyEffect;
    public AudioClip DestroyAudioClip;
    public develop_common.ClipData DestroyClipData;

    [Tooltip("自分がHitColliderあって、触れた相手のBodyColliderのRootObjectのUnitActionLoaderが一緒なら無視します")]
    public bool SelfHitNoneDestroy = true;

    private HitCollider _selfHitCollider;

    void Start()
    {
        TryGetComponent(out _selfHitCollider);
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

    private async void OnHit(GameObject hit)
    {
        foreach(var tag in HitDestroyTags) 
        { 
            if(hit.tag == tag)
            {
                if(SelfHitNoneDestroy)
                {
                    if (_selfHitCollider != null)
                        if (hit.TryGetComponent<develop_body.BodyCollider>(out var body))
                        {
                            if (body.RootObject.TryGetComponent<UnitActionLoader>(out var loader))
                                if (loader.UnitType == _selfHitCollider.AttackerUnitType) // 同じユニットならReturn
                                    return;
                        }
                    else if (hit.TryGetComponent<UnitActionLoader>(out var loader2))
                        {
                            if (loader2.UnitType == _selfHitCollider.AttackerUnitType)
                                return;
                        }


                }
                await UniTask.Delay(1);
                Destroy(gameObject);
                UtilityFunction.PlayEffect(gameObject, DestroyEffect);
                develop_common.AudioManager.Instance.PlayOneShotClipData(DestroyClipData);
                develop_common.AudioManager.Instance.PlayOneShot(DestroyAudioClip, develop_common.EAudioType.Se);
                return;
            }
        }
    }
}
