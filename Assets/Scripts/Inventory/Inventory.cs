using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    Being being;
    [SerializeField] private GameObject _DroppableItem;
    [SerializeField] private Animator _RigAnimator;
    [SerializeField] private List<Item> items;
    [SerializeField] private Transform items_parent;
    public Item current_item;
    int internal_item_index = 0;
    bool done;

    public void Awake() 
    {
        being = GetComponent<Being>();
    }

    void FixedUpdate()
    {
        // Disable whatever item was selected when the being was deactivated
        if (!being.isActive && !done) {
            done = true;
            try {
                Item _selected_item = items[internal_item_index];
                _selected_item.gameObject.SetActive(false);
            }
            catch (ArgumentOutOfRangeException) {
                // Pass
            }
            return;
        }
        if (being.isActive) {
            // Index Bounding
            if (internal_item_index >= items.Count) {
                internal_item_index = 0;
            }
            if (internal_item_index < 0) {
                internal_item_index = 0;
            }

            // Set the current Item
            Item selected_item = items[internal_item_index];
            if (selected_item) {
                selected_item.gameObject.SetActive(true);
                current_item = selected_item;
            }

            // Sync Animator to inventory
            if (current_item) {
                SetSelectedItem(current_item.item_name);
            }
            
            // Disable all non-selected items
            foreach (Item item in items) {
                bool _ThereIsAnItemAndItIsActiveAndEnabled = (item && item.enabled && item.gameObject.activeSelf);
                if (_ThereIsAnItemAndItIsActiveAndEnabled) {
                    if (item.item_name != current_item.item_name) {
                        item.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void Swap(float value) {
        if (value > 0f) {
            internal_item_index++;
        }
        if (value < 0f) {
            internal_item_index--;
        }
    }

    public void SetSelectedItem(string item_name) {
        if (_RigAnimator) {
            _RigAnimator.Play("item_" + item_name);
        }
    }



    public bool HasItem(string _item_name) {
        foreach (Item _item in items) {
            if (_item.item_name == _item_name) {
                return true;
            }
        }
        return false;
    }

    Item GetItem(string _item_name) {
        foreach (Item _item in items) {
            if (_item.item_name == _item_name) {
                return _item;
            }
        }
        return null;
    }

    public void RemoveItem(string _item_name) {
        foreach (Item _item in items) {
            if (_item.item_name == _item_name) {
                items.Remove(_item);
            }
        }
    }

    public void GiveItem(Item item) {
        if (item) {
            if (Count() <= 1) {
                current_item = item;
            }
            if (HasItem(item.item_name)) {
                Item _item = GetItem(item.item_name);
                if (_item is Weapon) {
                    Weapon weapon = (Weapon)_item;
                    weapon.GiveAmmo(1);
                }
            }
            item.transform.parent = items_parent;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            items.Add(item);
        }
    }

    public int Count() {
        return items.Count;
    }

    public bool HasAnItem() {
        return (current_item && current_item.item_name != "none");
    }

    public void DropItem() {
        if (UnityEngine.Random.Range(0f, 1f) >= .3f && _DroppableItem) {
            Instantiate(_DroppableItem, transform.position+new Vector3(0f,1.5f,0f), transform.rotation);
        }
    }
}
