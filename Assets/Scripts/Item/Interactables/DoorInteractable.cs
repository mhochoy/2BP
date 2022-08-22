using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    public bool locked {get ; private set;}
    Animator animator;
    void Start() {
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }

    protected override void Activate()
    {
        base.Activate();
        if (!locked) {
            animator.Play("open");
        }
    }

    protected override void Deactivate()
    {
        base.Deactivate();
        animator.Play("close");
    }
}
