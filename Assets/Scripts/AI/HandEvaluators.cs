
using UnityEngine;

public class AndEvaluator : IHandEvaluator
{
    private readonly string evaluatorName;
    private readonly IHandEvaluator[] evaluators;

    public AndEvaluator(string evaluatorName, params IHandEvaluator[] evaluators)
    {
        this.evaluatorName = evaluatorName;
        this.evaluators = evaluators;
    }

    public bool Evaluate()
    {
        bool result = true;

        for (int i = 0; i < evaluators.Length; i++)
        {
            if (!evaluators[i].Evaluate())
            {
                result = false;
                break;
            }
        }
        if (!string.IsNullOrEmpty(evaluatorName))
        {
            Debug.Log("Evaluator::" + evaluatorName + "::" + result);
        }
        return result;
    }
}

public class OrEvaluator : IHandEvaluator
{
    private readonly string evaluatorName;
    private readonly IHandEvaluator[] evaluators;

    public OrEvaluator(string evaluatorName, params IHandEvaluator[] evaluators)
    {
        this.evaluatorName = evaluatorName;
        this.evaluators = evaluators;
    }

    public bool Evaluate()
    {
        bool result = false;
        for (int i = 0; i < evaluators.Length; i++)
        {
            if (evaluators[i].Evaluate())
            {
                result = true;
                break;
            }
        }
        if (!string.IsNullOrEmpty(evaluatorName))
        {
            Debug.Log("Evaluator::" + evaluatorName + "::" + result);
        }
        return result;
    }
}



public class NotEvaluator : IHandEvaluator
{
    private readonly string evaluatorName;
    private readonly IHandEvaluator evaluator;

    public NotEvaluator(string evaluatorName, IHandEvaluator evaluator)
    {
        this.evaluatorName = evaluatorName;
        this.evaluator = evaluator;
    }

    public bool Evaluate()
    {
        bool result = !(evaluator.Evaluate());
        if (!string.IsNullOrEmpty(evaluatorName))
        {
            Debug.Log("Evaluator::" + evaluatorName + "::" + result);
        }
        return result;
    }
}



public class CardsEvaluator : IHandEvaluator
{
    private readonly string evaluatorName;
    private IHandDataProvider playerHand;
    private int[] cards;
    public CardsEvaluator(string evaluatorName, int[] cards, IHandDataProvider playerHand)
    {
        this.evaluatorName = evaluatorName;
        this.playerHand = playerHand;
        this.cards = cards;
    }
    public bool Evaluate()
    {
        bool result = false;
        foreach (int card in cards)
        {
            if (playerHand.HasCardWithScore(card))
            {
                result = true;
                break;
            }
        }

        if (!string.IsNullOrEmpty(evaluatorName))
        {
            Debug.Log("Evaluator::" + evaluatorName + "::" + result);
        }
        return result;
    }
}


public class AceWithMaxScoreEvaluator : IHandEvaluator
{
    private readonly string evaluatorName;
    private IHandDataProvider playerHand;
    public AceWithMaxScoreEvaluator(string evaluatorName, IHandDataProvider playerHand)
    {
        this.evaluatorName = evaluatorName;
        this.playerHand = playerHand;
    }
    public bool Evaluate()
    {
        bool result = playerHand.HasAceCardWithMaxScore();
        if (!string.IsNullOrEmpty(evaluatorName))
        {
            Debug.Log("Evaluator::" + evaluatorName + "::" + result);
        }
        return result;

    }
}

public class HandScoreEvaluator : IHandEvaluator
{
    private readonly string evaluatorName;
    private readonly int score;
    private IHandEvaluator.ValueComparitor comparitor;
    private IHandDataProvider playerHand;

    public HandScoreEvaluator(string evaluatorName, int score, IHandEvaluator.ValueComparitor comparitor, IHandDataProvider playerHand)
    {
        this.evaluatorName = evaluatorName;
        this.score = score;
        this.comparitor = comparitor;
        this.playerHand = playerHand;

    }
    public bool Evaluate()
    {
        bool result = false;
        switch (comparitor)
        {
            case IHandEvaluator.ValueComparitor.Less:
                result = playerHand.GetHandScore() < score;
                break;
            case IHandEvaluator.ValueComparitor.Greater:
                result = playerHand.GetHandScore() > score;
                break;
            case IHandEvaluator.ValueComparitor.Equal:
                result = playerHand.GetHandScore() == score;
                break;

        }
        if (!string.IsNullOrEmpty(evaluatorName))
        {
            Debug.Log("Evaluator::" + evaluatorName + "::" + result);
        }
        return result;
    }
}

public class CardCountEvaluator : IHandEvaluator
{
    private readonly string evaluatorName;
    private readonly int count;
    private IHandEvaluator.ValueComparitor comparitor;
    private IHandDataProvider playerHand;

    public CardCountEvaluator(string evaluatorName, int count, IHandEvaluator.ValueComparitor comparitor, IHandDataProvider playerHand)
    {
        this.evaluatorName = evaluatorName;
        this.count = count;
        this.comparitor = comparitor;
        this.playerHand = playerHand;

    }
    public bool Evaluate()
    {
        bool result = false;
        switch (comparitor)
        {
            case IHandEvaluator.ValueComparitor.Less:
                result = playerHand.GetCardCount() < count;
                break;
            case IHandEvaluator.ValueComparitor.Greater:
                result = playerHand.GetCardCount() > count;
                break;
            case IHandEvaluator.ValueComparitor.Equal:
                result = playerHand.GetCardCount() == count;
                break;
        }
        if (!string.IsNullOrEmpty(evaluatorName))
        {
            Debug.Log("Evaluator::" + evaluatorName + "::" +result);
        }
        return result;
    }
}