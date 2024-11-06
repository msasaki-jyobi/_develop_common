//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace develop_common
//{

//    public class StageGenerateController : MonoBehaviour
//    {
//        public string GenerateName;

//        void Start()
//        {
//            foreach(var info in StageGenerateManager.Instance.StageGenerateInfos)
//            {
//                if(info.GenerateName == GenerateName)
//                {
//                    var prefab = Instantiate(info.SubmitPrefab, transform.position, transform.rotation);
//                    prefab.transform.parent = transform;
//                }
//            }
//        }

//        //private void OnDrawGizmosSelected()
//        //{
//        //    // IDに基づく一貫性のある色を生成
//        //    Color color = GetColorFromID(GenerateID);

//        //    // Gizmosに色を設定し、球体を描画
//        //    Gizmos.color = color;
//        //    Gizmos.DrawSphere(transform.position, StageGenerateManager.Instance.GizmoSize);
//        //}

//        ///// <summary>
//        ///// GetColorFromIDメソッド
//        //// idを受け取り、IDに基づいた一意の色を生成します。System.RandomにIDをシードとして渡し、
//        //// 各色成分（R, G, B）に対してNextDoubleを使用して0〜1の範囲で乱数を生成します。
//        //// これにより、同じIDが与えられた場合は常に同じ色が生成されます。
//        ///// </summary>
//        ///// <param name="id"></param>
//        ///// <returns></returns>
//        //// IDに基づいた色を返すメソッド
//        //private Color GetColorFromID(int id)
//        //{
//        //    // IDから一意の乱数を生成するためにシードを設定
//        //    System.Random random = new System.Random(id);

//        //    // RGBの各成分を0〜1の範囲で設定
//        //    float r = (float)random.NextDouble();
//        //    float g = (float)random.NextDouble();
//        //    float b = (float)random.NextDouble();

//        //    return new Color(r, g, b);
//        //}
//    }
//}
