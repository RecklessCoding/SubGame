using UnityEngine;
using System.Collections;

public class AgentResourcesManager {

    private int food = 0;
    private int foodIncreaser = 1;
    private int foodDecreaser = 1;

    private int stamina = 10;

    private int rocks = 0;
    private int rocksIncreaser = 1;

    public int Food
    {
        get
        {
            return food;
        }
    }

    public int Rocks
    {
        get
        {
            return rocks;
        }
    }

    public int Stamina
    {
        get
        {
            return stamina;
        }
    }

    public void DecreaseStamina()
    {
        stamina--;
    }

    public void IncreaseFood()
    {
        Debug.Log(food);
        food = food + foodIncreaser;
    }

    public void DecreaseFood()
    {
        food =- foodDecreaser;
    }

    public void IncreaseRocks()
    {
        rocks = +rocksIncreaser;
    }
}
