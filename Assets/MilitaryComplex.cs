using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MilitaryComplex
{
    protected City city;
    public float money;
    public abstract int process();


}



public class Barrack : MilitaryComplex
{
    public Barrack(City c)
    {
        city = c;
        money = 500;
    }
    public override int process()
    {
        // Debug.Log("recruit " + city.localPops.Count / 10);
        int nbRecruit = (city.localPops.Count / 10);
        TradeItem weaponToBuy = city.retrieveFromStock("Sword");
        float cost = city.getBuyPrice(weaponToBuy, nbRecruit);
        // Debug.Log("Cost : " + cost);
        if (city.money >= cost)
        {
            StockItem weapons = city.buyItems(city.retrieveFromStock("Sword"), cost, nbRecruit);
            city.money -= cost;
            return weapons.quantity;
        }
        return 0;
    }
}


public abstract class MilitaryUnit
{
    public int strength;
    public bool ranged;
    public bool cavalry;
    public string name;
}

public class Militia : MilitaryUnit
{
    public Militia()
    {
        name = "Militia";
        ranged = false;
        cavalry = false;
        strength = 15;
    }
}