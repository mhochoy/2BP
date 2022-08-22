using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatInteractable : Interactable
{
    public Being npc;
    public List<string> dialogues;
    int current_dialogue;

    void Start() {
    }

    protected override void Update() {
        base.Update();
    }

    protected override void Activate()
    {
        base.Activate();
    }

    protected override void Deactivate()
    {
        base.Deactivate();
    }

    protected override void Check()
    {
        base.Check();
    }
}
