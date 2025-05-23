using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        None,
        Playing,
        Result
    }

    private int? currentPlayerIndex;
    [SerializeField]
    private Delaer dealer;
    [SerializeField]
    private PlayerController[] players;
    [SerializeField]
    private ResultUI resultUI;
    [SerializeField]
    private CardsPool cardsPool;
    private WinEvaluator winEvaluator;

    private GameState gameState;


    private void Awake()
    {
        ValidateSerializedFields();
        currentPlayerIndex = null;
        winEvaluator = new();
        gameState = GameState.None;
    }

    private void Start()
    {
        foreach (PlayerController playerController in players)
        {
            playerController.OnPlayerHit += PlayerController_OnPlayerHit;
            playerController.OnPlayerStand += PlayerController_OnPlayerStand;
        }
        resultUI.OnGameRestartEvent += ResultUI_OnGameRestartEvent;
        string messageText = "Welcome to BlackJack Pro";
        resultUI.ShowMessageUI(messageText);
    }

    private void OnDestroy()
    {
        foreach (PlayerController playerController in players)
        {
            playerController.OnPlayerHit -= PlayerController_OnPlayerHit;
            playerController.OnPlayerStand -= PlayerController_OnPlayerStand;
        }
        resultUI.OnGameRestartEvent -= ResultUI_OnGameRestartEvent;

    }

    private void ResultUI_OnGameRestartEvent(object sender, System.EventArgs e)
    {
        resultUI.HideMessageUI();
        ResetGameManager();
        StartGame();
    }

    public void ResetGameManager()
    {
        currentPlayerIndex = null;
        dealer.ResetDeck();
        foreach (PlayerController playerController in players)
        {
            playerController.ResetPlayerController(cardsPool);
        }
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
        StartCoroutine(DealInitialCards(() => {
            ActivatePlayerTurn();
        }));
    }

    private IEnumerator DealInitialCards(Action dealEndCallback)
    {
        float cardDealDelay = GameConstants.GetCardMoveDelay();
        int cardCount = 2;
        for (int i=0; i<cardCount; i++)
        {
            foreach (PlayerController playerController in players)
            {
                dealer.Hit(playerController);
                yield return new WaitForSeconds(cardDealDelay);
            }
        }
        foreach (PlayerController playerController in players)
        {
            playerController.GetHandVisual().DeactivateHand();
        }
        dealEndCallback?.Invoke();
    }


    private void PlayerController_OnPlayerStand(PlayerController playerController)
    {
        playerController.OnPlayerTurnEnd();
        playerController.GetHandVisual().DeactivateHand();
        ActivatePlayerTurn();
    }

    private void PlayerController_OnPlayerHit(PlayerController playerController)
    {
        playerController.OnPlayerTurnEnd();
        StartCoroutine(PerformPlayerHit(playerController, () => {
            playerController.GetHandVisual().DeactivateHand();
            if (playerController.IsBusted())
            {
                gameState = GameState.Result;
            }
            ActivatePlayerTurn();
        }));
    }

    private IEnumerator PerformPlayerHit(PlayerController playerController, Action completionCallback)
    {
        float cardDealDelay = GameConstants.GetCardMoveDelay();
        dealer.Hit(playerController);
        yield return new WaitForSeconds(cardDealDelay);
        completionCallback?.Invoke();
    }

    public void ActivatePlayerTurn()
    {
        int startIndex = (currentPlayerIndex == null) ? 0 : (int)currentPlayerIndex + 1;
        if (gameState == GameState.Playing && FindHittingPlayerIndex(startIndex, out int playerIndex))
        {
            Debug.Log("Hitting Player Found :: "+ playerIndex);
            currentPlayerIndex = playerIndex;
            PlayerController playerController = players[(int)currentPlayerIndex];
            playerController.OnPlayerTurnStart();
            playerController.GetHandVisual().ActivateHand();
        }
        else
        {
            StartCoroutine(EvaluateAndShowResult());
        }
    }


    private IEnumerator EvaluateAndShowResult()
    {
        const float resultDelay = 0.5f;
        yield return new WaitForSeconds(resultDelay);
        Dictionary<string, IHandDataProvider> playersData = GetAllPlayersData();
        string winText = winEvaluator.EvaluateWinnerAndGetResultText(playersData);
        resultUI.ShowMessageUI(winText);
    }


    private Dictionary<string, IHandDataProvider> GetAllPlayersData()
    {
        Dictionary<string, IHandDataProvider> playersData = new Dictionary<string, IHandDataProvider>();
        foreach (PlayerController playerController in players)
        {
            playersData[playerController.GetPlayerName()] = playerController.GetHandVisual();
        }
        return playersData;
    }


    private bool FindHittingPlayerIndex(int startIndex, out int playerIndex)
    {
        int count = players.Length;
        playerIndex = -1;
        for (int i = 0; i < count; i++)
        {
            int index = (startIndex + i) % count;
            if (players[index].IsHitting())
            {
                playerIndex = index;
                return true;
            }
        }
        return false; // No hitting player found
    }


    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void ValidateSerializedFields()
    {
        if (dealer == null) throw new MissingReferenceException("Dealer not configured");
        if (players.Length != 2) throw new MissingReferenceException("PlayerController shoudl be of count 2");
        if (resultUI == null) throw new MissingReferenceException("ResultUI not configured");
        if (cardsPool == null) throw new MissingReferenceException("Cardpool not configured");
    }
}
