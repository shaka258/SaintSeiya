using UnityEngine;
using System.Collections.Generic;

namespace SaintSeiya.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("Settings")]
        [Range(0f, 1f)] public float bgmVolume = 0.7f;
        [Range(0f, 1f)] public float sfxVolume = 1.0f;

        [Header("BGM Clips")]
        public AudioClip mainMenuBGM;
        public AudioClip battleBGM;
        public AudioClip fieldBGM;

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlayBGM(AudioClip clip, bool loop = true)
        {
            if (_bgmSource.clip == clip) return;
            _bgmSource.clip = clip;
            _bgmSource.loop = loop;
            _bgmSource.volume = bgmVolume;
            _bgmSource.Play();
        }

        public void StopBGM() => _bgmSource.Stop();

        public void PlaySFX(AudioClip clip)
        {
            _sfxSource.PlayOneShot(clip, sfxVolume);
        }

        public void SetBGMVolume(float vol)
        {
            bgmVolume = Mathf.Clamp01(vol);
            _bgmSource.volume = bgmVolume;
        }

        public void SetSFXVolume(float vol)
        {
            sfxVolume = Mathf.Clamp01(vol);
        }
    }
}
