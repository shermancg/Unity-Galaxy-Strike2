using UnityEngine;
using TMPro;

public class DialogueLines : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueTextObject;
    [SerializeField] string[] allDialogueLines;

    int currentDialogueLine = 0;

    public void NextDialogueLine()
    {
        currentDialogueLine++;
        dialogueTextObject.text = allDialogueLines[currentDialogueLine];
    }
}
