using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
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