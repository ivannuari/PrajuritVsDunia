using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    public static GameSetting Instance;

    [SerializeField] private int woodStorage;
    [SerializeField] private int coinAmount;

    [Header("UNIT OBJECT")]
    [SerializeField] private GameObject[] unitPrefab;
    [SerializeField] private Transform houseTransform;

    [Header("MERCHANT OBJECT")]
    [SerializeField] private GameObject merchantPrefab;
    [SerializeField] private Transform spawnPoint; // posisi awal merchant (di luar layar)
    [SerializeField] private Transform exitPoint;  // posisi keluar merchant (di luar layar)

    [Header("RESOURCES OBJECT")]
    [SerializeField] private GameObject[] resourcesObject;
    [SerializeField] private Transform treeContent;
    [SerializeField] private NavMeshSurface groundSurface;

    private List<TreeResource> trees = new List<TreeResource>();

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

        SpawnTrees();
    }

    private void SpawnTrees()
    {
        float radius = 50f;
        float innerExclusion = 10f; // area kosong di tengah

        for (int i = 0; i < 100; i++)
        {
            Vector3 pos;
            do
            {
                pos = UnityEngine.Random.insideUnitSphere * radius;
                pos.y = 0f;
            }
            while (Vector3.Distance(Vector3.zero, pos) < innerExclusion);

            var prefab = resourcesObject[UnityEngine.Random.Range(0, resourcesObject.Length)];
            var clone = Instantiate(prefab, pos, Quaternion.identity, treeContent);
        }

        // rebuild navmesh setelah semua pohon spawn
        groundSurface.BuildNavMesh();
    }

    public void SpawnUnit(UnitSO data)
    {
        var clone = Instantiate(unitPrefab[data.unitId]);
        clone.transform.position = houseTransform.position;

        if (trees.Count > 0)
        {
            TreeResource nearestTree = null;
            float minDist = Mathf.Infinity;

            foreach (var tree in trees)
            {
                if (tree == null) continue;
                if (!tree.HasWood()) continue;
                if (tree.IsReserved()) continue;

                float dist = Vector3.Distance(clone.transform.position, tree.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestTree = tree;
                }
            }

            if (nearestTree != null)
            {
                nearestTree.Reserve();

                UnitPrefab ai = clone.GetComponent<UnitPrefab>();
                if (ai != null)
                    ai.Init(houseTransform, nearestTree);
            }
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

    public int GetCoin()
    {
        return coinAmount;
    }
}
