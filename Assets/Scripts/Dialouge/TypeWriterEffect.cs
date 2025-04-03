using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : ITextEffect
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
            yield return new WaitForSeconds(speed);
        }
    }
}