using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text unitNameText;
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text unitCostText;

    [SerializeField] private GameObject progressCover;
    [SerializeField] private GameObject lockCover;

    private float progress = 1f;

    private Button spawnButton;
    private UnitSO data;

    private void Start()
    {
        if (gameObject.TryGetComponent(out spawnButton))
        {
            spawnButton.onClick.AddListener(() =>
            {
                int totalCoin = GameSetting.Instance.GetCoin();
                int totalReq = data.cost;
                
                if (totalCoin > totalReq)
                {
                    GameSetting.Instance.AddCoin(-totalReq);
                    GameSetting.Instance.SpawnUnit(data);
                    ProgressCard();
                }
            });
        }
    }

    private void Update()
    {
        if(progressCover.activeInHierarchy)
        {
            progress -= Time.deltaTime / 2f;
            progressCover.GetComponent<Image>().fillAmount = progress;

            if (progress < 0f) 
            {
                progressCover.SetActive(false);
                spawnButton.interactable = true;
            }
        }
    }

    private void ProgressCard()
    {
        progress = 1f;
        progressCover.SetActive(true);
        spawnButton.interactable = false;
    }

    public void SetupCard(UnitSO unit)
    {
        data = unit;

        unitNameText.text = unit.name;
        unitImage.sprite = unit.icon;
        unitCostText.text = $"{unit.cost} Coin";

        lockCover.SetActive(unit.isLock);
    }
}
