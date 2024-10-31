﻿using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class UnitComponents : MonoBehaviour
    {
        public Animator Animator;
        public AnimatorStateController AnimatorStateController;
        public UnitActionLoader UnitActionLoader;
        public BattleDealer AttackDealer;
        [Space(10)]
        public PartAttachment PartAttachment;
        public UnitInstance UnitInstance;
        public Animator UnitAvatar;
        public UnitVoice UnitVoice;
        public UnitShape UnitShape;

        [Space(10)]
        public bool IsSetAvatar; 

        private void Start()
        {
            if(IsSetAvatar)
                Animator.avatar = UnitAvatar.avatar;
        }


    }
}