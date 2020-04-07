using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorEngine : MonoBehaviour
{

    // public List<City> cities = new List<City>();
    // private float timeStart;
    // static float tickLength = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        // City c1 = new City("Paris", 15, 2);
        // City c2 = new City("Melun", -25, 19);
        // cities.Add(c1);
        // cities.Add(c2);
        WorldGenerator.generateCities();
    }
    // Update is called once per frame

}

