using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<PlayerController> OnPlayerHit;
    public event Action<PlayerController> OnPlayerStand;

    [SerializeField] private HandVisual handVisual;
    [SerializeField] private ActionButtonController actionButtonController;
    [SerializeField] private string playerName;


    private void Awake()
    {
        ValidateSerializedFields();
    }
    private void Start()
    {
        SetupActionButtonsController();
        actionButtonController.gameObject.SetActive(false);
    }

    protected virtual void SetupActionButtonsController()
    {
        actionButtonController.OnHit += ActionButtonController_OnHit;
        actionButtonController.OnStand += ActionButtonController_OnStand;
    }

    protected void ActionButtonController_OnStand()
    {
        handVisual.State = HandVisual.HandState.Stand;
        OnPlayerStand?.Invoke(this);
    }

    protected void ActionButtonController_OnHit()
    {
        OnPlayerHit?.Invoke(this);
    }

    public void ResetPlayerController(CardsPool cardsPool)
    {
        handVisual.ResetHandVisual(cardsPool);
        actionButtonController.gameObject.SetActive(false);
    }

    public HandVisual GetHandVisual()
    {
        return handVisual;
    }

    public virtual void OnPlayerTurnStart()
    {
        actionButtonController.gameObject.SetActive(true);
    }

    public virtual void OnPlayerTurnEnd()
    {
        actionButtonController.gameObject.SetActive(false);
    }

    public bool IsHitting()
    {
        return (handVisual.State == HandVisual.HandState.Hit);
    }

    public bool IsBusted()
    {
        return (handVisual.State == HandVisual.HandState.Bust);
    }


    public string GetPlayerName()
    {
        return playerName;
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void ValidateSerializedFields()
    {
        if (handVisual == null) throw new MissingReferenceException("HandVisual not configured");
        if (actionButtonController == null) throw new MissingReferenceException("ActionButtonController not configured");
    }

}
