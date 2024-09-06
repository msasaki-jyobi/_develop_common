using System.Collections;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
namespace GameSet.Common { 
    public class UtilityFunction : MonoBehaviour
    {

        /// <summary>
        /// 非同期処理を実装する
        /// </summary>
        /// <param name="time"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        static public IEnumerator Wait(float time, System.Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }

        /// <summary>
        /// Aの向いている方向から見て、Bの座標にセットする
        /// </summary>
        /// <param name="posA"></param>
        /// <param name="posB"></param>
        /// <returns></returns>
        static public Vector3 LocalLookPos(Transform posA, Vector3 posB)
        {
            Vector3 lookPos =
                posA.transform.right * posB.x +
                posA.transform.up * posB.y +
                posA.transform.forward * posB.z;
            return lookPos;
        }

        /// <summary>
        /// targetが向いている方向を基準にOffsetの方向に向く
        /// </summary>
        /// <param name="target">基準となるオブジェクト</param>
        /// <param name="setRotOffset">基準となるオブジェクトからどの方向に向くか</param>
        /// <returns></returns>
        static public Vector3 LocalSetRot(Transform target, Vector3 setRotOffset)
        {
            return target.localEulerAngles + setRotOffset;
        }
        //// 自分自身が向いている方向＋OffsetRot
        //Vector3 rot = transform.localEulerAngles + data.localEulerAngle;
        //// 生成したオブジェクトの向いている方向に反映
        //obj.transform.localRotation = Quaternion.Euler(rot);

        static public float GetAngle(Transform self, Transform target)
        {
            var diff = target.position - self.transform.position;
            var axis = Vector3.Cross(self.transform.forward, diff);
            var angle = Vector3.Angle(self.transform.forward, diff) * (axis.y < 0 ? -1 : 1);
            return angle;
        }
        /// <summary>
        /// マウスの座標とUI画像の座標を同期します
        /// </summary>
        /// <param name="cursorImage">カーソル用の画像</param>
        /// <param name="_canvasRect">カーソル用のRectTransform</param>
        /// <param name="_canvas">UIに利用しているCanvas</param>
        static public void CameraUI(Image cursorImage, Canvas _canvas, RectTransform _canvasRect)
        {
            Vector2 windowPosition;
            // カーソル非表示
            Cursor.visible = false;
            // 画面内でのマウス座標をUI座標に置き換える
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect,
                   Input.mousePosition, _canvas.worldCamera, out windowPosition);
            // カーソル画像の座標をマウスの位置に設定
            cursorImage.GetComponent<RectTransform>().anchoredPosition
                 = new Vector2(windowPosition.x, windowPosition.y);
        }

        /// <summary>
        /// クリックしたオブジェクトを返す
        /// </summary>
        /// <param name="camera">ゲーム用カメラ</param>
        /// <returns></returns>
        static public GameObject GetScreenClickObject(Camera camera, GameObject effectPrefab = null)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 3f);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (effectPrefab != null)
                {
                    GameObject effect = Instantiate(effectPrefab, hit.point, Quaternion.identity);
                    foreach (var g in effect.GetComponentsInChildren<ParticleSystem>())
                        g.loop = false;
                    Destroy(effect, 5f);
                }
                return hit.collider.gameObject;
            }
            return null;
        }

        /// <summary>
        /// クリックしたオブジェクトを親にして空オブジェクトを生成して返す
        /// </summary>
        /// <param name="camera">ゲーム用カメラ</param>
        /// <returns></returns>
        public static GameObject CreateScreenClickObject(Camera camera)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 3f);
            if (Physics.Raycast(ray, out hit, 100))
            {
                // 空のオブジェクト生成
                GameObject empty = Instantiate(new GameObject(), hit.point, Quaternion.Euler(camera.transform.rotation.eulerAngles));
                // 触れたオブジェクトに付与（親オブジェクト設定）
                empty.transform.parent = hit.collider.gameObject.transform;
                return empty;
            }
            return null;
        }

        /// <summary>
        /// ログを出力する
        /// </summary>
        /// <param name="message"></param>
        static public void Log(string message)
        {
            return;
            Debug.Log(message);
        }

        /// <summary>
        /// エフェクトを生成する
        /// </summary>
        /// <param name="root">エフェクトを生成する元のオブジェクト</param>
        /// <param name="effectPrefab">エフェクトプレハブ</param>
        /// <param name="loop">パーティクルシステムのループ</param>
        /// <param name="parentObject">設定したい親オブジェクト</param>
        public static void PlayEffect(GameObject root, GameObject effectPrefab, bool loop = false, GameObject parentObject = null, float destroyTime = 5f)
        {
            if (effectPrefab == null)
            {
                Debug.LogWarning($"{root.gameObject.transform.name}:effectはありません");
                return;
            }
            // エフェクトを生成する
            GameObject effect = Instantiate(effectPrefab, root.transform.position, Quaternion.identity);
            // ループ設定を反映
            foreach (var particle in effect.gameObject.GetComponentsInChildren<ParticleSystem>())
                particle.loop = loop;
            // 親オブジェクトを設定
            if (parentObject != null)
            {
                //effect.transform.parent = parentObject.transform;
                effect.transform.SetParent(parentObject.transform, false);
                effect.transform.localPosition = Vector3.zero;
                effect.transform.rotation = parentObject.transform.rotation;
            }
            Destroy(effect, destroyTime);
        }
    }
}