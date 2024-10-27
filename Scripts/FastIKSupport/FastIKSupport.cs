using Common;
using DitzelGames.FastIK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastIKSupport : MonoBehaviour
{
    public GameObject RootObject;
    public FastIKFabric FastIKFabric;
    public string TargetKeyName; // ’ÇÕ‘ÎÛ‚ÌKeyName PartAttachment

    public bool IsSync; // è‘«‚É‚Â‚¢‚Ä‚¢‚­
    public Vector3 Offset = new Vector3(0, -0.5f, 0);


    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        if (_gameManager != null)
        {
            FastIKFabric.Target = _gameManager.PlayerComponents.PartAttachment.GetBody(TargetKeyName).transform;
            _gameManager.ChangeCharacterEvent += OnChangeCharacterHandle;
        }


    }

    private void Update()
    {
        if(FastIKFabric.Target != null && IsSync)
        {
            var target = FastIKFabric.Target.transform;
            RootObject.transform.position = target.position + target.right * Offset.x + target.up * Offset.y + target.forward * Offset.z;

            // ¡“x‚Í’wå‚Ì‘ƒ‚Æ‚©”­“®‚µ‚½‚çON@‰ğœ‚ÅOFF
        }
    }

    private void OnChangeCharacterHandle(UnitComponents unitComponents)
    {
        FastIKFabric.Target = unitComponents.PartAttachment.GetBody(TargetKeyName).transform;
    }

}
