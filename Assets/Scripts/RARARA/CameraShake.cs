using System.Collections;
using UnityEngine;
using Cinemachine;
using JToolkit.Utility;
using Game.Management;

public class CameraShake : Singleton<CameraShake>
{/*
    private CinemachineBasicMultiChannelPerlin virtalCameraNoise;
    private CinemachineVirtualCamera virtalCam;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        virtalCam = PlayerManager.instance.game_camera.cv_camera;
        virtalCameraNoise = virtalCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Shake(3f, 1f, 1f);
        }
    }

    public void Shake(float _time, float _amplitude, float _frequency)
    {
        if (virtalCam != null && virtalCameraNoise != null)
        {
            virtalCameraNoise.m_AmplitudeGain = _amplitude;
            virtalCameraNoise.m_FrequencyGain = _frequency;
            StartCoroutine(CamNoise(_time));
        }
        else
        {
            Init();
            StartCoroutine(CamNoise(_time));
        }
    }

    private IEnumerator CamNoise(float _time)
    {
        if (virtalCam == null || virtalCameraNoise == null) yield break;

        yield return new WaitForSeconds(_time);

        virtalCameraNoise.m_AmplitudeGain = 0;
        virtalCameraNoise.m_FrequencyGain = 0;
    }
    */
}
