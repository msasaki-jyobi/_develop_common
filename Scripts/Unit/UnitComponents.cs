using Cinemachine;
using develop_timeline;
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
        public develop_common.InstanceManager InstanceManager;
        public develop_common.ShapeManager ShapeManager;
        [Space(10)]
        public bool IsSetAvatar;

        [Space(10)]
        public UnitDirectorCharacterOffset UnitDirectorCharacterOffset;

        private void Start()
        {
            if (Animator != null)
                if (UnitAvatar != null)
                    if (IsSetAvatar)
                        Animator.avatar = UnitAvatar.avatar;
        }


    }
}