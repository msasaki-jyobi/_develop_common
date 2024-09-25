using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

namespace develop_common
{
    public class SavePointManager : SingletonMonoBehaviour<SavePointManager>
    {
        public string SaveName = "Stage1";
        public string PlayerName = "Player";

        public AudioClip SaveSE; // Save音
        public TextMeshProUGUI SaveTextGUI; // Save演出

        [SerializeField] private bool _isLoadSavePoint = true;

        private List<SavePoint> _savePoints = new List<SavePoint>();
        private GameObject _player;
        private int _loadPoint;
        private SavePoint _startSavePoint;

        private void Start()
        {
            if (!_isLoadSavePoint) return;

            UniTask.Delay(10);
            _loadPoint = PlayerPrefs.GetInt(SaveName, -1);
            if (_loadPoint != -1)
            {
                _player = GameObject.Find("Player");
                _startSavePoint = SearchSavePoint(_loadPoint);

                UniTask.Delay(10);
                if (_startSavePoint != null)
                {
                    if (_player != null)
                    {
                        _player.transform.position = _startSavePoint.transform.position;
                        _player.transform.rotation = _startSavePoint.transform.rotation;
                    }
                    else
                    {
                        Debug.LogError($"プレイヤーが存在しません. 検索対象:{PlayerName}");
                    }
                }
            }
        }

        public void AddSavePoint(SavePoint point)
        {
            _savePoints.Add(point);
        }

        public SavePoint SearchSavePoint(int id)
        {
            foreach (var point in _savePoints)
            {
                if (point.PointID == id)
                    return point;
            }

            Debug.LogError($"SavePoint:{id} は存在しません");
            return null;
        }

        public async void OnSave(int id)
        {
            PlayerPrefs.SetInt(SaveName, id);
            var message = $"{SaveName} : {id} セーブ完了しました";
            Debug.Log(message);

            AudioManager.Instance.PlayOneShot(SaveSE, EAudioType.Se);
            if (SaveTextGUI != null)
            {
                SaveTextGUI.text = message;
                await UniTask.Delay(1000);
                SaveTextGUI.text = "";
            }


        }
    }

}
