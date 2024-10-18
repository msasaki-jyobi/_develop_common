using UnityEngine;
using TMPro; // TextMeshPro�p
using UniRx;
using UniTask = Cysharp.Threading.Tasks.UniTask; // UniTask�p
using DG.Tweening;
using System.Threading;
using System; // DOTween�p


namespace develop_common
{

    public class TextFadeController : SingletonMonoBehaviour<TextFadeController>
    {
        public TextMeshProUGUI MilestroneTextUGUI;
        public TextMeshProUGUI MessageTextUGUI;
        public float fadeDuration = 1f; // A�b�����ăt�F�[�h���鎞��
        public float waitDuration = 2f; // B�b�o�ߌ�ɍăt�F�[�h�A�E�g
        private Tween fadeTween; // DOTween��Tween�I�u�W�F�N�g�i�L�����Z���p�j
        private CancellationTokenSource cancellationTokenSource; // UniTask�p�̃L�����Z���g�[�N��

        void Start()
        {
            if (MilestroneTextUGUI != null)
            {
                // Q�L�[�������ꂽ�Ƃ��̏�����UniRx�ŊĎ�
                Observable.EveryUpdate()
                    .Where(_ => Input.GetKeyDown(KeyCode.Q))
                    .Subscribe(_ => OnQKeyPressed(MilestroneTextUGUI))
                    .AddTo(this); // �I�u�W�F�N�g���j�������Ƃ��Ɏ����I�ɍw�ǉ���
            }
        }

        private async void OnQKeyPressed(TextMeshProUGUI targetTextUGUI)
        {
            var defaultColor = Color.white;
            defaultColor.a = 0;
            targetTextUGUI.color = defaultColor;


            // �O��̃A�j���[�V�������L�����Z������
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            // �A���t�@�l��1�ɂ���Tween�̊J�n�i�r���ŐV����Q�L�[����������ƁA���݂�Tween���L�����Z���j
            fadeTween?.Kill();
            fadeTween = targetTextUGUI.DOFade(1f, fadeDuration).SetEase(Ease.Linear);

            // �A�j���[�V�����������B�b�ҋ@
            try
            {
                await UniTask.Delay((int)(waitDuration * 1000), cancellationToken: cancellationTokenSource.Token);

                // �ҋ@��ɃA���t�@�l��0�ɖ߂��A�j���[�V����
                fadeTween?.Kill();
                fadeTween = targetTextUGUI.DOFade(0f, fadeDuration).SetEase(Ease.Linear);
            }
            catch (OperationCanceledException)
            {
                // �L�����Z�������������ꍇ�̏����i�������Ȃ��ŏI���j
            }
        }

        private void OnDestroy()
        {
            // �I�u�W�F�N�g���j�������ۂɃ��\�[�X�����
            cancellationTokenSource?.Cancel();
        }

        // ����I����
        public void UpdateMileStone(string text)
        {
            MilestroneTextUGUI.text = $"�ڕW�F{text}";
            OnQKeyPressed(MilestroneTextUGUI);
        }
        public void UpdateMessageText(string text)
        {
            MessageTextUGUI.gameObject.SetActive(true);
            MessageTextUGUI.text = $"{text}";
            OnQKeyPressed(MessageTextUGUI);
        }
    }
}