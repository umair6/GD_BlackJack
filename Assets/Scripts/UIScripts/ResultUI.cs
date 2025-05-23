using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI messageText;

    [SerializeField]
    private Button restartButton;


    public event EventHandler OnGameRestartEvent;

    private void Awake()
    {
        ValidateSerializedFields();
        restartButton.onClick.AddListener(() => {

            OnGameRestartEvent?.Invoke(this, EventArgs.Empty);
        });
        gameObject.SetActive(false);
    }

    public void ShowMessageUI(string message)
    {
        messageText.text = message;
        gameObject.SetActive(true);
    }

    public void HideMessageUI()
    {
        gameObject.SetActive(false);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void ValidateSerializedFields()
    {
        if (messageText == null) throw new MissingReferenceException("ResultText not configured");
        if (restartButton == null) throw new MissingReferenceException("RestartButton not configured");
    }
}
