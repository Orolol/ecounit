using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class City : MonoBehaviour
{
    public string name;
    public float money;
    public int x;
    public int z;
    public int prosperity;
    public int foodStock = 100;
    private int popGrowth = 0;
    public List<TradeItem> stock;
    public List<Pop> localPops;
    public List<Pop> leavingPops = new List<Pop>();
    public List<Industry> industries;
    private float timeStart;
    static float tickLength = 0.1f;
    public int tier;
    public Vector3 vector;
    public Dictionary<City, NavMeshPath> pathToCities;
    public Dictionary<City, float> distanceToCities;
    protected static string[] resources = { "IronOre", "Stone", "CopperOre", "GoldOre", "Forest", "fertileLand" };
    public static System.Random rand = new System.Random();
    public List<string> resourcesAvailable = new List<string>();
    public List<MilitaryComplex> militaryBuildings = new List<MilitaryComplex>();
    public int nbMilitia = 10;
    private int economyStack = 10;
    private int economySpeed = 10;
    public void initCity(string n, int xpos = 0, int zpos = 0, int tierd = 1)
    {
        tier = tierd;
        x = xpos;
        z = zpos;
        name = n;
        foodStock = 100 * tier;
        money = 0;
        prosperity = 100;
        industries = new List<Industry>();
        localPops = new List<Pop>();
        stock = new List<TradeItem>();
        stock.Add(new TradeItem("Wheat", 50));
        stock.Add(new TradeItem("Cow", 10));
        stock.Add(new TradeItem("Sheep", 0));
        stock.Add(new TradeItem("Fish", 0));
        stock.Add(new TradeItem("Iron", 0));
        stock.Add(new TradeItem("Stone", 0));
        stock.Add(new TradeItem("Gold", 0));
        stock.Add(new TradeItem("Copper", 0));
        stock.Add(new TradeItem("Wood", 0));
        stock.Add(new TradeItem("Sword", 30));
        stock.Add(new TradeItem("Bow", 0));
        stock.Add(new TradeItem("Arrow", 0));
        if (tier >= 1)
        {
            industries.Add(new Field());
            localPops.AddRange(WorldGenerator.generatePopEnMasse(1, this, 5));
            localPops.Add(WorldGenerator.generatePop(5, this));
            resourcesAvailable.Add(resources[rand.Next(resources.Length)]);
        }
        if (tier >= 2)
        {
            industries.Add(new Field());
            industries.Add(new Pasture());
            localPops.AddRange(WorldGenerator.generatePopEnMasse(1, this, 7));
            localPops.AddRange(WorldGenerator.generatePopEnMasse(2, this, 2));
            localPops.Add(WorldGenerator.generatePop(5, this));
            resourcesAvailable.Add(resources[rand.Next(resources.Length)]);
            militaryBuildings.Add(new Barrack(this));
        }
        if (tier >= 3)
        {
            industries.Add(new Field());
            industries.Add(new Pasture());
            localPops.AddRange(WorldGenerator.generatePopEnMasse(1, this, 15));
            localPops.AddRange(WorldGenerator.generatePopEnMasse(2, this, 2));
            localPops.Add(WorldGenerator.generatePop(5, this));
            resourcesAvailable.Add(resources[rand.Next(resources.Length)]);
        }
        economySpeed = rand.Next(10, 15);


    }

    public float getBuyPrice(TradeItem titem, int quant)
    {
        if (quant <= titem.quantity)
        {
            return titem.currentBuyPrice * quant * 1.05f;
        }
        else
        {
            return titem.currentBuyPrice * titem.quantity * 1.05f;
        }
    }
    public StockItem buyItems(TradeItem titem, float m, int quant)
    {
        int q = titem.takeStock(quant);
        if (q > 0)
        {
            money += m * 0.05f;

        }
        return new StockItem(new Item(titem.getName()), q);
    }
    public float getSellPrice(ItemContainer sitem, int quant)
    {
        TradeItem titem = retrieveFromStock(sitem.getName());
        if (titem != null)
        {
            if (quant <= titem.quantity)
            {
                return titem.currentSellPrice * quant * 0.95f;
            }
            else
            {
                return titem.currentSellPrice * titem.quantity * 0.95f;
            }
        }
        else
        {
            return 0.0f;
        }
    }
    public float SellItems(ItemContainer sitem, int quant)
    {
        TradeItem titem = retrieveFromStock(sitem.getName());
        if (titem != null)
        {
            money += titem.currentBuyPrice * sitem.quantity * 0.05f;
            titem.addStock(sitem.quantity);
            return titem.currentSellPrice * quant * 0.95f;
        }
        else
        {
            return 0.0f;
        }
    }
    void Update()
    {
        timeStart += Time.deltaTime;
        if (timeStart >= tickLength)
        {
            ticker();
            timeStart = 0.0f;
        }
    }
    public TradeItem retrieveFromStock(string name)
    {
        return stock.Find(x => x.getName() == name);
    }
    private int consumeFood(int n)
    {
        // print("Try to consume " + n);
        int consumed = 0;

        foreach (TradeItem item in stock)
        {
            while (item.getFoodValue() > 0 && consumed < n && item.quantity > 0)
            {
                consumed += item.takeStock(1) * item.getFoodValue();
            }
        }
        // print("Got " + consumed + " from resources");
        if (consumed < n && foodStock >= n - consumed)
        {
            foodStock -= (n - consumed);
            consumed = n;
            // print("enougth stock. Left :  " + foodStock);
        }
        else if (consumed < n)
        {
            consumed += foodStock;
            foodStock = 0;
            // print("took all)");
        }
        return consumed;
    }
    public void ticker()
    {
        if (localPops != null && localPops.Count > 0)
        {
            foreach (Pop p in localPops)
            {
                if (p != null && p.workplace == null)
                {
                    industries = p.lookingForJob(industries);
                }
                if (p != null && p.type == 5)
                {
                    ((Merchant)p).analyzeMarket(stock);
                }
            }
        }
        if (industries.Any())
        {
            foreach (Industry ind in industries)
            {
                // Debug.Log("FILL " + ind);
                stock = ind.fillStock(stock);
                List<StockItem> prod = ind.process();
                if (prod != null)
                {
                    foreach (StockItem item in prod)
                    {
                        TradeItem titem = stock.Find(x => x.getName() == item.getName());
                        titem.addStock(item.quantity);
                    }
                }
            }
        }

        if (foodStock >= 10 && prosperity >= 100)
        {
            popGrowth++;
            if (popGrowth >= 100)
            {
                localPops.Add(WorldGenerator.generatePop(1, this));
                popGrowth -= 100;
                // Debug.Log("New Péon spawn");
            }
        }
        economyStack++;
        if (economyStack >= economySpeed)
        {
            economyStack = 0;
            EconomicTurn();
        }

        if (foodStock < 100 * tier)
        {
            foreach (TradeItem item in stock)
            {
                while (item.getFoodValue() > 0 && foodStock < 100 * tier && item.quantity > 0)
                {
                    foodStock += item.takeStock(1) * item.getFoodValue();
                }
            }
        }

        foreach (Pop l in leavingPops)
        {
            localPops.Remove(l);
        }
        leavingPops = new List<Pop>();



    }
    private void EconomicTurn()
    {
        // if (nbMilitia > 0)
        // {
        //     nbMilitia -= 1;
        // }
        if (resourcesAvailable.Any())
        {
            // var res = resourcesAvailable[resourcesAvailable.Count - 1];
            foreach (string res in resourcesAvailable)
            {
                Industry ind = Industry.generateIndustryForResource(res);
                if (ind != null)
                {
                    // resourcesAvailable.RemoveAt(resourcesAvailable.Count - 1);
                    industries.Add(ind);
                }

            }
            resourcesAvailable = new List<string>();
        }
        consumeFood(localPops.Count);
        foreach (Pop p in localPops)
        {
            if (p.type == 2 && p.workplace == null)
            {
                industries = p.lookingForJob(industries);
                if (p.workplace == null)
                {
                    foreach (TradeItem item in stock)
                    {
                        if (item.quantity > 5)
                        {
                            Industry ind = Industry.generateIndustryOpportunity(item.getName());
                            if (ind != null)
                            {
                                ind.addPop(p);
                                industries.Add(ind);
                            }
                        }
                    }
                }
            }
            money += p.payTaxes();
        }
        if (militaryBuildings.Any() && nbMilitia < tier * 20)
        {
            // Debug.Log(name + " NEED MORE MILITIA" + nbMilitia);
            foreach (MilitaryComplex ind in militaryBuildings)
            {
                nbMilitia += ind.process();
                // print("GOT" + nbMilitia);
            }
        }

    }
}


// public static class WorldGenerator
// {
//     public void generateCities(List<City> cities)
//     {

//         foreach (City c in cities)
//         {
//             GameObject myRoadInstance = Instantiate(Resources.Load("CityObject")) as GameObject;

//         }

//     }
// }