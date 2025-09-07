using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePage : Page
{
    [SerializeField] private Button callMerchantButton;

    [Header("FOR TOP CONTENT")]
    [SerializeField] private TMP_Text resourceText;
    [SerializeField] private TMP_Text coinText;

    [Header("FOR CARDS")]
    [SerializeField] private CardPrefab _card;
    [SerializeField] private Transform cardContent;

    private void Start()
    {
        GameSetting.Instance.OnWoodStorageUpdated += Instance_OnWoodStorageUpdated;
        GameSetting.Instance.OnCoinAmountUpdated += Instance_OnCoinAmountUpdated;
        // Setup Card
        SetupCard();

        callMerchantButton.onClick.AddListener(() => GameSetting.Instance.CallMerchant());
    }

    private void OnDestroy()
    {
        GameSetting.Instance.OnWoodStorageUpdated -= Instance_OnWoodStorageUpdated;
        GameSetting.Instance.OnCoinAmountUpdated -= Instance_OnCoinAmountUpdated;
    }

    private void Instance_OnWoodStorageUpdated(int amount)
    {
        resourceText.text = amount.ToString();
    }

    private void Instance_OnCoinAmountUpdated(int obj)
    {
        coinText.text = obj.ToString();
    }

    private void SetupCard()
    {
        foreach (UnitSO item in GameManager.Instance.GetAsset().GetAllUnitData())
        {
            var clone = Instantiate(_card, cardContent);
            clone.SetupCard(item);
        }
    }
}
