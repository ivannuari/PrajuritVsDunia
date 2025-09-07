using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text unitNameText;
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text unitCostText;

    private Button spawnButton;
    private UnitSO data;

    private void Start()
    {
        if (gameObject.TryGetComponent(out spawnButton))
        {
            spawnButton.onClick.AddListener(() => GameSetting.Instance.SpawnUnit(data));
        }
    }

    public void SetupCard(UnitSO unit)
    {
        data = unit;

        unitNameText.text = unit.name;
        unitImage.sprite = unit.icon;
        unitCostText.text = $"{unit.cost} Coin";
    }
}
