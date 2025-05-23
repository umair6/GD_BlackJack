using System.Collections.Generic;
using UnityEngine;

public class DeckGenerator : MonoBehaviour
{

    [SerializeField]
    private CardSO[] cards;


    private void Awake()
    {
        ValidateSerializedFields();
    }

    public List<CardSO> GetShuffledCardDeck(int deckCount)
    {
        List<CardSO> cardDeck = new List<CardSO>();

        for (int i = 0; i < deckCount; i++)
        {
            cardDeck.AddRange(cards);
        }
        cardDeck.Shuffle();
        return cardDeck;
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void ValidateSerializedFields()
    {
        if (cards.Length == 0) throw new MissingReferenceException("Cards not configured!");
    }
}
