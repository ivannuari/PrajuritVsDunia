using UnityEngine;

public class TreeResource : MonoBehaviour
{
    public int woodAmount = 20;

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
        return gathered;
    }
}