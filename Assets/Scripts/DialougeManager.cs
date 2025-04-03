using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Image portraitImage;

    [Header("Settings")]
    public float typingSpeed = 0.02f;

    private Coroutine typingCoroutine;

    public void ShowDialogue(string text, string speakerName, Sprite portrait)
    {
        dialoguePanel.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        nameText.text = speakerName;
        portraitImage.sprite = portrait;

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    IEnumerator TypeText(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public bool IsTyping()
    {
        return typingCoroutine != null;
    }

    public void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    public void SetFullText(string fullLine)
    {
        dialogueText.text = fullLine;
    }
}
