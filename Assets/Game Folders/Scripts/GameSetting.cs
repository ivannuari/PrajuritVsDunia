using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    public static GameSetting Instance;

    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform houseTransform;

    [SerializeField] private GameObject merchantPrefab;
    public Transform spawnPoint; // posisi awal merchant (di luar layar)
    public Transform exitPoint;  // posisi keluar merchant (di luar layar)

    [SerializeField] private int woodStorage;
    [SerializeField] private int coinAmount;

    [SerializeField] private List<TreeResource> trees = new List<TreeResource>();

    public event Action<UnitSO> OnUnitSpawned;
    public event Action<int> OnWoodStorageUpdated;
    public event Action<int> OnCoinAmountUpdated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnUnit(UnitSO data)
    {
        var clone = Instantiate(unitPrefab);
        clone.transform.position = houseTransform.position;

        if (trees.Count > 0)
        {
            TreeResource nearestTree = trees[UnityEngine.Random.Range(0, trees.Count)];
            float minDist = Vector3.Distance(clone.transform.position, nearestTree.transform.position);

            UnitPrefab ai = clone.GetComponent<UnitPrefab>();
            if (ai != null)
                ai.Init(houseTransform, nearestTree);
        }

        OnUnitSpawned?.Invoke(data);
    }

    public void SetTree(TreeResource treeResource)
    {
        trees.Add(treeResource);
    }

    public void AddWood(int amount)
    {
        woodStorage += amount;
        OnWoodStorageUpdated?.Invoke(woodStorage);
    }

    public TreeResource[] GetTrees()
    {
        return trees.ToArray();
    }

    public void RemoveTree(TreeResource treeResource)
    {
        trees.Remove(treeResource);
    }

    public void CallMerchant()
    {
        if (merchantPrefab == null || houseTransform == null) { return; }

        GameObject merchantObj = Instantiate(merchantPrefab, spawnPoint.position, Quaternion.identity);
        MerchantPrefab merchant = merchantObj.GetComponent<MerchantPrefab>();
        merchant.Init(houseTransform, exitPoint.position);
    }

    public void AddCoin(int coinEarned)
    {
        coinAmount += coinEarned;
        OnCoinAmountUpdated?.Invoke(coinAmount);
    }

    public int GetWood()
    {
        return woodStorage;
    }

    public void SetWood(int value)
    {
        woodStorage = value;
    }
}
