using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;


public class HandVisual : MonoBehaviour, IHandDataProvider
{

    public enum HandState
    {
        Hit,
        Stand,
        Bust
    }

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private Transform cardsContainer;
    private List<CardVisual> cards;
    private Fader fader;
    private int aceCount;

    public HandState State { get; set; }


    private void Awake()
    {
        ValidateSerializedFields();
        cards = new List<CardVisual>();
        fader = gameObject.GetComponent<Fader>();
    }

    private void Start()
    {
    }

    public void AddCard(CardVisual cardVisual)
    {
        cards.Add(cardVisual);
        cardVisual.transform.SetParent(cardsContainer, true);
        UpdateCardsPosition();
    }

    private void UpdateCardsPosition()
    {
        if (cards.Count == 0)
            return;

        float firstCardPosition = (1 - cards.Count) * GameConstants.CARD_SPACING / 2;

        for (int i = 0; i < cards.Count; i++)
        {
            float p = firstCardPosition + i * GameConstants.CARD_SPACING;

            Vector3 position = new Vector3(p, 0, i*-0.01f);
            CardVisual card = cards[i];
            Sequence sequence = DOTween.Sequence();
            sequence.Append(card.transform.DOLocalMove(position, GameConstants.CARD_MOVE_DELAY));
            if (i == cards.Count-1)
            {
                sequence.Append(card.transform.DOScaleX(0.0f, GameConstants.CARD_FLIP_DELAY).OnComplete(() => {
                    card.FaceUp();
                }));
                sequence.Append(card.transform.DOScaleX(1.0f, GameConstants.CARD_FLIP_DELAY).OnComplete(() => {
                    UpdateHandScore();
                    CheckBust();
                }));
            }
            sequence.SetEase(Ease.OutQuad);
        }

    }

    private void UpdateHandScore()
    {
        int score = GetHandScore();
        scoreText.text = (score == 0) ? "" : score.ToString();
    }

    public int GetHandScore()
    {
        int score = 0;
        aceCount = 0;
        int aceCardMaxMinScoreDiff = 10;
        int maxValidScore = 21;
        foreach (CardVisual card in cards)
        {
            score += card.CardSO.cardValue;
            if (card.CardSO.isAce)
            {
                aceCount += 1;
            }
        }

        while (aceCount > 0 && score > maxValidScore)
        {
            score -= aceCardMaxMinScoreDiff;
            aceCount--;
        }

        return score;
    }

    public void ResetHandVisual(CardsPool cardsPool)
    {
        fader?.UnFade();
        foreach (CardVisual cardVisual in cards)
        {
            cardVisual.Reset();
            cardsPool.Enqueue(cardVisual);
        }
        cards.Clear();
        UpdateHandScore();
        State = HandState.Hit;
        aceCount = 0;
    }

    public void CheckBust()
    {
        int maxValidScore = 21;
        int score = GetHandScore();
        if (score > maxValidScore)
        {
            State = HandState.Bust;
        }
    }


    public bool HasAceCardWithMaxScore()
    {
        return (aceCount > 0);
    }

    public bool HasCardWithScore(int score)
    {
        foreach (CardVisual card in cards)
        {
            if (card.CardSO.cardValue == score)
            {
                return true;
            }
        }
        return false;
    }

    public void ActivateHand()
    {
        fader?.UnFade();
    }

    public void DeactivateHand()
    {
        fader?.Fade();
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void ValidateSerializedFields()
    {
        if (scoreText == null) throw new MissingReferenceException("ScoreText not configured");
        if (cardsContainer == null) throw new MissingReferenceException("CardsContainer not configured");
    }

    public int GetCardCount()
    {
        return cards.Count;
    }
}
