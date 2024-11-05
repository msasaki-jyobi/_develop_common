using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    [AddComponentMenu("ActionFinish：終了時")]
    public class ActionFinish : MonoBehaviour
    {
        [Header("終了時：UnitStatus変更")]
        [Tooltip("終了時に、実行したキャラクターのUnitActionLoaderの状態を切り替える")]
        public EUnitStatus SetFinishStatus;
        
        [Header("終了時：次のActionData")]
        [Tooltip("終了時に、実行したキャラクターが設定されたアクションを自動で実行する")]
        public GameObject NextActionData;
        
        [Tooltip("再生後に起き上がる事ができるようにするならONにする PartAttachmentのIsDownがONになる.")]
        public bool IsDown;
    }
}
