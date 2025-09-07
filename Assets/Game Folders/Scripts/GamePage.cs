using System;
using UnityEngine;

public class GamePage : Page
{
    [Header("FOR CARDS")]
    [SerializeField] private CardPrefab _card;
    [SerializeField] private Transform cardContent;

    private void Start()
    {
        // Setup Card
        SetupCard();
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
