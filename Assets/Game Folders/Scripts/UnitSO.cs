using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Create New/Unit")]
public class UnitSO : ScriptableObject
{
    public string unitName;
    public int unitId;
    public Sprite icon;
    public int cost;

    public bool isLock;
}
