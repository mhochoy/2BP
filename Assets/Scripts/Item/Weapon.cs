using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Weapon : Item
{
    public int Damage;
    public int MaxBullets;
    public int Clips;
    public int Range;
    public int Force;
    public int Bullets {get; private set;}
    [Range(0,100)]
    [SerializeField] private int AIHitChance;
    [SerializeField] private bool Infinity;
    [SerializeField] private float fire_rate;
    [SerializeField] private GameObject _AggroTrigger;
    [SerializeField] private AudioClip _ReloadSound;
    [SerializeField] private GameObject _HitEffect;
    [SerializeField] private LayerMask _PlayerLayer;
    [SerializeField] private AudioClip _HitSound;
    [SerializeField] private Transform _FirePoint;
    GameObject _hit_effect;
    ParticleSystem _hit_particle;
    float last_fired;
    float _DistanceFromPlayer;
    Vector3 LastRaycastHit;

    public override void Awake()
    {
        base.Awake();
    }
    void Start() {
        if (_HitEffect) {
            _hit_effect = Instantiate(_HitEffect);
            _hit_particle = _hit_effect.GetComponent<ParticleSystem>();
        }
        Bullets = MaxBullets;
    }

    void Update() {
        _DistanceFromPlayer = UnityEngine.Random.Range(0, Vector3.Distance(transform.position, LastRaycastHit));
    }

    public override void Use()
    {
        if (Bullets > 0) {            
            Fire();
        }
    }

    void Fire() {
        Ray ray = new Ray();
        if (!_FirePoint) {
            ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        }
        else if (_FirePoint) {
            ray = new Ray(_FirePoint.position, _FirePoint.forward);
        }
        RaycastHit hit;
        last_fired += Time.deltaTime;

        if (ClickToUse) {
            SendBulletRaycast(ray, out hit);
            base.Use();
            if (Infinity) {
                return;
            }
            Bullets--;
        }
        else if (last_fired > fire_rate && !ClickToUse) {
            last_fired = 0;
            SendBulletRaycast(ray, out hit);
            base.Use();
            if (Infinity) {
                return;
            }
            Bullets--;
        }
    }

    public void SendBulletRaycast(Ray ray, out RaycastHit hit) {
        if (Physics.Raycast(ray, out hit, Range, _FirePoint ? _PlayerLayer : ~_PlayerLayer)) {
            Being _being;
            hit.transform.TryGetComponent<Being>(out _being);
            if (!_being) {
                hit.transform.GetComponentInParent<Being>();
            }
            if (hit.rigidbody) {
                hit.rigidbody.AddForceAtPosition(-hit.transform.forward * Force, hit.point, ForceMode.Impulse);
            }
            if (_FirePoint) {
                // AI Shooting
                float decision = _DistanceFromPlayer / 2;
                Debug.Log($"Distance: {_DistanceFromPlayer}, Decision: {decision}");
                if (decision <= AIHitChance) {
                    if (_HitSound) {
                        Invoke("PlayHitSound", .075f);
                    }
                    if (_being) {
                        _being.Damage(Damage);
                        SpawnHitEffect(hit.point, hit.normal);
                    }
                }
                return;
            }
            else {
                // Player shooting
                if (_HitSound) {
                    Invoke("PlayHitSound", .075f);
                }
                SpawnHitEffect(hit.point, hit.normal);
            }

            if (_being && _being.isActive) {
                if (_HitSound) {
                    Invoke("PlayHitSound", .075f);
                }
                _being.Damage(Damage);
            }
        }
    }

    void SpawnHitEffect(Vector3 point, Vector3 normal) {
        if (_HitEffect) {
            _hit_effect.transform.position = point;
            _hit_effect.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
            _hit_particle.Play();
        }
    }

    void PlayHitSound() {
        PlayEditedItemSound(_HitSound);
    }

    public void GiveAmmo(int amount) {
        Clips += amount;
    }

    public void Reload() {
        if (Clips > 0 && Bullets != MaxBullets) {
            PlayItemSound(_ReloadSound);
            Clips--;
            this.Bullets = MaxBullets;
        }
    }
}
