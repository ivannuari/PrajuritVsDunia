using System;
using UnityEngine;

public class TreeResource : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private int woodAmount = 20;
    [SerializeField] private float gatherTime = 10f;

    private void Start()
    {
        GameSetting.Instance.SetTree(this);
    }

    public bool HasWood()
    {
        return woodAmount > 0;
    }

    public int GatherWood(int amount)
    {
        int gathered = Mathf.Min(amount, woodAmount);
        woodAmount -= gathered;

        if (woodAmount <= 0)
        {
            // hapus dari daftar GameSetting dan destroy
            GameSetting.Instance.RemoveTree(this);
            Destroy(gameObject);
        }

        return gathered;
    }

    public float GetGatherTime()
    {
        return gatherTime;
    }
}