using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class UnitPrefab : MonoBehaviour
{
    public int carryCapacity = 5;
    public float gatherTime = 5f;

    private int carryingWood = 0;
    private bool isGathering = false;

    private NavMeshAgent agent;
    private TreeResource targetTree;
    private Transform house;
    private Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    public void Init(Transform houseBase, TreeResource nearestTree)
    {
        house = houseBase;
        targetTree = nearestTree;
        GoToTree();
    }

    private void Update()
    {
        if (anim != null) 
        {
            anim.SetFloat("Walk", agent.velocity.magnitude);
        }
    }

    private void GoToTree()
    {
        if (targetTree != null && targetTree.HasWood())
        {
            agent.isStopped = false;
            agent.SetDestination(targetTree.transform.position);
        }
        else
        {
            // cari pohon lain
            TreeResource newTree = FindNearestTree();
            if (newTree != null)
            {
                targetTree = newTree;
                agent.isStopped = false;
                agent.SetDestination(targetTree.transform.position);
            }
            else
            {
                Debug.Log($"{name} tidak ada pohon tersisa, berhenti bekerja.");
            }
        }
    }

    private void GoToHouse()
    {
        if (house != null)
        {
            agent.isStopped = false;
            agent.SetDestination(house.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isGathering && other.GetComponent<TreeResource>() == targetTree)
        {
            StartCoroutine(GatherCoroutine());
        }

        if (other.transform == house && carryingWood > 0)
        {
            // Tambah resource
            GameSetting.Instance.AddWood(carryingWood);
            carryingWood = 0;

            // Kembali ke tree lain kalau kayu habis
            GoToTree();
        }
    }

    private IEnumerator GatherCoroutine()
    {
        isGathering = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(gatherTime);

        int gathered = targetTree.GatherWood(carryCapacity);
        carryingWood = gathered;

        isGathering = false;

        GoToHouse();
    }

    private TreeResource FindNearestTree()
    {
        TreeResource[] trees = GameSetting.Instance.GetTrees();
        TreeResource nearest = null;
        float minDist = Mathf.Infinity;

        foreach (TreeResource t in trees)
        {
            if (!t.HasWood()) continue; // skip pohon habis

            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = t;
            }
        }

        return nearest;
    }
}