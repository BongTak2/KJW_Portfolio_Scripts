using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    #region SoundManager ΩÃ±€≈Ê
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    #endregion

    public AudioSource bgSound;
    public AudioSource effectSound;
    public AudioMixer audioMixer;

    private void Update()
    {
        //audioMixer.SetFloat("BGM", Mathf.Log10(Sound.bgmVolume) * 20 + 5);
        //audioMixer.SetFloat("SFX", Mathf.Log10(Sound.sfxVolume) * 20 + 5);
        bgSound.volume = Mathf.Clamp(Sound.bgmVolume, 0f, 1f);
        effectSound.volume = Mathf.Clamp(Sound.bgmVolume, 0f, 1f);

    }

    public void BGMPlay(AudioClip audioClip)
    {
        bgSound.clip = audioClip;
        bgSound.loop = true;
        bgSound.Play();
    }

    public void BGMStop()
    {
        bgSound.Stop();
    }

    public void SFXStop()
    {
        effectSound.Stop();
    }

    public void EffectSoundPlay(AudioClip audioClip)
    {
        effectSound.PlayOneShot(audioClip);
    }

    public void SetBGMVolume(float volume)
    {
        Sound.bgmVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        Sound.sfxVolume = volume;
    }
}
