using UnityEngine;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{
    [SerializeField]
    private Sprite backSprite;
    private SpriteRenderer spriteRenderer;


    public CardSO CardSO { get; set; }


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ValidateSerializedFields();
    }

    public void FaceUp()
    {
        spriteRenderer.sprite = CardSO.cardSprite;
    }


    public void Reset()
    {
        CardSO = null;
        spriteRenderer.sprite = backSprite;
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void ValidateSerializedFields()
    {
        if (spriteRenderer == null) throw new MissingReferenceException("Renderer not found in card!");
        if (backSprite == null) throw new MissingReferenceException("BackSprite not found in card!");
    }


}
