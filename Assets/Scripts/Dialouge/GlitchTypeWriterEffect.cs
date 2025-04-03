using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GlitchTypewriterEffect : ITextEffect
{
    public IEnumerator Run(string line, TextMeshProUGUI dialogueText, float speed, Func<bool> shouldSkip)
    {
        dialogueText.text = "";

        foreach (char c in line)
        {
            if (shouldSkip())
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += c;

            if (UnityEngine.Random.value < 0.05f)
                dialogueText.text += "<alpha=#00>" + c + "</alpha>";

            yield return new WaitForSeconds(speed * UnityEngine.Random.Range(0.5f, 1.2f));
        }
    }
}
