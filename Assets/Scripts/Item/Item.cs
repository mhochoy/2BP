using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string item_name;
    public ParticleSystem UseEffect;
    public Transform l_handle;
    public Transform r_handle;
    [SerializeField] protected AudioClip _UseSound;
    [SerializeField] protected Transform _EffectPoint;
    [SerializeField] protected bool ClickToUse;
    AudioSource _audio;
    float originalVolume;
    float originalPitch;


    void SetupSound() {
        if (_audio) {
            originalPitch = _audio.pitch;
            originalVolume = _audio.volume;
            _audio.clip = _UseSound;
        }
    }

    public virtual void Awake() {
        TryGetComponent<AudioSource>(out _audio);

        SetupSound();
    }

    public void FixedUpdate() {
        if (UseEffect) {
            UseEffect.transform.position = _EffectPoint.position;
        }
    }
    public virtual void Use() {
        PlayItemEffect();
        PlayItemSound();
    }

    public bool IsClickToUse() {
        return ClickToUse;
    }

    void PlayItemEffect() {
        if (UseEffect) {
            UseEffect.Play();
        }
    }

    void PlayItemSound() {
        if (_audio) {
            _audio.pitch = Random.Range(originalPitch - .075f, originalPitch + .075f);
            _audio.volume = Random.Range(originalVolume - .075f, originalVolume + .075f);
            _audio.Play();
        }
    }

    protected void PlayItemSound(AudioClip sound) {
        _audio.PlayOneShot(sound);
    }
}
