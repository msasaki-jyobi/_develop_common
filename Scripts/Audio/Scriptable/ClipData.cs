using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSet.Common
{
    public enum EAudioType
    {
        Se,
        Bgm,
        Voice
    }
    public class ClipData : ScriptableObject
    {
        public List<AudioClip> AudioClips;
        public EAudioType AudioType;
    }
}