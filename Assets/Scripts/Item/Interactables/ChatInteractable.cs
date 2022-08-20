using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatInteractable : Interactable
{
    public string chat_name;
    public List<string> dialogues;
    public int current_dialogue;
    public bool open;

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
}
