using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    public GameObject SoundUISet;
    public Slider sfxSlider;
    public Slider bgmSlider;

    private void Start()
    {
        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;
        sfxSlider.value = SoundManager.Instance.sfxSource.volume;

        bgmSlider.minValue = 0f;
        bgmSlider.maxValue = 1f;
        bgmSlider.value = SoundManager.Instance.bgmSource.volume;

        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
    }

    private void OnSFXVolumeChanged(float sfxValue)
    {
        SoundManager.Instance.sfxSource.volume = sfxValue;
    }

    private void OnBGMVolumeChanged(float bgmValue)
    {
        SoundManager.Instance.bgmSource.volume = bgmValue;
    }

    public void OnSoundSetting()
    {
        if(PlayerStats.Instance != null)
        {
            PlayerStats.Instance.canPuase = false;
        }
        SoundUISet.SetActive(true);
    }

    public void OffSoundSetting()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.canPuase = true;
        }
        SoundUISet.SetActive(false);
    }
}
