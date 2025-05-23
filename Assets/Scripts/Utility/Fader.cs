using TMPro;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField]
    private Transform container;


    [SerializeField]
    private TextMeshProUGUI text;


    private const float FADE_AMOUNT = 0.5f;

    // Update is called once per frame
    public void Fade()
    {
        SpriteRenderer[] renderers = container.GetComponentsInChildren<SpriteRenderer>(); //Or other renderer components
        foreach (SpriteRenderer renderer in renderers)
        {
            if (renderer != null)
            {
                Color originalColor = renderer.color;
                renderer.color = new Color(originalColor.r * FADE_AMOUNT, originalColor.g * FADE_AMOUNT, originalColor.b * FADE_AMOUNT, originalColor.a);
            }
        }
        Color textOriginalColor = text.color;
        text.color = new Color(textOriginalColor.r * FADE_AMOUNT, textOriginalColor.g * FADE_AMOUNT, textOriginalColor.b * FADE_AMOUNT, textOriginalColor.a);
    }

    public void UnFade()
    {
        SpriteRenderer[] renderers = container.GetComponentsInChildren<SpriteRenderer>(); //Or other renderer components
        foreach (SpriteRenderer renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.color = Color.white;
            }
        }       
        text.color = Color.white;

    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void ValidateSerializedFields()
    {
        if (container == null) throw new MissingReferenceException("Container not configured");
        if (text == null) throw new MissingReferenceException("Text not configured");
    }
}
