using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public int Damage;
    public int MaxBullets;
    public int Clips;
    public int Range;
    public int Force;
    public int Bullets {get; private set;}
    [SerializeField] private float fire_rate;
    [SerializeField] private AudioClip _ReloadSound;
    [SerializeField] private GameObject _HitEffect;
    [SerializeField] private LayerMask _PlayerLayer;
    GameObject _hit_effect;
    ParticleSystem _hit_particle;
    float last_fired;

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

    public override void Use()
    {
        if (Bullets > 0) {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            RaycastHit hit;
            
            if (ClickToUse) {
                base.Use();
                SendBulletRaycast(ray, out hit);
                Bullets--;
                return;
            }
            last_fired += Time.deltaTime;
        
            if (last_fired > fire_rate && !ClickToUse) {
                last_fired = 0;
                SendBulletRaycast(ray, out hit);
                base.Use();
                Bullets--;
            }
        }
    }

    public void SendBulletRaycast(Ray ray, out RaycastHit hit) {
        if (Physics.Raycast(ray, out hit, Range, ~_PlayerLayer)) {
            Being _being;
            hit.transform.TryGetComponent<Being>(out _being);

            Hit(_being, hit);
            PlayHitEffect(hit.point, hit.normal);
        }
    }

    void Hit(Being being, RaycastHit hit) {
        if (hit.rigidbody) {
            hit.rigidbody.AddForceAtPosition(-hit.transform.forward * Force, hit.point, ForceMode.Impulse);
        }
        if (being && being.isActive) {
            being.Damage(Damage);
        }
    }

    void PlayHitEffect(Vector3 point, Vector3 normal) {
        if (_HitEffect) {
            _hit_effect.transform.position = point;
            _hit_effect.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
            _hit_particle.Play();
        }
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
