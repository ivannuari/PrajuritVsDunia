using UnityEngine;

public class AssetManager : MonoBehaviour
{
    [SerializeField] private UnitSO[] allUnitDatas;

    public UnitSO[] GetAllUnitData()
    {
        return allUnitDatas;
    }
}
