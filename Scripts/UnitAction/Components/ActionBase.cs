using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
namespace develop_common
{
    [AddComponentMenu("ActionBase：必須")]
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
        [Space(10)]
        public ActionActiveAttackBody ActionActiveAttackBody;
        public ActionActiveAttackChange ActionActiveAttackChange;
        public ActionPullData ActionPullData;
        [Space(10)]
        public ActionDamageData ActionDamageData;
        [Space(10)]
        public ActionStartAdditiveParameter ActionStartAdditiveParameter;
        public ActionFinishAdditiveParameter ActionFinishAdditiveParameter;

        // 条件チェック
        // 実行後に行うなど…
        // 投げ技相手どうするなど…
    }
}