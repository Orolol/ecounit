using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MouseBehavior : MonoBehaviour
{

    private bool isCamMoving = false;
    public int speed = 2;

    private NavMeshAgent isUnitSelect = null;

    private City cityToShow = null;
    private MerchantBehaviour unitToShow = null;
    private PlayerUnitBehaviour playerUnitToShow = null;

    private int screenHeight = Screen.height;
    private int screenWidth = Screen.width;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = transform.position;
        if (Input.mousePosition.x > screenWidth - 30 && camPos.x < 500)
        {
            isCamMoving = true;
            camPos.x += speed * (Time.deltaTime + 1);
        }
        else if (Input.mousePosition.x < 30 && camPos.x > -500)
        {
            isCamMoving = true;
            camPos.x -= speed * (Time.deltaTime + 1);
        }

        else if (Input.mousePosition.y > screenHeight - 30 && camPos.z < 500)
        {
            isCamMoving = true;
            camPos.z += speed * (Time.deltaTime + 1);
        }
        else if (Input.mousePosition.y < 30 && camPos.z > -500)
        {
            isCamMoving = true;
            camPos.z -= speed * (Time.deltaTime + 1);
        }
        else
        {
            isCamMoving = false;
        }

        transform.position = camPos;

        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");
        if (ScrollWheelChange != 0)
        {
            float R = ScrollWheelChange * 15;                                   //The radius from current camera
            float PosX = Camera.main.transform.eulerAngles.x + 90;              //Get up and down
            float PosY = -1 * (Camera.main.transform.eulerAngles.y - 90);       //Get left to right
            PosX = PosX / 180 * Mathf.PI;                                       //Convert from degrees to radians
            PosY = PosY / 180 * Mathf.PI;                                       //^
            float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);                    //Calculate new coords
            float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);                    //^
            float Y = R * Mathf.Cos(PosX);                                      //^
            float CamX = Camera.main.transform.position.x;                      //Get current camera postition for the offset
            float CamY = Camera.main.transform.position.y;                      //^
            float CamZ = Camera.main.transform.position.z;                      //^
            Camera.main.transform.position = new Vector3(CamX + X, CamY + Y, CamZ + Z);//Move the main camera
        }


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
                if (hit.transform != null && hit.transform.gameObject.name == "CityCube")
                {
                    City c = hit.transform.gameObject.GetComponent<CityMouse>().city;
                    GameObject panelCity = GameObject.Find("PanelCity");
                    panelCity.GetComponent<CanvasGroup>().alpha = 1;
                    cityToShow = c;
                }
                else if (hit.transform != null && hit.transform.gameObject.name == "MerchantCylinder")
                {
                    MerchantBehaviour c = hit.transform.gameObject.GetComponentInParent<MerchantBehaviour>();
                    GameObject panelUnit = GameObject.Find("PanelUnit");
                    panelUnit.GetComponent<CanvasGroup>().alpha = 1;
                    unitToShow = c;
                }
                else if (hit.transform != null && hit.transform.gameObject.name == "PlayerCube")
                {
                    NavMeshAgent c = hit.transform.gameObject.GetComponentInParent<NavMeshAgent>();
                    unitToShow = null;
                    isUnitSelect = c;
                    GameObject panelUnit = GameObject.Find("PanelUnit");
                    panelUnit.GetComponent<CanvasGroup>().alpha = 1;
                    playerUnitToShow = hit.transform.gameObject.GetComponentInParent<PlayerUnitBehaviour>();
                }
                else
                {
                    Debug.Log("Loupe " + hit.transform.gameObject.name);
                    GameObject panelCity = GameObject.Find("PanelCity");
                    panelCity.GetComponent<CanvasGroup>().alpha = 0;
                    Debug.Log(hit.transform.gameObject.name);
                    cityToShow = null;
                    unitToShow = null;
                    isUnitSelect = null;
                }
        }

        if (Input.GetMouseButtonDown(1) && isUnitSelect != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {

                isUnitSelect.SetDestination(hit.point);
                // isUnitSelect.
                // hit.transform
                Debug.Log("GO TO" + hit.transform.position);
            }

        }


        if (unitToShow != null)
        {
            GameObject panelCity = GameObject.Find("PanelUnit");
            Text unitDetail = panelCity.GetComponentInChildren<Text>();
            unitDetail.text = unitToShow.name + "\r\n";
            unitDetail.text += unitToShow.pop.money + "\r\n";
            unitDetail.text += "-------------------------- \r\n";
            unitDetail.text += "Inventory \r\n\r\n";
            foreach (StockItem item in unitToShow.items)
            {
                if (item.quantity > 0)
                {
                    unitDetail.text += item.getName() + " : " + item.quantity + " bought at " + item.cost + " \r\n";
                }
            }
            unitDetail.text += unitToShow.goal.name + "\r\n";
            unitDetail.text += "-------------------------- \r\n";
            unitDetail.text += "City stock \r\n\r\n";
            foreach (TradeItem item in unitToShow.goal.stock)
            {
                // if(item.quantity > 0)
                unitDetail.text += item.getName() + " : " + item.quantity + " | " + item.currentBuyPrice + "/" + item.currentSellPrice + "\r\n";
            }



        }
        if (playerUnitToShow != null)
        {
            GameObject panelCity = GameObject.Find("PanelUnit");
            Text unitDetail = panelCity.GetComponentInChildren<Text>();
            // unitDetail.text = playerUnitToShow.name + "\r\n";
            unitDetail.text = "Soldiers : " + playerUnitToShow.nbSoldier + "\r\n";
            unitDetail.text = "Money : " + playerUnitToShow.money + "\r\n";
            unitDetail.text += "-------------------------- \r\n";
            unitDetail.text += "Inventory \r\n\r\n";
            foreach (StockItem item in playerUnitToShow.inventory)
            {
                if (item.quantity > 0)
                {
                    unitDetail.text += item.getName() + " : " + item.quantity + " bought at " + item.cost + " \r\n";
                }
            }

        }
        if (cityToShow != null)
        {
            GameObject panelCity = GameObject.Find("PanelCity");
            Text cityDetail = panelCity.GetComponentInChildren<Text>();
            cityDetail.text = cityToShow.name + "\r\n";
            cityDetail.text = "Food stock : " + cityToShow.foodStock + "\r\n";
            cityDetail.text += "City tier " + cityToShow.tier + "\r\n";
            cityDetail.text += "City Treasure : " + cityToShow.money + "\r\n";
            cityDetail.text += "Militia : " + cityToShow.nbMilitia + "\r\n";
            cityDetail.text += "-------------------------- \r\n";
            cityDetail.text += "City Pop \r\n\r\n";
            int nbWorker = 0;
            int nbWMerc = 0;
            foreach (Pop i in cityToShow.localPops)
            {
                if (i.type == 1) nbWorker++;
                if (i.type == 5) nbWMerc++;
            }
            foreach (Pop i in cityToShow.localPops)
            {
                if (i.type == 5)
                {
                    cityDetail.text += "Merchant :" + i.name + " // money : " + ((Merchant)i).money + "\r\n";
                }
            }
            cityDetail.text += "Workers :" + nbWorker + "\r\n";
            cityDetail.text += "Merchants :" + nbWMerc + "\r\n";
            cityDetail.text += "-------------------------- \r\n";
            cityDetail.text += "City ressources : ";
            foreach (string i in cityToShow.resourcesAvailable)
            {
                if (i != null)
                {
                    cityDetail.text += i + " @ ";
                }
            }
            cityDetail.text += "\r\n";
            cityDetail.text += "-------------------------- \r\n";
            cityDetail.text += "City industries \r\n\r\n";
            foreach (Industry i in cityToShow.industries)
            {

                cityDetail.text += i.GetType().ToString() + " : " + i.workers.Count + "workers \r\n";
            }
            cityDetail.text += "-------------------------- \r\n";
            cityDetail.text += "City stock \r\n\r\n";
            foreach (TradeItem item in cityToShow.stock)
            {
                if (item.quantity > 0)
                {
                    cityDetail.text += item.getName() + " : " + item.quantity + " | buy at " + Math.Round(item.currentBuyPrice, 2) + "/ sell at " + Math.Round(item.currentSellPrice, 2) + "\r\n";
                }
            }
            // cityDetail.text += "-------------------------- \r\n";
            // cityDetail.text += "Neighbours \r\n\r\n";
            // foreach (KeyValuePair<City, float> c in cityToShow.distanceToCities)
            // {
            //     // if(item.quantity > 0)
            //     cityDetail.text += c.Key.name + " : " + c.Value + "\r\n";
            // }
            // cityDetail.text += "-------------------------- \r\n";
            // cityDetail.text += "City pops \r\n\r\n";


        }
    }


}
