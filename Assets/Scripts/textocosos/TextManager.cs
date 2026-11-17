using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TextManagerr : MonoBehaviour
{
    [Header("Global Data")]
    public static TextManagerr Instance;

    public Language currentLanguage;

    [Header("Texts")]
    [SerializeField] private TextAsset textJSON;
    private TextReferenceWopper wopper;

   
    private Dictionary<string, string> textDictionary = new Dictionary<string, string>();

    public HashSet<ITranscoso> transcosos = new HashSet<ITranscoso>();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void ChangeLanguage(Language language)
    {
        currentLanguage = language;
        wopper = JsonUtility.FromJson<TextReferenceWopper>(textJSON.text);

        switch (language)
        {
            case Language.en:
            {
                    textDictionary = wopper.texts.DistinctBy(item => item.text_key).ToDictionary(item => item.text_key, item => item.en);
                    break;
            }
            case Language.es:
                {
                    textDictionary = wopper.texts.DistinctBy(item => item.text_key).ToDictionary(item => item.text_key, item => item.es);
                    break;
                }

        }
        TranslateTranslatable();
    }

    public void TranslateTranslatable()
    {
        foreach(ITranscoso transcoso in transcosos)
        {
            transcoso.TranslatableText.text = GetText(transcoso.TextID);
        }
    }
    public string GetText (string searchedKey)
    {
        string text;
        if(textDictionary.TryGetValue(searchedKey, out text))
        {
            text = textDictionary[searchedKey];
        }
        else
        {
            text = "Missing Text";
        }
        return text;
    }
}


public class TextReferenceWopper
{
    public TextReference[] texts;
}
public class TextReference
{
    public string text_key;

    public string en;

    public string es;
}
public enum Language
{
    en,

    es,
}
