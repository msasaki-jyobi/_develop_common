using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace develop_common
{
    public class ActionBase : MonoBehaviour 
    {
        public ActionRequirement ActionRequirement;
        [Space(10)]
        public ActionStart ActionStart;
        [Space(10)]
        public ActionFrame ActionFrame;
        [Space(10)]
        public ActionFinish ActionFinish;
        [Space(10)]
        public ActionParent ActionParent;
        public ActionPrefabInfo ActionPrefabInfo;
        public ActionDamageValue ActionDamageValue;
        [Space(10)]
        public ActionStartAdditiveParameter ActionStartAdditiveParameter;
        public ActionFinishAdditiveParameter ActionFinishAdditiveParameter;

        // �����`�F�b�N
        // ���s��ɍs���Ȃǁc
        // �����Z����ǂ�����Ȃǁc
    }
}