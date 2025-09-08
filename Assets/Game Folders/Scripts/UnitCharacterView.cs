using System;
using UnityEngine;

public class UnitCharacterView : MonoBehaviour
{
    [SerializeField] private GameObject[] allHeads;

    private void Awake()
    {
        RandomHead();
    }

    private void RandomHead()
    {
        foreach (GameObject head in allHeads)
        {
            head.SetActive(false);
        }

        int _rand = UnityEngine.Random.Range(0, allHeads.Length);
        allHeads[_rand].SetActive(true);
    }
}
