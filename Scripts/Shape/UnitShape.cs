using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    /// <summary>
    /// ユニットのシェイプを管理するクラス
    /// </summary>
    [AddComponentMenu("UnitShape：表情管理")]
    public class UnitShape : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer; // 表情を変更するSkinMeshRenderer
        [SerializeField] private List<BlendShapeData> _blendShapeDatas; // シェイプの中身を保持したデータリスト
        [SerializeField] private float _defaultChangeSpeed = 3; // Shapeを滑らかに切り替える速度
        [SerializeField] private float _defaultChangeInterval = 0.5f; // Shapeを切り替える周期
        [SerializeField] private ShapeWordData _defaultShapeWordData; // デフォルトのワードデータ
        [Header("設定不要")]
        [SerializeField] private ShapeWordData _playShapeWordData; // 常時再生に利用するワードデータ

        private float _changeIntervalTimer; // カウント用
        private float _changeInterval; // 切り替え間隔
        private float _changeSpeed; // 切り替えスピード
        private int shapeAllLength; // BlendShape最大数保持
        private Mesh mesh; // BlendShape最大数取得用

        void Start()
        {
            // meshの最大数を取得
            mesh = _skinnedMeshRenderer.sharedMesh;
            shapeAllLength = mesh.blendShapeCount;

            // デフォルトのシェイプをセットしておく
            SetShapeWard(_defaultShapeWordData);
        }

        void Update()
        {
            // シェイプがなければ終了
            if (_blendShapeDatas.Count == 0) return;

            if (_changeIntervalTimer >= 0)
                _changeIntervalTimer -= Time.deltaTime;

            if (_changeIntervalTimer <= 0)
            {
                if (_blendShapeDatas == null) return;
                if (_playShapeWordData == null) return;
                //Debug.Log($"***SHAPE****:1:{_blendShapeDatas}, 2:{_playShapeWordData.shapeWordData}, 3:{_playShapeWordData.notWardData}");
                KeyWardShape(_blendShapeDatas, _playShapeWordData.WordData, _playShapeWordData.NotWardData);
                _changeIntervalTimer = _changeInterval;
            }

        }

        /// <summary>
        /// シェイプをセットする
        /// </summary>
        /// <param name="shapeWordData">再生するキーワードデータ（newして作成しても良い）</param>
        /// <param name="changeSpeed">切り替えスピード</param>
        /// <param name="changeInterval">切り替え間隔</param>
        public void SetShapeWard(ShapeWordData shapeWordData, float changeSpeed = -1, float changeInterval = -1)
        {
            _playShapeWordData = shapeWordData; // 切り替えワードを設定
            _changeSpeed = changeSpeed == -1 ? _defaultChangeSpeed : changeSpeed; // 切り替えスピードを設定
            _changeInterval = changeInterval == -1 ? _defaultChangeInterval : changeInterval; // 切り替え間隔を設定
            _changeIntervalTimer = 0;
        }

        /// <summary>
        /// 指定されたキーワードのシェイプを一致するものの中からランダムで選定
        /// </summary>
        /// <param name="blendShapeDatas">表情一覧が格納された変数を入れる</param>
        /// <param name="keyWard">実行したい表情のキーワード</param>
        /// <param name="notWard">実行したくない表情のキーワード</param>
        private void KeyWardShape(List<BlendShapeData> blendShapeDatas, string keyWard, List<string> notWard)
        {
            // ワードに引っかかったシェイプを入れるLIST
            List<BlendShapeData> shapeDatas = new List<BlendShapeData>();
            //shapeDatas.Clear();

            // シェイプの中から指定されたワードでランダム再生
            for (int s = 0; s < blendShapeDatas.Count; s++)
            {
                bool notWordFlg = false;

                // シェイプを１つずつ見ていってワードが一致するなら
                if (blendShapeDatas[s].name.Contains(keyWard))
                {
                    // NotWord検索 ヒット
                    for (int t = 0; t < notWard.Count; t++)
                    {
                        //もし含んでいけない文字列があるシェイプだったなら次のfor文へ
                        if (blendShapeDatas[s].name.Contains(notWard[t]))
                        {
                            notWordFlg = true;
                            continue;
                        }
                    }
                    if (notWordFlg)
                        continue;

                    // 再生用シェイプリストに格納しておく
                    if (!notWordFlg) shapeDatas.Add(blendShapeDatas[s]);
                }
            }

            // LISTの中からシェイプをランダム実行
            if (shapeDatas.Count != 0)
            {
                int ran = Random.Range(0, shapeDatas.Count);
                ShapeChange(shapeDatas[ran].BlendShapeList, _changeSpeed);
            }
        }
        /// <summary>
        /// 指定された表情にする為、各シェイプの目標値を設定する
        /// </summary>
        /// <param name="blendValues">切り替えたい表情の</param>
        /// <param name="init">シェイプキーを全て0にするか？</param>
        /// <param name="speed">表情を切り替えるスピードを受け取る</param>
        private void ShapeChange(List<BlandValue> blendValues, float speed)
        {
            // シェイプがなければ終了
            if (blendValues.Count == 0) return;

            // 初期化フラグがONなら全てのシェイプキーを0に設定
            //if (init) ShapeInit();

            //表情のシェイプに変更が必要か全て確認する
            for (int i = 0; i < shapeAllLength; i++)
            {
                // Bodyの現在要素のシェイプの値を保持
                float startValue = _skinnedMeshRenderer.GetBlendShapeWeight(i);
                // Bodyの切り替える設定値を初期値で設定
                float goalValue = 0;
                // 表情に必要なシェイプの値リストを確認していく 22, 45, 67 ...
                for (int j = 0; j < blendValues.Count; j++)
                {
                    // 今確認しているBodyのシェイプと一致するシェイプがあれば
                    if (mesh.GetBlendShapeName(i) == blendValues[j].ShapeName)
                    {
                        // 用意された値をゴールの値を設定しておく
                        goalValue = blendValues[j].ShapeValue;
                    }
                }
                // 現在の要素のシェイプキーをゴール値まで滑らかに切り替える
                StartCoroutine(ShapeLerp(i, startValue, goalValue, speed));
            }
        }

        /// <summary>
        /// 設定された目標値にシェイプを滑らかに切り替える
        /// </summary>
        /// <param name="no">Bodyの滑らかに切り替えたいシェイプNoを受け取る</param>
        /// <param name="startValue">シェイプの現在の値を受け取る</param>
        /// <param name="goalValue">シェイプの目標値を受け取る</param>
        /// <param name="speed">表情を切り替える速度を受け取る</param>
        /// <returns></returns>
        private IEnumerator ShapeLerp(int no, float startValue, float goalValue, float speed)
        {
            //Debug.Log($"no:{no}");
            // 滑らかに切り替えるLerp用の値
            float lerpValue = 0;
            // Lerpの値が1を超えるまで
            while (lerpValue <= 1)
            {
                // Lerpの第３引数用 開始位置(0)〜目標位置(1)へ滑らかに
                lerpValue += Time.deltaTime * speed;
                // Lerpの実行
                float shapeChangeVelue = Mathf.Lerp(startValue, goalValue, lerpValue);
                // Lerpの値をシェイプキーに格納
                _skinnedMeshRenderer.SetBlendShapeWeight(no, shapeChangeVelue);
                yield return null;
            }
        }
    }
}

