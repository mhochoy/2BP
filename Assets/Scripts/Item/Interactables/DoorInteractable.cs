using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    public bool locked {get ; private set;}
    Animator animator;
    AudioClip open;
    AudioClip close;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public override void Activate()
    {
        base.Activate();
        if (!locked) {
            animator.Play("open");
            base.sound.PlayOneShot(open);
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        animator.Play("close");
        base.sound.PlayOneShot(close);
    }
}
