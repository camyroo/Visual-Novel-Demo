using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InkManager : MonoBehaviour
{
    public static InkManager Instance { get; private set; }
    private ITextEffect currentTextEffect;

    [Header("Background")]
    public Image backgroundImage;


    [Header("Ink")]
    public TextAsset inkJSONAsset;
    private Story story;

    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Image portraitImage;
    public GameObject choicesPanel;
    public GameObject choiceButtonPrefab;

    [Header("Typewriter Settings")]
    public float typewriterSpeed = 0.02f;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool skipTyping = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        story = new Story(inkJSONAsset.text);
        currentTextEffect = new TypewriterEffect(); // Default effect
        ContinueStory();
    }

    public void SetTextEffect(ITextEffect effect)
    {
        currentTextEffect = effect;
    }



    void ContinueStory()
    {
        StartCoroutine(HandleStoryLineByLine());
    }

    IEnumerator HandleStoryLineByLine()
    {
        while (story.canContinue)
        {
            string line = story.Continue().Trim();
            List<string> tags = story.currentTags;

            string speaker = story.variablesState["speaker"]?.ToString();
            string portrait = story.variablesState["portrait"]?.ToString();

            ShowDialogue(line, speaker, portrait);
            TagEffectManager.Instance.HandleTags(tags);


            // Wait for input or skip
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            if (isTyping)
            {
                skipTyping = true;
                yield return new WaitUntil(() => !isTyping);
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            }
            yield return new WaitUntil(() => !Input.GetMouseButton(0));
        }

        // Show choices
        if (story.currentChoices.Count > 0)
        {
            ShowChoices();
        }
        else
        {
            dialogueText.text = "END";
        }
    }



    void ShowDialogue(string line, string speakerID, string portraitKey)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(line));

        // Speaker
        if (!string.IsNullOrEmpty(speakerID))
        {
            CharacterData data = CharacterDatabase.Instance.GetCharacter(speakerID);
            if (data != null)
            {
                nameText.text = data.displayName;

                if (data.portraits.TryGetValue(portraitKey, out string portraitPath))
                    portraitImage.sprite = Resources.Load<Sprite>(portraitPath);
            }
        }
        else
        {
            nameText.text = "";
        }

        // Background
        string bgKey = story.variablesState["background"]?.ToString();
        if (!string.IsNullOrEmpty(bgKey))
        {
            Sprite bgSprite = Resources.Load<Sprite>("Backgrounds/" + bgKey);
            if (bgSprite != null)
            {
                backgroundImage.sprite = bgSprite;
            }
            else
            {
                Debug.LogWarning($"Background not found: {bgKey}");
            }
        }
    }


    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        skipTyping = false;

        yield return StartCoroutine(currentTextEffect.Run(
            line, dialogueText, typewriterSpeed, () => skipTyping));

        isTyping = false;
    }


    void ShowChoices()
    {
        Debug.Log("Displaying choices");

        foreach (Transform child in choicesPanel.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < story.currentChoices.Count; i++)
        {
            Choice choice = story.currentChoices[i];
            GameObject button = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;

            int choiceIndex = i;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log($"Choice selected: {choice.text}");
                story.ChooseChoiceIndex(choiceIndex);
                choicesPanel.SetActive(false);
                ContinueStory();
            });
        }

        choicesPanel.SetActive(true);
    }
}
