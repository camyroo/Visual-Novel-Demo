using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TagEffectManager : MonoBehaviour
{
    public static TagEffectManager Instance;

    public TextMeshProUGUI dialogueText;
    public Image backgroundImage_;

    public Image jumpscareImage;
    public AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

public void HandleTags(List<string> tags)
{
    bool customEffectSet = false;

    foreach (string tag in tags)
    {
        Debug.Log($"Handling tag: {tag}");

        // Set custom text effects
        if (tag == "glitch")
        {
            InkManager.Instance.SetTextEffect(new GlitchTypewriterEffect());
            customEffectSet = true;
        }
        else if (tag.StartsWith("flashglitch:"))
        {
            string glitchText = tag.Substring("flashglitch:".Length).Replace("_", " ");
            InkManager.Instance.SetTextEffect(new FlashGlitchEffect(glitchText, 1.5f));
            customEffectSet = true;
        }

        // Visual/audio effects
        switch (tag)
        {
            case "glitch":
                StartCoroutine(GlitchText());
                break;
            case "whisper":
                Debug.Log("Whisper tag matched. Attempting to play Fortnite_Death");
                PlaySFX("Fortnite_Death");
                break;
            case "flicker":
                StartCoroutine(FlickerBG());
                break;
        }
    }

    // Default to typewriter if no tag overrides it
    if (!customEffectSet)
    {
        InkManager.Instance.SetTextEffect(new TypewriterEffect());
    }
}



    IEnumerator GlitchText()
    {
        FontStyles originalStyle = dialogueText.fontStyle;
        Color originalColor = dialogueText.color;

        dialogueText.fontStyle = FontStyles.Italic;
        dialogueText.color = Color.red;

        yield return new WaitForSeconds(0.15f);

        dialogueText.fontStyle = originalStyle;
        dialogueText.color = originalColor;
    }


IEnumerator FlickerBG()
{
    for (int i = 0; i < 3; i++)
    {
        backgroundImage_.enabled = false;
        jumpscareImage.enabled = true; // Show scary image
        yield return new WaitForSeconds(0.2f);

        backgroundImage_.enabled = true;
        jumpscareImage.enabled = false; // Hide it again
        yield return new WaitForSeconds(0.05f);
    }

    // Make sure it ends off
    jumpscareImage.enabled = false;
}


    public void PlaySFX(string clipName)
    {
        Debug.Log($"Trying to load: SFX/{clipName}");
        AudioClip clip = Resources.Load<AudioClip>($"SFX/{clipName}");
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
            Debug.Log("Clip played!");
        }
        else
        {
            Debug.LogWarning($"SFX clip not found: {clipName}");
        }
    }


}
