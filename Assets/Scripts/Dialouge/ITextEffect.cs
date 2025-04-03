using System;
using System.Collections;
using TMPro;

public interface ITextEffect
{
    IEnumerator Run(string line, TextMeshProUGUI dialogueText, float speed, Func<bool> shouldSkip);
}