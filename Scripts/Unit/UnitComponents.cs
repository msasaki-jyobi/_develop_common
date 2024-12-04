using Cinemachine;
using System.Collections;
using UnityEngine;

namespace develop_common
{
    public class UnitComponents : MonoBehaviour
    {
        public Animator Animator;
        public AnimatorStateController AnimatorStateController;
        public UnitActionLoader UnitActionLoader;
        public UnitHealth UnitHealth;
        public CinemachineFreeLook BodyFreeLook; // 自由視点じゃないFreeLook
        [Space(10)]
        public BattleDealer AttackDealer;
        public PartAttachment PartAttachment;
        public UnitInstance UnitInstance;
        public Animator UnitAvatar;
        public UnitShape UnitShape;
        public UnitVoice UnitVoice;
        public develop_body.UnitBody UnitBody;
        [Space(10)]
        public IKController AttaqckIKController;

        [Space(10)]
        public bool IsSetAvatar; 

        private void Start()
        {
            if(IsSetAvatar)
                Animator.avatar = UnitAvatar.avatar;
        }


    }
}