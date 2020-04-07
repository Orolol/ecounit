using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public string name;
    private int money;
    public int x;
    public int z;
    public int prosperity;
    private int popGrowth = 0;
    public Dictionary<string, int> stock;
    public List<Pop> localPops;
    public List<Industry> industries;
    private float timeStart;
    static float tickLength = 0.1f;
    public int tier;
    public void initCity(string n, int xpos = 0, int zpos = 0, int tierd = 1)
    {
        tier = tierd;
        x = xpos;
        z = zpos;
        name = n;
        money = 0;
        prosperity = 100;
        industries = new List<Industry>();
        localPops = new List<Pop>();
        stock = new Dictionary<string, int>();
        stock.Add("Wheat", 0);
        stock.Add("Cow", 0);
        stock.Add("Iron", 0);
        if (tier >= 1)
        {
            industries.Add(new Field("field1"));
            industries.Add(new Pasture("pasturo"));
            localPops.Add(new Worker("Jean", 1));
            localPops.Add(new Worker("Paul", 1));
            localPops.Add(new Worker("Deux", 1));
        }
        if (tier >= 2)
        {
            industries.Add(new Field("field1"));
            industries.Add(new Pasture("pasturo"));
            localPops.Add(new Worker("Jean", 1));
            localPops.Add(new Worker("Paul", 1));
            localPops.Add(new Worker("Deux", 1));
        }
        if (tier >= 3)
        {
            industries.Add(new Field("field1"));
            industries.Add(new Pasture("pasturo"));
            localPops.Add(new Worker("Jean", 1));
            localPops.Add(new Worker("Paul", 1));
            localPops.Add(new Worker("Deux", 1));
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

    private int foodStock()
    {
        int food = 0;
        foreach (KeyValuePair<string, int> entry in stock)
        {
            switch (entry.Key)
            {
                case "Wheat":
                    food += entry.Value;
                    break;
                case "Cow":
                    food += entry.Value * 3;
                    break;
            }
        }

        return food;
    }

    public void ticker()
    {

        foreach (Pop p in localPops)
        {
            if (p.workplace == null)
            {
                industries = p.lookingForJob(industries);
            }
        }

        foreach (Industry ind in industries)
        {
            stock = ind.fillStock(stock);
            Dictionary<string, int> prod = ind.process();
            if (prod != null)
            {
                foreach (KeyValuePair<string, int> entry in prod)
                {
                    stock[entry.Key] += entry.Value;
                }
            }

        }

        if (foodStock() >= 10 && prosperity >= 100)
        {
            popGrowth++;
            if (popGrowth >= 100)
            {
                localPops.Add(new Worker("Péon", 1));
                popGrowth -= 100;
                Debug.Log("New Péon spawn");
            }
        }

    }

}

public abstract class Pop
{
    public string name;
    public int type;

    public Industry workplace = null;

    public void setWorkplace(Industry i)
    {
        workplace = i;
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



public class Worker : Pop
{

    private float work;
    public Worker(string n, int t)
    {
        name = n;
        type = t;
        work = 0;
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

}

public abstract class Industry
{
    public List<Pop> workers;
    protected float work;
    public string name;
    public int availableJobs;
    public int neededJob;
    protected int workNeeded;
    public Dictionary<string, int> production;
    public Dictionary<string, int> components;
    public Dictionary<string, int> stockItem;
    public Dictionary<string, int> fillStock(Dictionary<string, int> p)
    {
        if (components != null)
        {
            foreach (KeyValuePair<string, int> entry in components)
            {
                if (p[entry.Key] >= entry.Value)
                {
                    stockItem[entry.Key] += entry.Value;
                    p[entry.Key] -= entry.Value;
                    // Debug.Log ("Stock filled at " + this.name);
                }
            }
        }
        return p;
    }
    public bool addPop(Pop p)
    {
        if (p.type == neededJob && availableJobs > 0)
        {
            workers.Add((Worker)p);
            availableJobs -= 1;
            return true;
        }
        else
        {
            Debug.Log("Wasnt a worker or wp full");
            return false;
        }

    }
    public Dictionary<string, int> process()
    {
        // Debug.Log("Let'sproduce at " + this.name);
        foreach (Worker w in workers)
        {
            // Debug.Log("Go to work :" + w.name);
            work += w.working();
        }
        if (work >= workNeeded)
        {
            if (components != null)
            {
                foreach (KeyValuePair<string, int> entry in components)
                {
                    if (stockItem.ContainsKey(entry.Key) && stockItem[entry.Key] >= entry.Value)
                    {
                        stockItem[entry.Key] -= entry.Value;
                        work -= workNeeded;
                        // Debug.Log (this.name + " produce " + production);
                        return production;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                // Debug.Log (this.name + " produce " + production);
                work -= workNeeded;
                return production;
            }

        }
        return null;
    }

}

public class Field : Industry
{
    public Field(string n)
    {
        work = 0;
        workers = new List<Pop>();
        name = n;
        production = new Dictionary<string, int>();
        production.Add("Wheat", 1);
        availableJobs = 5;
        neededJob = 1;
        workNeeded = 20;
    }
}

public class Pasture : Industry
{
    public Pasture(string n)
    {
        work = 0;
        workers = new List<Pop>();
        name = n;
        production = new Dictionary<string, int>();
        production.Add("Cow", 1);
        components = new Dictionary<string, int>();
        stockItem = new Dictionary<string, int>();
        production.Add("Wheat", 1);
        availableJobs = 4;
        neededJob = 1;
        workNeeded = 30;
    }

}

class IndustrySortPerJob : IComparer<Industry>
{
    public int Compare(Industry x, Industry y)
    {
        return y.availableJobs.CompareTo(x.availableJobs);

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