using UnityEngine;

public class AIPlayerController : PlayerController
{


    [SerializeField]
    private HandVisual opponentHandVisual;

    private IHandEvaluator canHitEvaluator;



    private void Awake()
    {
        ValidateSerializedFields();
        canHitEvaluator = CreateCanHitEvaluator(GetHandVisual(), opponentHandVisual);

    }

    protected override void SetupActionButtonsController()
    {
    }

    public override void OnPlayerTurnStart()
    {
        float randomDely = Random.Range(0, 1);

        Invoke("PerformAIMove", randomDely);
    }

    private void PerformAIMove()
    {
        if (CanPlayerHit())
        {
            ActionButtonController_OnHit();
        }
        else
        {
            ActionButtonController_OnStand();
        }
    }

    public override void OnPlayerTurnEnd()
    {
    }


    private bool CanPlayerHit()
    {
        return canHitEvaluator.Evaluate();
    }

    private IHandEvaluator CreateCanHitEvaluator(IHandDataProvider player, IHandDataProvider opponent)
    {
        IHandDataProvider playerHandDataProvider = opponent;
        IHandDataProvider aiPlayerHandDataProvider = player;

        IHandEvaluator aiPlayerHasAceWithMaxScore = new AceWithMaxScoreEvaluator("", aiPlayerHandDataProvider);
        IHandEvaluator aiPlayerHasNoAceWithMaxScore = new NotEvaluator("", aiPlayerHasAceWithMaxScore);

        IHandEvaluator playerHasAceOrCardWithMoreThanSeven = new CardsEvaluator("", new int[] { 7,8,9,10,11 }, playerHandDataProvider);
        IHandEvaluator aiScoreLessThan17 = new HandScoreEvaluator("", 17, IHandEvaluator.ValueComparitor.Less, aiPlayerHandDataProvider);

        IHandEvaluator playerHas456 = new CardsEvaluator("", new int[] { 4, 5, 6}, playerHandDataProvider);
        IHandEvaluator aiScoreLessThan12 = new HandScoreEvaluator("", 12, IHandEvaluator.ValueComparitor.Less, aiPlayerHandDataProvider);

        IHandEvaluator playerHas23 = new CardsEvaluator("", new int[] { 2, 3}, playerHandDataProvider);
        IHandEvaluator aiScoreLessThan13 = new HandScoreEvaluator("", 13, IHandEvaluator.ValueComparitor.Less, aiPlayerHandDataProvider);


        IHandEvaluator rule1_a = new AndEvaluator("rule1_a", aiPlayerHasNoAceWithMaxScore, playerHasAceOrCardWithMoreThanSeven, aiScoreLessThan17);
        IHandEvaluator rule1_b = new AndEvaluator("rule1_b", aiPlayerHasNoAceWithMaxScore, playerHas456, aiScoreLessThan12);
        IHandEvaluator rule1_c = new AndEvaluator("rule1_c", aiPlayerHasNoAceWithMaxScore, playerHas23, aiScoreLessThan13);
        IHandEvaluator rule1 = new OrEvaluator("rule1", rule1_a, rule1_b, rule1_c);


        IHandEvaluator aiScoreGreaterEqual19 = new HandScoreEvaluator("", 18, IHandEvaluator.ValueComparitor.Greater, aiPlayerHandDataProvider);
        IHandEvaluator aiScoreEqual18 = new HandScoreEvaluator("", 18, IHandEvaluator.ValueComparitor.Equal, aiPlayerHandDataProvider);
        IHandEvaluator aiCardMoreThan2 = new CardCountEvaluator("", 2, IHandEvaluator.ValueComparitor.Greater, aiPlayerHandDataProvider);


        IHandEvaluator skip_rule_a = new AndEvaluator("skip_rule_a", aiScoreGreaterEqual19);
        IHandEvaluator skip_rule_b_1 = new AndEvaluator("", aiScoreEqual18, aiCardMoreThan2);
        IHandEvaluator skip_rule_b_2 = new CardsEvaluator("", new int[] { 9, 10, 11 }, playerHandDataProvider); ;
        IHandEvaluator skip_rule_b_2_NOT = new NotEvaluator("", skip_rule_b_2);
        IHandEvaluator skip_rule_b = new AndEvaluator("skip_rule_b", skip_rule_b_1, skip_rule_b_2_NOT);
        IHandEvaluator rule2 = new AndEvaluator("rule2", aiPlayerHasAceWithMaxScore, new NotEvaluator("",
            new OrEvaluator("(skip_rule_a || skip_rule_b)", skip_rule_a, skip_rule_b)));

        IHandEvaluator finalEvaluator = new OrEvaluator("finalEvaluator", rule1, rule2);
        return finalEvaluator;

    }


    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void ValidateSerializedFields()
    {
        if (opponentHandVisual == null) throw new MissingReferenceException("Opponent hand not configured!");
    }
}
