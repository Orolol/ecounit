using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MerchantBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshPath path;
    public List<StockItem> items;
    public City goal;
    public Vector3 currentPos;
    public NavMeshAgent agent;
    public float money = 0.0f;
    private string status = "waiting";
    public Merchant pop;

    public void MerchantUnitInit(NavMeshPath p, List<StockItem> i, City city, Merchant m)
    {
        path = p;
        items = i;
        goal = city;
        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        agent.SetPath(p);
        status = "busy";
        pop = m;
    }

    void Start()
    {
        // Debug.Log("MERCHANT CREATED");
    }

    void Update()
    {

        if (agent != null && agent.remainingDistance < 10 && status == "busy")
        {
            foreach (StockItem sitem in items)
            {
                // Debug.Log("SELLING " + sitem.quantity + sitem.getName());
                float money = goal.SellItems(sitem, sitem.quantity);
                sitem.quantity = 0;
                pop.money += money;
            }
            status = "waiting";
            // Debug.Log("FINITO" + money);
            goal.localPops.Add(pop);
            pop.city = goal;
            GameObject.Destroy(gameObject);
        }

        // GameObject.Destroy(gameObject);
        // agent.
        // Debug.Log(path);
        // if (path.status == NavMeshPathStatus.PathComplete)
        // {
        //     Debug.Log("FINITO");
        //     // goal.stock
        // }

    }


}
