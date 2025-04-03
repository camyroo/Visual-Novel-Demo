using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FlashGlitchEffect : ITextEffect
{
    private string overrideText;
    private float flashDuration;

    public FlashGlitchEffect(string text, float duration = 1.5f)
    {
        overrideText = text;
        flashDuration = duration;
    }

    public IEnumerator Run(string line, TextMeshProUGUI dialogueText, float speed, Func<bool> shouldSkip)
    {
        // Cache original state
        string originalText = line;
        Color originalColor = dialogueText.color;
        FontStyles originalStyle = dialogueText.fontStyle;

        // Flash override text
        dialogueText.color = Color.red;
        dialogueText.fontStyle = FontStyles.Bold;
        dialogueText.text = overrideText;

        yield return new WaitForSeconds(flashDuration);

        // Restore original line
        dialogueText.color = originalColor;
        dialogueText.fontStyle = originalStyle;
        dialogueText.text = originalText;

        yield return null;
    }
}
