using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCam : MonoBehaviour
{
    public float ShakeDuration = 0.3f;
    public float ShakeAmplitude = 1.2f;
    public float ShakeFrequency = 2.0f;

    private float ShakeElapsedTime = 0f;

    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameranoise;

    // Start is called before the first frame update
    void Start()
    {
        if (virtualCamera != null)
        {
            virtualCameranoise = virtualCamera.GetCinemachineComponent < Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
    }

    public void Shake()
    {
        ShakeElapsedTime = ShakeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (virtualCamera != null || virtualCameranoise != null)
        {
            if (ShakeElapsedTime > 0)
            {
                virtualCameranoise.m_AmplitudeGain = ShakeAmplitude;
                virtualCameranoise.m_FrequencyGain = ShakeFrequency;

                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                virtualCameranoise.m_AmplitudeGain = 0f;
                ShakeElapsedTime = 0f;
            }
        }
    }
}
