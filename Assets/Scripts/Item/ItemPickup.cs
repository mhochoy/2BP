using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum PickupType {
        Weapon,
        Health,
        Power
    }
    public PickupType ItemType;
    MeshRenderer mesh;
    public Item item;
    public int value;

    public void Start() {
        mesh = GetComponentInChildren<MeshRenderer>();
    }
    public void PickedUp() {
        mesh.enabled = false;
        gameObject.SetActive(false);
    }
}
