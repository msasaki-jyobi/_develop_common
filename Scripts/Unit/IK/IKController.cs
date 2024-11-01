using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace develop_common
{
    public class IKController : MonoBehaviour
    {
        public List<IKInfo> IKInfos = new List<IKInfo>();

        public async void SetTargetEnableIK(string ikKeyName, float lifeTime, Transform target, float angle = 30)
        {
            // 対象との角度が範囲外ならReturn
            //if(angle > 30)  return

            foreach (IKInfo info in IKInfos)
            {
                if (info.IKKeyName == ikKeyName)
                {
                    info.IKFabric.enabled = true;

                    // 前方にいるならON：対策：ｓ

                    // IKのTargetを設定
                    info.IKFabric.Target = info.IKTarget.transform;
                    // IK：座標を同期
                    info.IKTarget.SyncTarget = target.gameObject;
                    // IK： 剣の向きをターゲットへのベクトルにする
                    //info.IKTarget.transform.rotation = Quaternion.Euler(info.IKFabric.transform.position - target.transform.position);

                    // AからBへの方向ベクトルを取得
                    //Vector3 direction = (target.transform.position - info.IKFabric.transform.position).normalized;
                    //// Bのforwardに方向ベクトルをそのまま反映
                    //info.IKTarget.transform.forward = direction;
                    info.IKTarget.IsTargetLook = true;
                    info.IKTarget.Origin = info.IKFabric.transform;


                    await UniTask.Delay((int)(1000 * lifeTime));
                    info.IKFabric.enabled = false;
                }
            }
        }
    }


}