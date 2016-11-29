using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TimeDistribution : MonoBehaviour
{
    public GameObject foodSlider;

    public GameObject bridgesSlider;

    public GameObject houseSlider;

    private float remainer; 

    private float foodTime = 60;

    private float bridgesTime = 60;

    private float housesTime = 60;

    private float restHouseTime = 60;

    private float MAX_VALUE = 180;

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

        if ((bridgesTime == 0 || housesTime == 0) && (change < 0))
        {
            change = change * 2;
        }

        foodTime = newValue;
        bridgesTime = UpdateTimeAllocation(bridgesTime, change);
        housesTime = UpdateTimeAllocation(housesTime, change);

        UpdateSliders();
    }

    public void OnBridgesSliderChange(float newValue)
    {
        float change = bridgesTime - newValue;

        if ((foodTime == 0 || housesTime == 0) && (change < 0))
        {
            change = change * 2;
        }

        foodTime = UpdateTimeAllocation(foodTime, change);
        bridgesTime = newValue;
        housesTime = UpdateTimeAllocation(housesTime, change);

        UpdateSliders();
    }

    public void OnHousesSliderChange(float newValue)
    {
        float change = housesTime - newValue;

        if ((foodTime == 0 || bridgesTime == 0) && (change < 0))
        {
            change = change * 2;
        }

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
            remainer = 0 - timeTobeUpdated;
        }

        if (timeTobeUpdated > MAX_VALUE)
        {
            timeTobeUpdated = MAX_VALUE;
            remainer = 0 + change;
        }

        return timeTobeUpdated;
    }

    private void UpdateSliders()
    {
        foodSlider.GetComponent<Slider>().value = foodTime;
        bridgesSlider.GetComponent<Slider>().value = bridgesTime;
        houseSlider.GetComponent<Slider>().value = housesTime;
    }
}
