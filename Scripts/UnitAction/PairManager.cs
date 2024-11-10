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
                    PlayRandomCamera(unit, value);
                    break;
                case "DefaultCamera":
                    develop_easymovie.CameraManager.Instance.SetDefaultCamera(false);
                    break;
            }
        }

        public void PlayRandomCamera(UnitComponents unit, string keyName)
        {
            if (unit != null)
            {
                var target = unit.UnitInstance.SearchObject(keyName);
                if (target != null)
                    develop_easymovie.CameraManager.Instance.ChangeRandomCamera(target.transform, unit.UnitInstance.RandomCameras);
            }
        }


    }
}