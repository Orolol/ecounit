using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    public int basePrice;
    public string name;

    public int foodValue;
    private static Dictionary<string, int> itemValues = new Dictionary<string, int>
    {
        ["Iron"] = 10,
        ["Stone"] = 8,
        ["Gold"] = 40,
        ["Copper"] = 13,
        ["Wood"] = 8,
        ["Clay"] = 15,
        ["Grape"] = 6,
        ["Wheat"] = 2,
        ["Arrow"] = 1,
        ["Cow"] = 8,
        ["Fish"] = 3,
        ["Sword"] = 20,
        ["Bow"] = 13
    };
    private static Dictionary<string, int> foodValues = new Dictionary<string, int>
    {
        ["Iron"] = 0,
        ["Stone"] = 0,
        ["Gold"] = 0,
        ["Copper"] = 0,
        ["Wood"] = 0,
        ["Clay"] = 0,
        ["Grape"] = 1,
        ["Wheat"] = 2,
        ["Arrow"] = 0,
        ["Cow"] = 5,
        ["Fish"] = 2,
        ["Sword"] = 0,
        ["Bow"] = 0
    };

    // { "Iron", "Stone", "Copper", "Gold", "Wood", "Clay", "Grape", "Bow", "Swords", "Wheat", "Cow", "Fish" };

    public Item(string n)
    {
        if (itemValues.Keys.Contains(n))
        {
            name = n;
            basePrice = itemValues[n];
            foodValue = foodValues[n];
        }
        else
        {
            // Debug.LogError("Item not found :" + n);
        }
    }
}

public class ItemContainer
{
    protected Item item;
    public int quantity;
    public string getName()
    {
        return item.name;
    }
    public int getFoodValue()
    {
        return item.foodValue;
    }
    public int getBasePrice()
    {
        return item.basePrice;
    }

}

public class StockItem : ItemContainer
{
    public float cost = 0;
    public StockItem(Item i, int quant)
    {
        item = i;
        quantity = quant;
    }
    public StockItem(string i, int quant)
    {
        item = new Item(i);
        quantity = quant;
    }

    public int takeStock(int q, bool strict = false)
    {
        if (quantity >= q)
        {
            quantity -= q;
            return q;
        }
        else
        {
            if (strict)
            {
                return 0;
            }
            else
            {
                int r = quantity;
                quantity = 0;
                return r;
            }
        }
    }
    public void addStock(int q)
    {
        quantity += q;
    }

}

public class TradeItem : ItemContainer
{

    public float currentBuyPrice;
    public float currentSellPrice;


    public TradeItem(Item i, int quant)
    {
        item = i;
        quantity = quant;
        currentBuyPrice = i.basePrice * 1.05f;
        currentSellPrice = i.basePrice * 0.95f;

    }
    public TradeItem(string i, int quant)
    {
        item = new Item(i);
        quantity = quant;
        currentBuyPrice = item.basePrice * 1.05f;
        currentSellPrice = item.basePrice * 0.95f;
    }

    public int takeStock(int q)
    {
        if (quantity >= q)
        {
            float ratio = (q / (quantity + 5.0f));
            float priceChange = (100.0f + (5.0f * ratio)) / 100;
            // Debug.Log("take " + q + "/" + quantity + " " + getName() + " pricechange :" + priceChange + " ratio " + ratio);
            quantity -= q;
            currentBuyPrice *= priceChange;
            currentSellPrice *= priceChange;
            return q;
        }
        else
        {
            int r = quantity;
            float ratio = (q / (r + 1.0f));
            float priceChange = (100.0f + (10.0f * ratio)) / 100;
            quantity = 0;
            currentBuyPrice *= priceChange;
            currentSellPrice *= priceChange;
            return r;
        }
    }
    public void addStock(int q)
    {
        float ratio = (q / (quantity + 5.0f));
        float priceChange = (100.0f - (5.0f * ratio)) / 100;
        // Debug.Log("ADD " + q + "/" + quantity + " " + getName() + " pricechange :" + priceChange + " ratio " + ratio);

        quantity += q;
        currentBuyPrice *= priceChange;
        currentSellPrice *= priceChange;
    }




}
