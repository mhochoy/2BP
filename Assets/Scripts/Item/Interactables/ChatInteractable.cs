using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatInteractable : Interactable
{
    public string chat_name;
    public List<string> dialogues;
    int current_dialogue;
    bool open;
    Being npc;
    BoxCollider _collider;

    void Start() {
        npc = GetComponentInParent<Being>();
    }

    public override void Activate()
    {
        base.Activate();
        open = true;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        open = false;
    }

    public override void Check()
    {
        base.Check();
        if (npc.health <= 0) {
            transform.gameObject.SetActive(false);
        }
        else {
            transform.gameObject.SetActive(true);
        }
    }
}
