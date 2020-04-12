using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Industry
{
    public List<Pop> workers;
    protected float work;

    public int availableJobs;
    public int neededJob;
    protected int workNeeded;
    public List<StockItem> production = new List<StockItem>();
    public List<StockItem> components;
    public List<StockItem> stockItem;
    public List<TradeItem> fillStock(List<TradeItem> p)
    {
        if (components != null)
        {
            foreach (StockItem citem in components)
            {
                TradeItem titem = p.Find(x => x.getName() == citem.getName());
                StockItem sitem = stockItem.Find(x => x.getName() == citem.getName());

                if (titem != null && sitem != null && sitem.quantity < citem.quantity)
                {
                    sitem.addStock(titem.takeStock(citem.quantity));
                }
            }
        }
        return p;
    }
    public bool addPop(Pop p)
    {
        if (p.type == neededJob && availableJobs > 0)
        {
            workers.Add(p);
            availableJobs -= 1;
            return true;
        }
        else
        {
            Debug.Log("Wasnt a worker or wp full");
            return false;
        }

    }
    public List<StockItem> process()
    {
        if (workers.Count > 0)
        {
            foreach (Pop w in workers)
            {
                work += w.working();
            }
        }
        if (work >= workNeeded)
        {
            if (components != null)
            {
                foreach (StockItem citem in components)
                {
                    StockItem sitem = stockItem.Find(x => x.getName() == citem.getName());

                    if (sitem != null && sitem.quantity >= citem.quantity)
                    {
                        int r = sitem.takeStock(citem.quantity, true);
                        if (r == citem.quantity)
                        {
                            work -= workNeeded;
                            // Debug.Log (this.name + " produce " + production);
                            return production;
                        }
                        else
                        {
                            return null;
                        }
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

    public static Industry generateIndustryForResource(string res)
    {
        switch (res)
        {
            case "IronOre":
                return new IronMine();
            case "GoldOre":
                return new GoldMine();
            case "CopperOre":
                return new CopperMine();
            case "Stone":
                return new Quarry();
            case "fertileLand":
                return new Field();
            case "Forest":
                return new Lumberjack();
            default:
                return null;
        }
    }
    public static Industry generateIndustryOpportunity(string res)
    {
        switch (res)
        {
            case "Iron":
                return new Blacksmith();
            case "Wood":
                return new Woodworkshop();
            default:
                return null;
        }
    }



}

public class Field : Industry
{
    public Field()
    {
        work = 0;
        workers = new List<Pop>();
        production = new List<StockItem>();
        production.Add(new StockItem("Wheat", 1));
        availableJobs = 3;
        neededJob = 1;
        workNeeded = 10;

    }
}

public class Pasture : Industry
{
    public Pasture()
    {
        work = 0;
        workers = new List<Pop>();
        production.Add(new StockItem("Cow", 1));
        // components = new List<StockItem>();
        // stockItem = new List<StockItem>();
        // stockItem.Add(new StockItem("Wheat", 0));
        // components.Add(new StockItem("Wheat", 1));
        availableJobs = 4;
        neededJob = 1;
        workNeeded = 20;
    }

}
public class IronMine : Industry
{
    public IronMine()
    {
        work = 0;
        workers = new List<Pop>();

        production.Add(new StockItem("Iron", 1));
        availableJobs = 6;
        neededJob = 1;
        workNeeded = 10;

    }

}
public class GoldMine : Industry
{
    public GoldMine()
    {
        work = 0;
        workers = new List<Pop>();
        production.Add(new StockItem("Gold", 1));
        availableJobs = 6;
        neededJob = 1;
        workNeeded = 50;

    }

}
public class CopperMine : Industry
{
    public CopperMine()
    {
        work = 0;
        workers = new List<Pop>();
        production.Add(new StockItem("Copper", 1));
        availableJobs = 6;
        neededJob = 1;
        workNeeded = 20;

    }

}
public class Quarry : Industry
{
    public Quarry()
    {
        work = 0;
        workers = new List<Pop>();
        production.Add(new StockItem("Stone", 1));
        availableJobs = 5;
        neededJob = 1;
        workNeeded = 15;

    }
}
public class Lumberjack : Industry
{
    public Lumberjack()
    {
        work = 0;
        workers = new List<Pop>();
        production.Add(new StockItem("Wood", 1));
        availableJobs = 4;
        neededJob = 1;
        workNeeded = 10;

    }
}
public class Blacksmith : Industry
{
    public Blacksmith()
    {
        work = 0;
        workers = new List<Pop>();
        production.Add(new StockItem("Sword", 1));
        components = new List<StockItem>();
        stockItem = new List<StockItem>();
        stockItem.Add(new StockItem("Iron", 0));
        components.Add(new StockItem("Iron", 1));
        availableJobs = 4;
        neededJob = 2;
        workNeeded = 25;
    }
}
public class Woodworkshop : Industry
{
    public Woodworkshop()
    {
        work = 0;
        workers = new List<Pop>();
        production.Add(new StockItem("Bow", 1));
        production.Add(new StockItem("Arrow", 20));
        components = new List<StockItem>();
        stockItem = new List<StockItem>();
        stockItem.Add(new StockItem("Wood", 0));
        components.Add(new StockItem("Wood", 1));
        availableJobs = 4;
        neededJob = 2;
        workNeeded = 20;
    }
}

class IndustrySortPerJob : IComparer<Industry>
{
    public int Compare(Industry x, Industry y)
    {
        return y.availableJobs.CompareTo(x.availableJobs);

    }
}