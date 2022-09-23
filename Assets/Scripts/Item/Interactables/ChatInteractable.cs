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
    bool working = false;
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
        Tick();
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

    private void Tick() {
        if (!working) {
            RunDialogue();
        }
    }

    void RunDialogue() {
        foreach (Event dialogue in dialogues) {
            if (!completedDialogues.Contains(dialogue)) {
                StartCoroutine(ShowDialogue(dialogue));
            }
            break;
        }
    }

    private IEnumerator ShowDialogue(Event dialogue) {
        // do stuff here, show win screen, etc.
        StartDialogue(dialogue);
        // just a simple time delay as an example
        yield return new WaitForSeconds(2.5f);
        // wait for player to press F
        yield return waitForKeyPress(KeyCode. F); // wait for this function to return
        // do other stuff after key press
        ClearDialogue(dialogue);
    }

    private void StartDialogue(Event dialogue) {
        working = true;
        dialogueUI.StartDialogue(dialogue);
    }

    private void ClearDialogue(Event dialogue) {
        completedDialogues.Add(dialogue);
        dialogueUI.TypingController.Reset();
        DisableDialogue();
        working = false;
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
