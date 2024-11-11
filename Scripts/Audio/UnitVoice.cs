using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace develop_common
{

    public class UnitVoice : MonoBehaviour
    {
        public float VoiceSpan = 0.2f;
        public List<VoiceKind> VoiceClips = new List<VoiceKind>();

        private float _voiceTimer;

        private void Update()
        {
            if (_voiceTimer > 0) { _voiceTimer -= Time.deltaTime; }
        }

        public void PlayVoice(string keyName, bool ignoreTimer = false)
        {
            if (!ignoreTimer)
                if (_voiceTimer > 0) return;

            foreach (var voice in VoiceClips)
            {
                if (keyName == voice.ClipName)
                {
                    _voiceTimer = VoiceSpan;
                    AudioManager.Instance.PlayOneShotClipData(voice.ClipData);
                }
            }
        }
    }
}

