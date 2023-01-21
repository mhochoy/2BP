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

    protected override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
    }

    protected override void OnTriggerStay(Collider col)
    {
        base.OnTriggerStay(col);
    }

    protected override void OnTrigger()
    {
        Being _being = null;
        base.OnTrigger();
        if (!base.being) {
            _being = base.GetBeing();
        }
        if (item) {
            GivePlayer(item);
        }
        if (value > 0 && base.being) {
            base.being.AddScore(value);
        }
    }

    protected override void Activate()
    {
        
        base.Activate();
        Interactable.SetActive(false);
        Invoke("DisableMainObject", 3f);
    }

    void DisableMainObject() {
        gameObject.SetActive(false);
    }
}
