using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Item : MonoBehaviour
{
    public string item_name;
    public ParticleSystem UseEffect;
    public Transform l_handle;
    public Transform r_handle;
    [SerializeField] protected AudioClip _UseSound;
    [SerializeField] protected Transform _EffectPoint;
    [SerializeField] protected bool ClickToUse;
    CinemachineImpulseSource recoil;
    AudioSource _audio;
    float originalVolume;
    float originalPitch;
    public virtual void Awake() {
        TryGetComponent<CinemachineImpulseSource>(out recoil);
        TryGetComponent<AudioSource>(out _audio);

        if (_audio) {
            originalPitch = _audio.pitch;
            originalVolume = _audio.volume;
            _audio.clip = _UseSound;
        }
    }

    public void FixedUpdate() {
        if (UseEffect) {
            UseEffect.transform.position = _EffectPoint.position;
        }
    }
    public virtual void Use() {
        if (_audio) {
            _audio.pitch = Random.Range(originalPitch - .075f, originalPitch + .075f);
            _audio.volume = Random.Range(originalVolume - .075f, originalVolume + .075f);
            _audio.Play();
        }
        if (UseEffect) {
            UseEffect.Play();
        }
        if (recoil) {
            recoil.GenerateImpulse();
        }
    }

    public bool IsClickToUse() {
        return ClickToUse;
    }

    protected void PlayItemSound(AudioClip sound) {
        _audio.PlayOneShot(sound);
    }
}
