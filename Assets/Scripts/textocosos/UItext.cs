using TMPro;
using UnityEngine;

public class UItext : MonoBehaviour, ITranscoso
{
    [field:SerializeField] public TextMeshProUGUI TranslatableText { get; set; }
    [field: SerializeField] public string TextID { get; set; }

    public void OnEnable()
    {
        FindAnyObjectByType<TextManagerr>().transcosos.Add(this);
        TextManagerr.Instance.transcosos.Add(this);
        TranslatableText.text = TextManagerr.Instance.GetText(TextID);
    }

    public void OnDisable()
    {
        TextManagerr.Instance.transcosos.Remove(this);
    }
}


