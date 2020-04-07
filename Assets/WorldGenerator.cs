using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WorldGenerator : MonoBehaviour
{

    public static string[] names = { "Paris", "Stormwind", "Melun", "Minas Tirith", "Ironforge", "Night City", "Londres", "Caemlyn", "Tear", "Illian", "Dijon" };
    public static System.Random rand = new System.Random();

    static WorldGenerator()
    {

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
            GameObject CityObject = Instantiate(Resources.Load("CityObject"), new Vector3(x, 0, z), Quaternion.identity) as GameObject;
            CityObject.transform.localScale = new Vector3(tier, tier, tier); ;
            City cb = CityObject.AddComponent<City>();
            cb.initCity(name, x, z, tier);
            Transform t = CityObject.transform.Find("Canvas").GetChild(0);
            Text cityName = t.GetComponent<Text>();
            cityName.text = name;
        }






        GameObject[] cc = GameObject.FindGameObjectsWithTag("CityCube");

        foreach (GameObject go in cc)
        {
            CityMouse cm = go.AddComponent<CityMouse>();
            City ccc = go.GetComponentInParent<City>();
            cm.city = ccc;
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