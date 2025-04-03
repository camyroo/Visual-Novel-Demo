using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string id;
    public string displayName;
    public Dictionary<string, string> portraits; // Runtime dictionary
}

[System.Serializable]
public class CharacterDataFlat
{
    public string id;
    public string displayName;
    public string[] portraitKeys;
    public string[] portraitPaths;
}

[System.Serializable]
public class CharacterDataListFlat
{
    public CharacterDataFlat[] characters;
}

public class CharacterDatabase : MonoBehaviour
{
    public static CharacterDatabase Instance;

    private Dictionary<string, CharacterData> characterDict = new Dictionary<string, CharacterData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCharacterData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadCharacterData()
    {
        TextAsset json = Resources.Load<TextAsset>("characterData");

        if (json == null)
        {
            Debug.LogError("characterData.json not found in Resources.");
            return;
        }

        CharacterDataListFlat flatData = JsonUtility.FromJson<CharacterDataListFlat>("{\"characters\":" + json.text + "}");

        foreach (var flat in flatData.characters)
        {
            var portraitDict = new Dictionary<string, string>();
            for (int i = 0; i < flat.portraitKeys.Length; i++)
            {
                string key = flat.portraitKeys[i].ToLower().Trim();
                string path = flat.portraitPaths[i].Trim();
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(path))
                    portraitDict[key] = path;
            }

            CharacterData runtimeData = new CharacterData
            {
                id = flat.id.ToLower().Trim(),
                displayName = flat.displayName.Trim(),
                portraits = portraitDict
            };

            characterDict[runtimeData.id] = runtimeData;
        }

        Debug.Log($"Loaded {characterDict.Count} characters.");
    }

    public CharacterData GetCharacter(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;

        id = id.ToLower().Trim();
        return characterDict.ContainsKey(id) ? characterDict[id] : null;
    }
}
