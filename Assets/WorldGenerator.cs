using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class WorldGenerator : MonoBehaviour
{

    public static string[] names = { "Paris", "Stormwind", "Melun", "Bobigny", "Meaux", "Caed Nua", "Baldur's Gate", "Meaux 2", "Tanchico", "Minas Tirith", "Ironforge", "Night City", "Londres", "Caemlyn", "Tear", "Illian", "Dijon" };
    public static string[] popNames = { "Jean", "Merlin", "Gaston", "Illidan", "Orolol", "Robespierre" };
    public static System.Random rand = new System.Random();

    public static List<City> listCities = new List<City>();

    static WorldGenerator()
    {

    }

    public static Pop generatePop(int type, City c)
    {
        if (type == 1)
        {
            Worker p = new Worker(popNames[rand.Next(popNames.Length)], type);
            p.city = c;
            return p;
        }
        if (type == 2)
        {
            Artisan p = new Artisan(popNames[rand.Next(popNames.Length)]);
            p.city = c;
            return p;
        }
        if (type == 5)
        {
            Merchant p = new Merchant(popNames[rand.Next(popNames.Length)]);
            p.city = c;
            return p;
        }
        return null;
    }
    public static List<Pop> generatePopEnMasse(int type, City c, int q)
    {
        List<Pop> l = new List<Pop>();
        for (int i = 0; i < q; i++)
        {
            Pop p = generatePop(type, c);
            if (p != null)
            {
                l.Add(p);
            }
        }
        // print("ADD POP" + l.Count);
        return l;
    }

    public static void generateMerchant(City from, City to, List<StockItem> items, Merchant pop)
    {
        GameObject merchantObject = Instantiate(Resources.Load("Merchant"), from.vector, Quaternion.identity) as GameObject;
        merchantObject.transform.localScale = new Vector3(10, 10, 10);
        // Debug.Log("MERCH AT " + from.name + " TO " + to.name);
        MerchantBehaviour m = merchantObject.GetComponent<MerchantBehaviour>();
        // Debug.Log(from.name);
        // Debug.Log(to.name);
        // Debug.Log(from.pathToCities[to]);
        // Debug.Log("FIND" + from.pathToCities[to]);
        // Debug.Log("OK");
        m.MerchantUnitInit(from.pathToCities[to], items, to, pop);
    }


    public static void generateCities()
    {
        // Random rand = new Random();
        for (int i = 0; i < names.Length; i++)
        {
            string name = names[i];
            int x = rand.Next(-500, 500);
            int z = rand.Next(-500, 500);
            int tier = rand.Next(1, 4);
            Vector3 v = new Vector3(x, 0, z);
            GameObject CityObject = Instantiate(Resources.Load("CityObject"), v, Quaternion.identity) as GameObject;
            CityObject.transform.localScale = new Vector3(tier, tier, tier); ;
            City cb = CityObject.AddComponent<City>();
            cb.vector = v;
            cb.initCity(name, x, z, tier);
            Transform t = CityObject.transform.Find("Canvas").GetChild(0);
            Text cityName = t.GetComponent<Text>();
            cityName.text = name;
            listCities.Add(cb);
        }






        GameObject[] cc = GameObject.FindGameObjectsWithTag("CityCube");

        foreach (GameObject go in cc)
        {
            CityMouse cm = go.AddComponent<CityMouse>();
            City ccc = go.GetComponentInParent<City>();
            cm.city = ccc;
        }
        foreach (City origin in listCities)
        {
            origin.distanceToCities = new Dictionary<City, float>();
            origin.pathToCities = new Dictionary<City, NavMeshPath>();
            foreach (City target in listCities)
            {
                if (target != origin)
                {
                    NavMeshPath path = new NavMeshPath();
                    NavMesh.CalculatePath(origin.vector, target.vector, NavMesh.AllAreas, path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        float distance = 0.0f;
                        Vector3 previous = path.corners[0];
                        foreach (Vector3 corner in path.corners)
                        {
                            if (previous != corner)
                            {
                                distance += Vector3.Distance(previous, corner);
                            }
                            previous = corner;
                        }
                        origin.distanceToCities.Add(target, distance);
                        origin.pathToCities.Add(target, path);
                    }
                    else
                    {
                        Debug.Log(path.status.ToString() + origin.vector.ToString() + target.vector.ToString());
                    }

                }
            }
        }

    }
}

public struct CityData
{
    public string name;
    public int x;
    public int z;

    public CityData(string n, int xpos, int zpos)
    {
        name = n;
        x = xpos;
        z = zpos;
    }
}