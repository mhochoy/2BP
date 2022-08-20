using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : Interactable
{
    [SerializeField] Item item;
    public override void Activate()
    {
        base.Activate();
        
        if (item) {
            GivePlayer(item);
        }
        gameObject.SetActive(false);
    }
}
