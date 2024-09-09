using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace GameSet.Common
{
    public class TimerManager : MonoBehaviour
    {
        public ReactiveProperty<float> GameTimer { get; private set; } = new ReactiveProperty<float>();

        [SerializeField] private TextMeshProUGUI _timerTextGUI;
        [SerializeField] private TextMeshProUGUI _completeTextGUI;

        [SerializeField] private float __completeTime = 30f;
        [SerializeField] private string _prefix = "Timer: ";
        [SerializeField] private string _suffix = " Seconds";
        [SerializeField] private string _completeMessage = "Game Hello!!";
        [SerializeField] private int __completeEventDelay = 3000;
        [SerializeField] private UnityEvent _completeEvent;

        private bool _complete = false;
        private bool _isStop = false;

        private void Start()
        {
            GameTimer.Value = __completeTime;

            if (_completeTextGUI != null)
                _completeTextGUI.text = "";

            GameTimer
                .Subscribe(async (x) =>
                {
                    if (_isStop) return;

                    if (!_complete)
                    {
                        if (x <= 0)
                        {
                            _complete = true;
                            GameTimer.Value = 0;
                            if (_completeTextGUI != null)
                                _completeTextGUI.text = _completeMessage;

                            await UniTask.Delay(__completeEventDelay);
                            _completeEvent?.Invoke();
                        }
                    }
                });
        }

        private void Update()
        {
            if (_isStop) return;

            if (GameTimer.Value > 0)
            {
                GameTimer.Value -= Time.deltaTime;

                if (_timerTextGUI != null)
                    _timerTextGUI.text = $"{_prefix}{GameTimer.Value.ToString("F2")}{_suffix}";
            }
        }

        public void OnActiveChangeTimer(bool active) => _isStop = active;
    }
}

