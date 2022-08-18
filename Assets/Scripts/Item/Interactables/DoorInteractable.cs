using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    public bool locked {get ; private set;}
    Animator animator;
    AudioSource sound;
    AudioClip open;
    AudioClip close;

    void Start() {
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }

    public override void Activate()
    {
        base.Activate();
        if (!locked) {
            animator.Play("open");
            sound.PlayOneShot(open);
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        animator.Play("close");
        sound.PlayOneShot(close);
    }
}
