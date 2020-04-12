using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Pop
{
    public string name;
    public int type;

    public City city;
    public Industry workplace = null;
    protected float work = 0;

    public abstract int payTaxes();

    public void setWorkplace(Industry i)
    {
        workplace = i;
    }

    public float working()
    {
        work += 1.0f;
        if (work >= 1)
        {
            work = 0;
            return 1.0f;
        }
        else { return 0; }
    }
    public List<Industry> lookingForJob(List<Industry> inds)
    {
        inds.Sort(new IndustrySortPerJob());
        foreach (Industry ind in inds)
        {
            // Debug.Log (this.name + " looking at " + ind.name + " //  available : " + ind.availableJobs);
            if (ind.availableJobs > 0 && ind.neededJob == this.type)
            {
                this.workplace = ind;
                ind.addPop(this);
                // Debug.Log (this.name + " found a job at " + ind.name);
                break;
            }
        }
        return inds;
    }
}

public class Artisan : Pop
{
    public float money;
    public Artisan(string n)
    {
        name = n;
        type = 2;
        money = 500.0f;
    }

    public override int payTaxes()
    {
        return 3;
    }

    public void lookingForWorkshop()
    {

    }
}

public class Merchant : Pop
{

    public float money;
    public Merchant(string n)
    {
        name = n;
        type = 5;
        money = 2000.0f;
    }
    public override int payTaxes()
    {
        return 5;
    }
    public List<StockItem> analyzeMarket(List<TradeItem> stock)
    {
        if (city != null)
        {
            List<StockItem> merchantStock = new List<StockItem>();
            foreach (TradeItem titem in stock)
            {
                if (titem.quantity > 20)
                {
                    // Debug.Log("Business opportunity in " + city.name +" for " + titem.getName() );
                    float d = 999.0f;
                    City t = null;
                    foreach (KeyValuePair<City, float> entry in city.distanceToCities)
                    {

                        if (entry.Key.getSellPrice(titem, 20) / entry.Value < d && entry.Key.getSellPrice(titem, 20) * 1.05f > city.getBuyPrice(titem, 20))
                        {
                            t = entry.Key;
                            d = entry.Key.getSellPrice(titem, 20) / entry.Value;
                        }
                    }
                    if (t != null)
                    {
                        float m = city.getBuyPrice(titem, 20);
                        StockItem s = city.buyItems(titem, m, 20);
                        s.cost = m / 20;
                        merchantStock.Add(s);
                        money -= m;
                        WorldGenerator.generateMerchant(city, t, merchantStock, this);
                        city.leavingPops.Add(this);
                        city = null;

                    }

                }
            }
        }
        return null;
    }
}



public class Worker : Pop
{


    public Worker(string n, int t)
    {
        name = n;
        type = t;
    }
    public override int payTaxes()
    {
        return 2;
    }
    // public override float working()
    // {
    //     work += 1.0f;
    //     if (work >= 1)
    //     {
    //         work = 0;
    //         return 1.0f;
    //     }
    //     else { return 0; }
    // }

}
