using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitBehaviour : MonoBehaviour
{
    public List<StockItem> inventory;
    public float money;
    public int nbSoldier = 100;
    public City cityAttached = null;
    protected Camera cam;
    private RectTransform panelPos;
    // Start is called before the first frame update
    void Start()
    {
        inventory = new List<StockItem>();
        money = 100;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        panelPos = gameObject.GetComponentsInChildren<RectTransform>()[1];

    }

    public void openTradeInterface()
    {
        Debug.Log("TRADE WITH " + cityAttached);
        GameObject panelCity = GameObject.Find("TradePanel");
        panelCity.GetComponent<CanvasGroup>().alpha = 1;
        print(panelCity.transform.GetChildCount());
        // panelCity.
    }

    // Update is called once per frame
    void Update()
    {
        if (cityAttached == null)
        {
            foreach (City city in WorldGenerator.listCities)
            {
                if ((this.transform.position - city.vector).sqrMagnitude < 5 * 5)
                {
                    cityAttached = city;
                    Debug.Log("PLAYER IN " + city.name);
                    this.GetComponentInChildren<CanvasGroup>().alpha = 1;
                }
            }
        }
        else
        {
            if ((this.transform.position - cityAttached.vector).sqrMagnitude > 5 * 5)
            {
                this.GetComponentInChildren<CanvasGroup>().alpha = 0;
                cityAttached = null;
            }
            else
            {
                Vector3 screenPos = cam.WorldToScreenPoint(gameObject.transform.position);
                panelPos.anchoredPosition3D = screenPos;
            }
        }

    }
}
