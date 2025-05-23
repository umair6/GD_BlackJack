using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonController : MonoBehaviour
{

    [SerializeField]
    private Button hitButton;
    [SerializeField]
    private Button standButton;


    public event Action OnHit;
    public event Action OnStand;


    private void Awake()
    {
        ValidateSerializedFields();
        hitButton.onClick.AddListener(() => {
            OnHit?.Invoke();
        });

        standButton.onClick.AddListener(() => {
            OnStand?.Invoke();
        });
    }


    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void ValidateSerializedFields()
    {
        if (hitButton == null) throw new MissingReferenceException("HitButton not configured!");
        if (standButton == null) throw new MissingReferenceException("scoreText is not assigned.");
    }

}
