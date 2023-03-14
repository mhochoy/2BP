using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.Animations.Rigging;
using Cinemachine;

public class Being : MonoBehaviour
{
    Animator animator;
    NavMeshAgent _agent;
    HandleRagdoll ragdoll;
    HandleNPCIKWeights _NPCIKWeights;
    Vector3 direction;
    CharacterController _controller;
    Inventory inventory;
    [SerializeField] Interactable nearestInteractable;
    CinemachineImpulseSource impulse;
    float originalPitch;
    float originalVolume;
    public int points;
    public PlayerInput _input;
    public bool isActive;
    public int health;
    public bool enemy;
    public bool ItemBox;
    public bool isAI;
    [Range(0, 20)]
    public float speed;
    [Range(0, 20)]
    public float gravity;
    [SerializeField] private GameObject AggroTrigger;
    [SerializeField] private AudioSource BeingSound;
    [SerializeField] private AudioClip HEALTH_AUDIO_CLIP;
    [SerializeField] private List<AudioClip> HitSounds;
    [SerializeField] private AudioClip DeathSound;
    [SerializeField] private List<Rig> _NPCRigs;
    [SerializeField] private GameObject ItemTree; 
    [SerializeField] private GameObject CameraTarget;
    bool aggro;
    float originalSpeed;
    
    void Awake() {
        isActive = true;
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
        TryGetComponent<PlayerInput>(out _input);
        TryGetComponent<CinemachineImpulseSource>(out impulse);
        TryGetComponent<HandleRagdoll>(out ragdoll);
        Setup();
    }

    void Setup() {
        if (ragdoll) {
            ragdoll.DeactivateRagdoll();
        }
        if (BeingSound) {
            originalPitch = BeingSound.pitch;
            originalVolume = BeingSound.volume;
        }
        if (_input) {
            _controller = GetComponent<CharacterController>();
        }
        if (!_input) {
            _agent = GetComponent<NavMeshAgent>();
            _NPCIKWeights = GetComponent<HandleNPCIKWeights>();
            ragdoll = GetComponent<HandleRagdoll>();

            if (_agent) {
                originalSpeed = _agent.speed;
            }
            if (_NPCIKWeights) {
                SetTarget(null);
            }
            if (ragdoll) {
                ragdoll.DeactivateRagdoll();
            }
        }
        else {
            return;
        }
    }

    void FixedUpdate()
    {
        // Edge case
        if (!_input && !_agent || !isActive) {
            return;
        }
        // Is a human in control or is this an npc?
        if (!_input) {
            isAI = true;
        } else {
            isAI = false;
        }
        // Handle human movement
        if (!isAI) {
            Move(_input.x, _input.z);
        }
    }

    void Update() {
        if (!_input) {
            return;
        }
        
        if (!isAI) {
            var item = inventory.current_item;
            bool _HasAnItemThatIsntTheDefaultOne = inventory.HasAnItem();
            
            
            if (_HasAnItemThatIsntTheDefaultOne) {
                // Handle Use
                if (item.IsClickToUse()) {
                    Use(_input.shoot);
                }
                else {
                    Use(_input.shoot_held);
                }
            }

            if (inventory && inventory.Count() > 1) {
                Swap(_input.switch_axis);
            }
            
            Reload(_input.reload);
        }
        else {
            
        }
    }

    public void GiveHealth(int value) {
        if (BeingSound) {
            BeingSound.PlayOneShot(HEALTH_AUDIO_CLIP);
        }
        if (health + value > 100 && !isAI) {
            health = 100;
        }
        else {
            health += value;
        }
    }

    public virtual void Damage(int value) {
        if (health - value > 0) {
            health -= value;
            if (!aggro) {
                aggro = true;
            }
            if (BeingSound) {
                int decision = UnityEngine.Random.Range(0, HitSounds.Count + 2);
                if (decision < HitSounds.Count) {
                    PlayEditedBeingSound(HitSounds[decision]);
                }
            }
            if (impulse) {
                impulse.GenerateImpulse();
            }
        }
        else {
            health = 0;
            if (impulse) {
                impulse.m_DefaultVelocity.y = impulse.m_DefaultVelocity.y * 4;
                impulse.GenerateImpulse();
            }
            Die();
        }
    }

    void Die() {
        if (BeingSound) {
            PlayEditedBeingSound(DeathSound);
        }
        if (ItemTree) {
            ItemTree.SetActive(false);
        }
        if (inventory && inventory.HasDroppableItem()) {
            inventory.DropItem();
        }
        if (isAI) {
            if (_NPCRigs.Count > 0) {
                foreach (Rig rig in _NPCRigs) {
                    rig.weight = 0;
                }
            }
            if (_NPCIKWeights) {
                _NPCIKWeights.Deactivate();
            }
            if (_agent) {
                _agent.enabled = false;
            }
        }
        else if (!isAI) {
            if (CameraTarget) {
                CameraTarget.transform.parent = null;
            }
            _controller.enabled = false;
        }
        
        if (ragdoll) {
            ragdoll.ActivateRagdoll();
        }
        if (animator) {
            animator.enabled = false;
        }
        isActive = false;
        if (ItemBox) {
            gameObject.SetActive(false);
        }
        enabled = false;
    }

    public virtual void Move(float x, float y) {
        // Movement in the case of Player
        if (x != 0) {
            animator.SetFloat("x", x);
        }
        if (y != 0) {
            animator.SetFloat("y", y);
        }
        direction = transform.forward * y + transform.right * (x/2);
        direction.y -= gravity;
        _controller.Move(direction * speed * Time.deltaTime);
    }

    public virtual void Move(Vector3 direction, bool walk = false) {
        // Movement in the case of AI. Quick and Dirty
        Vector3 _position = transform.position;

        animator.SetFloat("x", _agent.velocity.x);
        animator.SetFloat("y", _agent.velocity.z);
        _position.y -= gravity;
        transform.position = _position;
        _agent.SetDestination(direction);
    }

    public virtual void Use(bool is_using) {
        bool _HasAnItemAndItIsAWeapon = (inventory.current_item && inventory.current_item is Weapon);

        if (is_using) {
            inventory.current_item.Use();
            if (_HasAnItemAndItIsAWeapon && !isAI) {
                GameObject aggrotrig = Instantiate(AggroTrigger, transform.position, transform.rotation);
                SphereCollider spherecol = aggrotrig.GetComponent<SphereCollider>();

                spherecol.enabled = true;
            }
        }
    }

    public virtual void Reload(bool reload) {
        try {
            Weapon weapon = (Weapon)inventory.current_item;

            if (reload && weapon.Clips > 0) {
                weapon.Reload();
            }
        }
        catch (InvalidCastException) {
            // The item is not a weapon
            return;
        }
    }

    public void Swap(float scroll) {
        if (inventory) {
            inventory.Swap(scroll);
        }
    }

    public bool HasInventory() {
        if (inventory) {
            return true;
        }

        return false;
    }

    public void EnableLockOn() {
        _NPCIKWeights.EnableLook();
    }

    public void DisableLockOn() {
        _NPCIKWeights.DisableLook();
    }

    public void SetTarget(Transform target) {
        _NPCIKWeights.SetTarget(target);
    }

    void OnTriggerEnter(Collider col) {
        ItemPickup pickup;
        col.gameObject.TryGetComponent<ItemPickup>(out pickup);

        // Layer 2 is AggroTriggers
        if (gameObject.layer == 2) {
            return;
        }

        if (pickup != null && !isAI) {
            switch (pickup.ItemType) {
                case ItemPickup.PickupType.Health:
                    if (health < 100) {
                        pickup.PickedUp();
                        GiveHealth(pickup.value);
                    }
                    break;
                case ItemPickup.PickupType.Weapon:
                    pickup.PickedUp();
                    inventory.GiveItem(pickup.item);
                    break;
                default:
                    pickup.PickedUp();
                    break;
            }
        }
    }

    public bool HasAnItem() {
        if (inventory) {
            return inventory.HasAnItem();
        }
        return false;
    }

    public void AddItemToInventory(Item item) {
        if (HasInventory()) {
            inventory.GiveItem(item);
        }
    }

    public void AddScore(int value) {
        points += value; 
    }

    public string GetItemStats() {
        bool IHaveAnItemAndItIsAWeapon = (inventory.current_item && inventory.current_item is Weapon);
        bool IHaveAnItemAndItsARegularItem = (inventory.current_item && inventory.current_item is Item);
        if (IHaveAnItemAndItIsAWeapon) {
            Weapon weapon = (Weapon)inventory.current_item;
            return $"{weapon.Bullets} / {weapon.Clips} - {weapon.item_name}";
        }
        if (IHaveAnItemAndItsARegularItem) {
            return $"{inventory.current_item.item_name}";
        }
        return "";
    }

    public void SetNearestInteraction(Interactable interactable) {
        nearestInteractable = interactable;
    }

    public Interactable GetNearestInteraction() {
        return nearestInteractable;
    }

    protected void PlayEditedBeingSound(AudioClip sound) {
        BeingSound.pitch = UnityEngine.Random.Range(originalPitch - .075f, originalPitch + .075f);
        BeingSound.volume = UnityEngine.Random.Range(originalVolume - .075f, originalVolume + .075f);
        BeingSound.clip = sound;
        BeingSound.Play();
    }

    public bool IsAggro() {
        return aggro;
    }

    public void SetAggro(bool state) {
        aggro = state;
    }
}
