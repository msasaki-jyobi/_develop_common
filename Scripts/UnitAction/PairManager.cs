using Cinemachine;
using develop_easymovie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class PairManager : SingletonMonoBehaviour<PairManager>
    {
        public void PlayPair(UnitComponents unit, string key, string value)
        {
            Debug.Log($"Pair:{unit.name}, {key}, {value}");
            switch (key)
            {
                case "RandomCamera":
                    PlayRandomCamera(unit);
                    break;
                case "DefaultCamera":
                    develop_easymovie.CameraManager.Instance.SetDefaultCamera(false);
                    break;
            }
        }

        public void PlayRandomCamera(UnitComponents unit)
        {
            if (unit.BodyFreeLook == null) return;

            if (unit != null)
            {
                // UnitInstanceからBody取得
                var target = unit.UnitInstance.RandomBodys[UnityEngine.Random.Range(0, unit.UnitInstance.RandomBodys.Count)];
                if (target != null)
                {
                    // UnitComponentのFreeLookを設定
                    develop_easymovie.CameraManager.Instance.ChangeRandomCamera(target.transform, unit.BodyFreeLook);
                }
            }
        }

    }
}