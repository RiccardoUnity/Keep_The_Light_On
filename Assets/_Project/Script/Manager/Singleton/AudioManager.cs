using System;
using System.Collections;
using UnityEngine;
using GWM = GameWorldManager;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private bool _isMyAwake;

    [SerializeField] private AudioClip _dawnDusk;
    private bool _isDawnDusk;
    [SerializeField] private AudioClip _music;

    private float _volume;
    private float _volumeMax;
    private float _volumeMaster;
    private float _volumeMusic;
    private float _volumeVFX;
    private AudioSource _audioSource;
    public event Action<float> onVomuneVFXChange;

    private IEnumerator _animation;
    private bool _isInAnimation;
    private float _currentTime;
    private float _timeCoroutine;
    private event Action _onAnimation;
    private event Action _onLateAnimation;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;
            _volumeMax = _audioSource.volume;
            _volumeMaster = GWM.Instance.UIPause.UIOption.VolumeMaster.value;
            _volumeMusic = GWM.Instance.UIPause.UIOption.VolumeMusic.value;
            _volumeVFX = GWM.Instance.UIPause.UIOption.VolumeVFX.value;
            SetVolumeMusic();

            GWM.Instance.UIPause.UIOption.VolumeMaster.onValueChanged.AddListener(ChangeVolumeMasterOption);
            GWM.Instance.UIPause.UIOption.VolumeMusic.onValueChanged.AddListener(ChangeVolumeMusicOption);
            GWM.Instance.UIPause.UIOption.VolumeVFX.onValueChanged.AddListener(ChangeVolumeVFXOption);
            GWM.Instance.TimeManager.onDawn += PlayOnDawnDusk;
            GWM.Instance.TimeManager.onDusk += PlayOnDawnDusk;
            GWM.Instance.TimeManager.onDay += PlayOnDayNight;
            GWM.Instance.TimeManager.onNight += PlayOnDayNight;

            if (GWM.Instance.TimeManager.DayTime == DayTime.Day || GWM.Instance.TimeManager.DayTime == DayTime.Night)
            {
                PlayOnDayNight();
            }
            else
            {
                PlayOnDawnDusk();
            }

            _onLateAnimation += SetVolumeVFX;
        }
    }

    private void PlayOnDayNight()
    {
        if (_animation == null)
        {
            _timeCoroutine = 5f;
            _onAnimation = VolumeDown;
            _onLateAnimation = PutAudioMusic;
            _animation = ChangeVolume();
            StartCoroutine(_animation);
        }
    }
    private void PlayOnDawnDusk()
    {
        if (_animation == null)
        {
            _timeCoroutine = 5f;
            _onAnimation = VolumeDown;
            _onLateAnimation = PutAudioDawnDusk;
            _animation = ChangeVolume();
            StartCoroutine(_animation);
        }
    }

    private void VolumeDown() => _audioSource.volume = _volume * (1f - _currentTime);
    private void VolumeUp() => _audioSource.volume = _volume * _currentTime;

    private void PutAudioMusic()
    {
        _audioSource.clip = _music;
        _timeCoroutine = 5f;
        _onAnimation = VolumeUp;
        _onLateAnimation = null;
        _audioSource.Play();
        StartCoroutine(ChangeVolume());
}
private void PutAudioDawnDusk()
    {
        _audioSource.clip = _dawnDusk;
        _timeCoroutine = 5f;
        _onAnimation = VolumeUp;
        _onLateAnimation = null;
        _audioSource.Play();
        StartCoroutine(ChangeVolume());
    }

    private IEnumerator ChangeVolume()
    {
        //Debug.Log("Start");
        _currentTime = 0f;
        if (_timeCoroutine == 0f)
        {
            _timeCoroutine = 1f;
        }

        _isInAnimation = true;
        while (_isInAnimation)
        {
            _currentTime += Time.deltaTime / _timeCoroutine;

            if (_currentTime > 1)
            {
                _currentTime = 1f;
                _isInAnimation = false;
            }

            _onAnimation?.Invoke();
            yield return null;
        }

        _animation = null;
        _onLateAnimation?.Invoke();
        //Debug.Log("End");
    }

    private void ChangeVolumeMasterOption(float value)
    {
        _volumeMaster = value;
        SetVolumeMusic();
        SetVolumeVFX();
    }

    private void ChangeVolumeMusicOption(float value)
    {
        _volumeMusic = value;
        SetVolumeMusic();
    }

    private void ChangeVolumeVFXOption(float value)
    {
        _volumeVFX = value;
        SetVolumeVFX();
    }

    private void SetVolumeMusic()
    {
        float lerpMusic = Mathf.Lerp(0f, _volumeMaster, _volumeMusic);
        _volume = Mathf.Lerp(0f, _volumeMax, lerpMusic);
        _audioSource.volume = _volume;
    }

    private void SetVolumeVFX()
    {
        float lerpVFX = Mathf.Lerp(0f, _volumeMaster, _volumeVFX);
        onVomuneVFXChange?.Invoke(lerpVFX);
    }
}
