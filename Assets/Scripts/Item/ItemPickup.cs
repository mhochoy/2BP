using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    MeshRenderer mesh;
    public Item item;

    public void Start() {
        mesh = GetComponentInChildren<MeshRenderer>();
    }
    public void PickedUp() {
        mesh.enabled = false;
        gameObject.SetActive(false);
    }
}
