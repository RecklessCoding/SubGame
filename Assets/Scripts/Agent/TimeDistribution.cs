using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TimeDistribution : MonoBehaviour
{
    private float foodTime = 60;

    private float bridgesTime = 60;

    private float housesTime = 60;

    private float restHouseTime = 60;

    public float FoodTime
    {
        get
        {
            return foodTime;
        }
    }

    public float BridgesTime
    {
        get
        {
            return bridgesTime;
        }
    }

    public float HousesTime
    {
        get
        {
            return housesTime;
        }
    }
    public float RestHousesTime
    {
        get
        {
            return restHouseTime;
        }
    }


    // Use this for initialization
    void Start()
    {
        UpdateSliders();
    }

    public void OnFoodSliderChange(float newValue)
    {
        float change = foodTime - newValue;

        foodTime = newValue;
        bridgesTime = UpdateTimeAllocation(bridgesTime, change);
        housesTime = UpdateTimeAllocation(housesTime, change);

        UpdateSliders();
    }

    public void OnBridgesSliderChange(float newValue)
    {
        float change = bridgesTime - newValue;

        foodTime = UpdateTimeAllocation(foodTime, change);
        bridgesTime = newValue;
        housesTime = UpdateTimeAllocation(housesTime, change);

        UpdateSliders();
    }

    public void OnHousesSliderChange(float newValue)
    {
        float change = housesTime - newValue;

        foodTime = UpdateTimeAllocation(foodTime, change);
        bridgesTime = UpdateTimeAllocation(bridgesTime, change);
        housesTime = newValue;

        TimeDistributionUpdated();
    }

    private void TimeDistributionUpdated()
    {
        UpdateSliders();
        AlertActionSelection();
    }

    private void AlertActionSelection()
    {
        AgentsActionSelector agentsAS = gameObject.GetComponent("AgentsActionSelector") as AgentsActionSelector;
        agentsAS.ReStrategise();
    }

    private float UpdateTimeAllocation(float timeTobeUpdated, float change)
    {
        timeTobeUpdated = timeTobeUpdated + change / 2;

        if (timeTobeUpdated < 0)
        {
            timeTobeUpdated = 0;
        }

        return timeTobeUpdated;
    }

    private void UpdateSliders()
    {
        GameObject.Find("SliderFood").GetComponent<Slider>().value = foodTime;
        GameObject.Find("SliderBridges").GetComponent<Slider>().value = bridgesTime;
        GameObject.Find("SliderHouses").GetComponent<Slider>().value = housesTime;
    }
}
