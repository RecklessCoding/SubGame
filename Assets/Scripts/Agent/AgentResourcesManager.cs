using UnityEngine;
using System.Collections;
using System;

public class AgentResourcesManager
{
    private int food = 0;

    private int stamina = 15;

    private int rocks = 0;

    private GameObject home = null;

    public AgentResourcesManager()
    {

    }

    internal int Food
    {
        get
        {
            return food;
        }
    }

    internal int Rocks
    {
        get
        {
            return rocks;
        }
    }

    internal int Stamina
    {
        get
        {
            return stamina;
        }
    }

    internal GameObject Home
    {
        get
        {
            return home;
        }
    }

    internal void SetHome(GameObject house)
    {
        if (home == null)
        {
            home = house;
        }
    }

    internal void IncreaseStamina()
    {
        stamina++;
    }

    internal void DecreaseStamina()
    {
        stamina--;
    }

    internal void IncreaseFood()
    {
        food = 1;
    }

    internal void DecreaseFood()
    {
        food = 0;
    }

    internal void IncreaseRocks()
    {
        rocks = 1;
    }

    internal void DecreaseRocks()
    {
        rocks = 0;
    }

    internal bool HasRocks()
    {
        return (rocks > 0);
    }

    internal bool HasHome()
    {
        return (home != null);
    }

    internal bool HasHomeBuilt()
    {
        if (HasHome())
        {
            return home.CompareTag("HouseBuiltAvailable") || home.CompareTag("HouseBuiltFull");
        }
        else
        {
            return false;
        }
    }

    internal bool HasHomeNotBuilt()
    {
        if (HasHome())
        {
            return home.CompareTag("HouseNotBuiltAvailable") || home.CompareTag("HouseNotBuiltFull");
        }
        else
        {
            return false;
        }
    }

    internal bool HasFood()
    {
        return (food > 0);
    }
}