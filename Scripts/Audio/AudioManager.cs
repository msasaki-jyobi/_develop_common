using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        [SerializeField] private AudioSource _seAudio;
        [SerializeField] private AudioSource _bgmAudio;
        [SerializeField] private AudioSource _voiceAudio;
        [SerializeField] private List<AudioClip> _debugSEs = new List<AudioClip>();

        public void PlayOneShot(AudioClip clip, EAudioType audioType)
        {
            if (clip == null) return;

            switch (audioType)
            {
                case EAudioType.Se:
                    _seAudio.PlayOneShot(clip);
                    break;
                case EAudioType.Bgm:
                    _bgmAudio.PlayOneShot(clip);
                    break;
                case EAudioType.Voice:
                    _voiceAudio.PlayOneShot(clip);
                    break;
            }
        }

        public void PlayOneShotClipData(ClipData clip)
        {
            if (clip == null) return;
            if (clip.AudioClips.Count == 0) return;

            int ran = Random.Range(0, clip.AudioClips.Count);

            switch (clip.AudioType)
            {
                case EAudioType.Se:
                    _seAudio.PlayOneShot(clip.AudioClips[ran]);
                    break;
                case EAudioType.Bgm:
                    _bgmAudio.clip = clip.AudioClips[ran];
                    _bgmAudio.Play();
                    break;
                case EAudioType.Voice:
                    _voiceAudio.PlayOneShot(clip.AudioClips[ran]);
                    break;
            }
        }

        public void OnChangeSEPitch(float value)
        {
            _seAudio.pitch = value;
        }

        public void OnDebugSE(int num)
        {
            PlayOneShot(_debugSEs[num], EAudioType.Se);
        }
    }
}