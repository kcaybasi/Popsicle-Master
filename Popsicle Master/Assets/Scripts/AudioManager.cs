using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioSource drippingSound;
    private bool isMute;
    private bool isHapticEnabled;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isHapticEnabled = true;
    }

    public void PlayDrippingSound()
    {
        if (!drippingSound.isPlaying)
        {
            drippingSound.Play();
        }
        
    }

    public void Mute()
    {
        if (!isMute)
        {
            AudioListener.volume = 0;
            isMute = true;
        }
        else
        {
            AudioListener.volume = 1;
            isMute = false;
        }
    }

    public void Vibrate()
    {
        if (isHapticEnabled)
        {
            MMVibrationManager.Haptic(HapticTypes.SoftImpact);
        }
        
    }

    public void DisableHaptic()
    {
        if (isHapticEnabled)
        {
            isHapticEnabled = false;
        }
        else
        {
            isHapticEnabled = true;
        }
    }

}
