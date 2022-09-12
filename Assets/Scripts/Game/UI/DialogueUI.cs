using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public EventController TypingController;
    public Image DialogueImage;

    public void StartDialogue(Event dialogueEvent) {
        gameObject.SetActive(true);
        TypingController.actualEvent = dialogueEvent;
        TypingController.ReproduceText();
    }

    public void SetDialogueImageMaterial(Material material) {
        if (DialogueImage) {
            DialogueImage.material = material;
            DialogueImage.SetMaterialDirty();
        }
    }
}
