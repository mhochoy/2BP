using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatInteractable : Interactable
{
    public Being npc;
    public GameObject DialogueBox;
    public List<Event> dialogues;
    List<Event> completedDialogues = new List<Event>();
    DialogueUI dialogueUI;
    [SerializeField] bool working = false;
    [SerializeField] Material NPCDialogueMaterial;

    void Start() {
        DialogueBox.TryGetComponent<DialogueUI>(out dialogueUI);
    }

    protected override void Update() {
        base.Update();
        bool dead = false;
        if (npc.health <= 0 && !dead) {
            dead = true;
            DisableDialogue();
        }
    }

    protected override void Activate()
    {
        base.Activate();
        dialogueUI.SetDialogueImageMaterial(NPCDialogueMaterial);
    }

    void DisableDialogue() {
        DialogueBox.SetActive(false);
    }

    protected override void Deactivate()
    {
        DialogueBox.SetActive(false);
        base.Deactivate();
    }

    protected override void Check()
    {
        base.Check();
    }

    protected override void OnActivate()
    {
        base.OnActivate();
        if (!working) {
            RunDialogue();
        }
    }

    void RunDialogue() {
        StartCoroutine(ShowDialogue());
    }

    private IEnumerator ShowDialogue() {
        working = true;
        foreach (Event dialogue in dialogues) {
            if (!completedDialogues.Contains(dialogue)) {
                // do stuff here, show win screen, etc.
                dialogueUI.StartDialogue(dialogue);
                // just a simple time delay as an example
                yield return new WaitForSeconds(.5f);
                completedDialogues.Add(dialogue);
                // wait for player to press F
                yield return waitForKeyPress(KeyCode. F); // wait for this function to return
                // do other stuff after key press
                dialogueUI.TypingController.Reset();
                DisableDialogue();
                working = false;
            }
        }
    }
    
    private IEnumerator waitForKeyPress(KeyCode key)
    {
        bool done = false;
        while(!done) // essentially a "while true", but with a bool to break out naturally
        {
            if(Input.GetKeyDown(key))
            {
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
        // now this function returns
    }
}
