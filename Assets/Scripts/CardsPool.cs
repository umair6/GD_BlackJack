using System.Collections.Generic;
using UnityEngine;

public class CardsPool : MonoBehaviour
{
    private Queue<CardVisual> poolQueue;

    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private int initialSize;


    private void Awake()
    {
        ValidateSerializedFields();
        poolQueue = new Queue<CardVisual>();

        for (int i = 0; i < initialSize; i++)
        {
            AddCardVisualToQueue();
        }
    }

    private void AddCardVisualToQueue()
    {
        CardVisual cardVisual = Instantiate(cardPrefab).GetComponent<CardVisual>();
        cardVisual.transform.SetParent(this.transform, false);
        cardVisual.gameObject.SetActive(false);
        poolQueue.Enqueue(cardVisual);
    }

    public CardVisual Dequeue()
    {
        if (poolQueue.Count == 0)
        {
            AddCardVisualToQueue();
        }

        CardVisual cardVisual = poolQueue.Dequeue();
        cardVisual.gameObject.SetActive(true);
        return cardVisual;
    }

    public void Enqueue(CardVisual cardVisual)
    {
        cardVisual.gameObject.SetActive(false);
        cardVisual.transform.SetParent(this.transform, false);
        poolQueue.Enqueue(cardVisual);
    }


    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void ValidateSerializedFields()
    {
        if (cardPrefab == null) throw new MissingReferenceException("CardPrefab not configured!");
    }
}
