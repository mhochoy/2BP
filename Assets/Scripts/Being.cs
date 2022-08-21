using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.Animations.Rigging;
public class Being : MonoBehaviour
{
    Animator animator;
    NavMeshAgent _agent;
    HandleRagdoll ragdoll;
    HandleNPCIKWeights _NPCIKWeights;
    Vector3 direction;
    CharacterController _controller;
    Inventory inventory;
    string nearestInteractable;
    public PlayerInput _input;
    public bool isActive;
    public int health;
    public bool isAI;
    [Range(0, 20)]
    public float speed;
    [Range(0, 20)]
    public float gravity;
    [SerializeField] private List<Rig> _NPCRigs;
    [SerializeField] private GameObject ItemTree; 
    
    void Awake() {
        isActive = true;
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
        TryGetComponent<PlayerInput>(out _input);
        if (_input) {
            _controller = GetComponent<CharacterController>();
        }
        if (!_input) {
            _agent = GetComponent<NavMeshAgent>();
            _NPCIKWeights = GetComponent<HandleNPCIKWeights>();
            ragdoll = GetComponent<HandleRagdoll>();
            ragdoll.DeactivateRagdoll();
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
            var item = inventory.current_item;
            bool _HasAnItemThatIsntTheDefaultOne = inventory.HasAnItem();
            Move(_input.x, _input.z);
            
            if (_HasAnItemThatIsntTheDefaultOne) {
                // Handle Use
                if (item.IsClickToUse()) {
                    Use(_input.shoot);
                }
                else {
                    Use(_input.shoot_held);
                }
            }
        }
    }

    void Update() {
        if (!isAI) {
            if (inventory && inventory.Count() > 1) {
                Swap(_input.switch_axis);
            }
            
            Reload(_input.reload);
        }
    }

    public virtual void Damage(int value) {
        if (health - value > 0) {
            health -= value;
        }
        else {
            // Handle death
            if (isAI) {
                
                Die();
            }
        }
    }

    void Die() {
        if (ItemTree) {
            ItemTree.SetActive(false);
        }
        if (isAI) {
            if (_NPCRigs.Count > 0) {
                foreach (Rig rig in _NPCRigs) {
                    rig.weight = 0;
                }
            }
            if (inventory) {
                inventory.DropItem();
            }
            _NPCIKWeights.Deactivate();
            animator.enabled = false;
            _agent.enabled = false;
            ragdoll.ActivateRagdoll();
            isActive = false;
            enabled = false;
        }
        else {

        }
    }

    public virtual void Move(float x, float y) {
        // Movement in the case of Player
        if (x != 0) {
            animator.SetFloat("x", x);
        }
        if (y != 0) {
            animator.SetFloat("y", y);
        }
        direction = transform.forward * y + transform.right * x;
        direction.y -= gravity;
        _controller.Move(direction * speed * Time.deltaTime);
    }

    public virtual void Move(Vector3 direction) {
        // Movement in the case of AI. Quick and Dirty
        Vector3 _position = transform.position;
        animator.SetFloat("x", _agent.velocity.x);
        animator.SetFloat("y", _agent.velocity.z);
        _position.y -= gravity;
        transform.position = _position;
        _agent.SetDestination(direction);
    }

    public virtual void Use(bool is_using) {
        if (is_using) {
            inventory.current_item.Use();
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

    public bool IsLockedOn() {
        return _NPCIKWeights.LockOnActive;
    }

    public bool IsLockedOn(string tag) {
        return _NPCIKWeights.target.tag == tag && IsLockedOn();
    }

    public bool HasInventory() {
        if (inventory) {
            return true;
        }

        return false;
    }

    public Transform GetTarget() {
        return _NPCIKWeights.target;
    }

    void OnTriggerEnter(Collider col) {
        ItemPickup pickup;
        col.gameObject.TryGetComponent<ItemPickup>(out pickup);

        if (pickup != null && !isAI) {
            pickup.PickedUp();
            inventory.GiveItem(pickup.item);
        }
    }

    public void AddItemToInventory(Item item) {
        if (HasInventory()) {
            inventory.GiveItem(item);
        }
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

    public void SetNearestInteraction(string name) {
        nearestInteractable = name;
    }

    public string GetNearestInteraction() {
        return nearestInteractable;
    }
}
