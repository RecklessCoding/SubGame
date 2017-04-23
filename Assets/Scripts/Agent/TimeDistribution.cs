using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TimeDistribution : MonoBehaviour
{
    public GameObject foodSlider;

    public GameObject bridgesSlider;

    public GameObject housesSlider;

    public GameObject foodLabel;

    public GameObject bridgesLabel;

    public GameObject housesLabel;

    public GameObject nightObject;

    public float timeInDay = 120;

    public float foodTime = 30;

    public float bridgesTime = 30;

    public float housesTime = 30;

    public float dayLength = 90;

    private float nightLength = 30;

    public float nextNight = 0;
    public float nextDay = 0;

    public bool isNight = false;

    private const float FOOD_TIME = 30;
    private const float BRIDGE_TIME = 30;
    private const float HOUSES_TIME = 30;
    private const float DAY_LENGTH = 90;
    private const float NIGHT_LENGTH = 30;
    private const float TIME_IN_DAY = 120;

    public int daysPassed = 0;
    public int daysUntilNextMigrant = 0;

    private bool isPlaying = true;

    private float totalFoodTime = 0;
    private float totalBridgesTime = 0;
    private float totalHousesTime = 0;
    private float averageFoodTime = 0;
    private float averageBridgesTime = 0;
    private float averageHousesTime = 0;

    private int numberOfChanges = -1;

    private bool isImmigrationOn = true;

    internal float AverageFoodTime
    {
        get
        {
            return averageFoodTime;
        }
    }
    internal float AverageBridgesTime
    {
        get
        {
            return averageBridgesTime;
        }
    }
    internal float AverageHouseTime
    {
        get
        {
            return averageHousesTime;
        }
    }

    internal float FoodTime
    {
        get
        {
            return foodTime;
        }
    }

    internal float BridgesTime
    {
        get
        {
            return bridgesTime;
        }
    }

    internal float HousesTime
    {
        get
        {
            return housesTime;
        }
    }

    internal float RestHousesTime
    {
        get
        {
            return nightLength;
        }
    }


    internal float TimeInDay
    {
        get
        {
            return timeInDay;
        }
    }

    internal float NightLength
    {
        get
        {
            return nightLength;
        }
    }


    internal int DaysPassed
    {
        get
        {
            return daysPassed;
        }
    }

    // Use this for initialization
    void Start()
    {
        AllocateDayTime();

        TimeDistributionUpdated();

        nextNight = Mathf.FloorToInt(dayLength);

        numberOfChanges = 0;

        if (PlayerPrefs.GetInt("Immigration") == 1)
        {
            isImmigrationOn = true;
        }
        else
        {
            isImmigrationOn = false;
        }
    }

    void Update()
    {
        if (isPlaying)
        {
            if (Time.time >= nextNight && !isNight) // Time to pick a new work
            {
                nextDay = Mathf.FloorToInt(nightLength) + Mathf.FloorToInt(Time.time);
                isNight = true;

                nightObject.SetActive(true);

                AgentsActionSelector agentsAS = gameObject.GetComponent("AgentsActionSelector") as AgentsActionSelector;
                agentsAS.IsNight();
            }

            if (Time.time >= nextDay && isNight) // Time to pick a new work
            {
                nextNight = Mathf.FloorToInt(dayLength) + Mathf.FloorToInt(Time.time);
                isNight = false;

                nightObject.SetActive(false);

                AgentsActionSelector agentsAS = gameObject.GetComponent("AgentsActionSelector") as AgentsActionSelector;
                agentsAS.IsDay();

                daysPassed = daysPassed + 1;
                daysUntilNextMigrant += 1;
            }

            if (daysUntilNextMigrant >= 10)
            {
                if (isImmigrationOn)
                {
                    (gameObject.GetComponent("AgentsCreator") as AgentsCreator).MigratePopulation(50);
                }

                daysUntilNextMigrant = 0;
            }
        }
    }

    internal void EndGame()
    {
        isPlaying = false;

        Time.timeScale = 0;
    }

    internal void OnFoodSliderChange(float newValue)
    {
        float change = foodTime - newValue;

        if (newValue >= dayLength)
        {
            foodTime = newValue;
            bridgesTime = 0;
            housesTime = 0;
        }
        else
        {
            foodTime = newValue;
            bridgesTime = UpdateTimeAllocation(bridgesTime, change);
            housesTime = UpdateTimeAllocation(housesTime, change);
        }

        TimeDistributionUpdated();
    }

    internal void OnBridgesSliderChange(float newValue)
    {
        float change = bridgesTime - newValue;

        if (newValue >= dayLength)
        {
            foodTime = 0;
            bridgesTime = newValue;
            housesTime = 0;
        }
        else
        {
            foodTime = UpdateTimeAllocation(foodTime, change);
            bridgesTime = newValue;
            housesTime = UpdateTimeAllocation(housesTime, change);
        }

        TimeDistributionUpdated();
    }

    internal void OnHousesSliderChange(float newValue)
    {
        float change = housesTime - newValue;

        if (newValue >= dayLength)
        {
            foodTime = 0;
            bridgesTime = 0;
            housesTime = newValue;
        }
        else
        {
            foodTime = UpdateTimeAllocation(foodTime, change);
            bridgesTime = UpdateTimeAllocation(bridgesTime, change);
            housesTime = newValue;
        }

        TimeDistributionUpdated();
    }

    private void AllocateDayTime()
    {
        dayLength = foodTime + bridgesTime + housesTime;
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
        timeTobeUpdated = timeTobeUpdated + (change / 2);

        if (timeTobeUpdated < 0)
        {
            timeTobeUpdated = 0;
        }

        if (timeTobeUpdated >= dayLength)
        {
            timeTobeUpdated = dayLength;
        }

        return timeTobeUpdated;
    }

    private void UpdateSliders()
    {
        foodSlider.GetComponent<Slider>().maxValue = dayLength;
        bridgesSlider.GetComponent<Slider>().maxValue = dayLength;
        housesSlider.GetComponent<Slider>().maxValue = dayLength;

        foodSlider.GetComponent<Slider>().value = foodTime;
        bridgesSlider.GetComponent<Slider>().value = bridgesTime;
        housesSlider.GetComponent<Slider>().value = housesTime;

        foodLabel.GetComponent<Text>().text = (foodTime / dayLength * 100).ToString("0.0") + "%";
        bridgesLabel.GetComponent<Text>().text = (bridgesTime / dayLength * 100).ToString("0.0") + "%";
        housesLabel.GetComponent<Text>().text = (housesTime / dayLength * 100).ToString("0.0") + "%";

        totalFoodTime += foodTime;
        totalBridgesTime += bridgesTime;
        totalHousesTime += housesTime;

        numberOfChanges++;

        averageFoodTime = totalFoodTime / (numberOfChanges);
        averageBridgesTime = totalBridgesTime / (numberOfChanges);
        averageHousesTime = totalHousesTime / (numberOfChanges);
    }

    internal void ChangeDayNightCycle(float factor)
    {
        if (factor != 1)
        {
            float foodTimeRatio = foodTime / dayLength;
            float bridgesTimeRatio = bridgesTime / dayLength;
            float housesTimeRatio = housesTime / dayLength;

            dayLength = DAY_LENGTH / factor;
            nightLength = NIGHT_LENGTH / factor;

            timeInDay = TIME_IN_DAY / factor;

            foodTime = foodTimeRatio * dayLength;
            bridgesTime = bridgesTimeRatio * dayLength;
            housesTime = housesTimeRatio * dayLength;

            nextNight = ((nextNight - Mathf.FloorToInt(Time.time)) / factor) + Mathf.FloorToInt(Time.time);
            nextDay = ((nextDay - Mathf.FloorToInt(Time.time)) / factor) + Mathf.FloorToInt(Time.time);

        }
        else
        {
            ResetTimes();
        }

        UpdateSliders();

        AgentsActionSelector agentsAS = gameObject.GetComponent("AgentsActionSelector") as AgentsActionSelector;
        agentsAS.ChangeSpeed();
    }

    private void ResetTimes()
    {
        foodTime = FOOD_TIME;
        bridgesTime = BRIDGE_TIME;
        housesTime = HOUSES_TIME;
        dayLength = DAY_LENGTH;
        nightLength = NIGHT_LENGTH;
        timeInDay = TIME_IN_DAY;
    }
}
