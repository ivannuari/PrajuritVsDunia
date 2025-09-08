using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MerchantPrefab : MonoBehaviour
{
    [SerializeField] private int woodPerCoin = 3; // berapa kayu untuk 1 coin
    private bool hasArrived = false;

    private Transform house;
    private Vector3 exitPoint;

    private NavMeshAgent agent;
    private Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    public void Init(Transform houseTarget, Vector3 exitPosition)
    {
        house = houseTarget;
        exitPoint = exitPosition;

        // Jalan ke house
        agent.isStopped = false;
        agent.SetDestination(house.position);
    }

    private void Update()
    {
        if (anim != null)
        {
            anim.SetFloat("Walk", agent.velocity.magnitude);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasArrived && other.transform == house)
        {
            hasArrived = true;
            StartCoroutine(StopAtHouseCoroutine());
        }
    }

    private IEnumerator StopAtHouseCoroutine()
    {
        // Stop di depan house
        agent.isStopped = true;
        anim.SetFloat("Walk", 0);

        // Tunggu 2 detik
        yield return new WaitForSeconds(10f);

        // Tukar resource
        ExchangeResource();

        // Lanjut jalan keluar
        LeaveScene();
    }

    private void ExchangeResource()
    {
        int wood = GameSetting.Instance.GetWood();
        if (wood > 0)
        {
            // hitung coin berdasarkan rate
            int coinEarned = wood / woodPerCoin;
            int leftoverWood = wood % woodPerCoin;

            GameSetting.Instance.AddCoin(coinEarned);
            GameSetting.Instance.SetWood(leftoverWood); // sisa kayu yang tidak bisa ditukar

            Debug.Log($"Merchant menukar {wood} kayu → {coinEarned} coin, sisa {leftoverWood} kayu");
        }
    }

    private void LeaveScene()
    {
        agent.isStopped = false;
        agent.SetDestination(exitPoint);
        Destroy(gameObject, 10f); // auto destroy
    }
}
