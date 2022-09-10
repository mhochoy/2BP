using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : Interactable
{
    [SerializeField] GameObject Interactable;
    [SerializeField] Item item;
    [SerializeField] int value;

    void Start() {
        sound = GetComponent<AudioSource>();
    }
    protected override void Activate()
    {
        base.Activate();
        Being being = base.GetBeing();
        
        if (item) {
            GivePlayer(item);
            if (value > 0) {
                being.AddScore(value);
            }
        }
        Interactable.SetActive(false);
        Invoke("DisableMainObject", 3f);
    }

    void DisableMainObject() {
        gameObject.SetActive(false);
    }
}
