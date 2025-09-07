using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    public static GameSetting Instance;

    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform houseTransform;

    [SerializeField] private int woodStorage;

    [SerializeField] private List<TreeResource> trees = new List<TreeResource>();

    public event Action<UnitSO> OnUnitSpawned;

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

    public void AddWood(int carryingWood)
    {

    }

    public TreeResource[] GetTrees()
    {
        return trees.ToArray();
    }
}
