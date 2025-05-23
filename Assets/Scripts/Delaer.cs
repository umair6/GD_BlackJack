using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delaer : MonoBehaviour
{
    private const int CARDS_DECK_COUNT = 1;
    [SerializeField]
    private DeckGenerator deckGenerator;
    [SerializeField]
    private CardsPool cardsPool;
    [SerializeField]
    private Transform cardSpawnPosition;


    private List<CardSO> cardsDeck;

    private void Awake()
    {
        ValidateSerializedFields();
        ResetDeck();
    }

    public void ResetDeck()
    {
        cardsDeck = deckGenerator.GetShuffledCardDeck(CARDS_DECK_COUNT);
    }

    private CardVisual GetCardVisual()
    {
        CardVisual cardVisual = cardsPool.Dequeue();
        cardVisual.transform.position = cardSpawnPosition.position;
        cardVisual.CardSO = GetCardSO();
        return cardVisual;

    }

    private CardSO GetCardSO()
    {
        if (cardsDeck == null || cardsDeck.Count == 0)
        {
            throw new System.NullReferenceException("CardDeck not found");
        }

        CardSO card = cardsDeck[0];
        cardsDeck.RemoveAt(0);
        return card;
    }

    public void Hit(PlayerController playerController)
    {
        playerController.GetHandVisual().AddCard(GetCardVisual());
    }


    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void ValidateSerializedFields()
    {
        if (deckGenerator == null) throw new MissingReferenceException("DeckGenerator not configured");
        if (cardsPool == null) throw new MissingReferenceException("CardsPool not configured");
        if (cardSpawnPosition == null) throw new MissingReferenceException("CardSpawnPosition not configured");       
    }

}
