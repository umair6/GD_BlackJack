using UnityEngine;


[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/Card")]


public class CardSO : ScriptableObject
{
    [SerializeField]
    public Sprite cardSprite;
    [SerializeField]
    public int cardValue;
    [SerializeField]
    public bool isAce;

}
