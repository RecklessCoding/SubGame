using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TimeDistribution : MonoBehaviour
{
    public GameObject predatorsManager;

    public GameObject foodSlider;

    public GameObject bridgesSlider;

    public GameObject housesSlider;

    public GameObject procreationSlider;

    public GameObject foodLabel;

    public GameObject bridgesLabel;

    public GameObject housesLabel;

    public GameObject procreationLabel;

    public GameObject nightObject;

    private float timeInDay = 270;

    private float foodTime = 90;

    private float bridgesTime = 0;

    private float housesTime = 90;

    private float procreationTime = 0;

    private float dayLength = 180;

    private float nightLength = 90;

    public float nextNight = 0;
    public float nextDay = 0;

    public bool isNight = false;

    public int daysPassed = 0;
    public int daysUntilNextMigrant = 0;

    private bool isPlaying = true;

    private float totalFoodTime = 0;
    private float totalBridgesTime = 0;
    private float totalHousesTime = 0;
    private float totalProcreationTime = 0;

    private float averageFoodTime = 0;
    private float averageBridgesTime = 0;
    private float averageHousesTime = 0;
    private float averageProcreationTime = 0;

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

    internal float ProcreationTime
    {
        get
        {
            return procreationTime;
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
        Application.targetFrameRate = 30;

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

                for (int i = 0; i < transform.childCount; i++)
                {
                    AgentActionsSelector agent = transform.GetChild(i).GetComponent("AgentActionsSelector") as AgentActionsSelector;
                    agent.IsNight();
                }

                predatorsManager.GetComponent<PredatorsManager>().setNight(true);
            }

            if (Time.time >= nextDay && isNight) // Time to pick a new work
            {
                nextNight = Mathf.FloorToInt(dayLength) + Mathf.FloorToInt(Time.time);
                isNight = false;

                nightObject.SetActive(false);

                for (int i = 0; i < transform.childCount; i++)
                {
                    AgentActionsSelector agent = transform.GetChild(i).GetComponent("AgentActionsSelector") as AgentActionsSelector;
                    agent.IsDay();
                }

                daysPassed = daysPassed + 1;
                daysUntilNextMigrant += 1;

                predatorsManager.GetComponent<PredatorsManager>().setNight(false);
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


    void Awake()
    {
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
            procreationTime = 0;
        }
        else
        {
            if (change <= 0)
            {
                if ((bridgesTime == 0 && housesTime == 0) ^ (bridgesTime == 0 && procreationTime == 0) ^ (procreationTime == 0 && housesTime == 0))
                {
                    foodTime = newValue;
                    bridgesTime = UpdateTimeAllocation(bridgesTime, change, 1);
                    housesTime = UpdateTimeAllocation(housesTime, change, 1);
                    procreationTime = UpdateTimeAllocation(procreationTime, change, 1);
                }
                else if ((bridgesTime == 0 ^ housesTime == 0) ^ procreationTime == 0)
                {
                    foodTime = newValue;
                    bridgesTime = UpdateTimeAllocation(bridgesTime, change, 2);
                    housesTime = UpdateTimeAllocation(housesTime, change, 2);
                    procreationTime = UpdateTimeAllocation(procreationTime, change, 2);
                }
                else
                {
                    foodTime = newValue;
                    bridgesTime = UpdateTimeAllocation(bridgesTime, change, 3);
                    housesTime = UpdateTimeAllocation(housesTime, change, 3);
                    procreationTime = UpdateTimeAllocation(procreationTime, change, 3);
                }
            }
            else
            {
                foodTime = newValue;
                bridgesTime = UpdateTimeAllocation(bridgesTime, change, 3);
                housesTime = UpdateTimeAllocation(housesTime, change, 3);
                procreationTime = UpdateTimeAllocation(procreationTime, change, 3);
            }
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
            procreationTime = 0;
        }
        else
        {
            if (change <= 0)
            {
                if ((foodTime == 0 && housesTime == 0) ^ (foodTime == 0 && procreationTime == 0) ^ (procreationTime == 0 && housesTime == 0))
                {
                    foodTime = UpdateTimeAllocation(foodTime, change, 1);
                    bridgesTime = newValue;
                    housesTime = UpdateTimeAllocation(housesTime, change, 1);
                    procreationTime = UpdateTimeAllocation(procreationTime, change, 1);
                }
                else if ((foodTime == 0 ^ housesTime == 0) ^ procreationTime == 0)
                {
                    foodTime = UpdateTimeAllocation(foodTime, change, 2);
                    bridgesTime = newValue;
                    housesTime = UpdateTimeAllocation(housesTime, change, 2);
                    procreationTime = UpdateTimeAllocation(procreationTime, change, 2);
                }
                else
                {
                    foodTime = UpdateTimeAllocation(foodTime, change, 3);
                    bridgesTime = newValue;
                    housesTime = UpdateTimeAllocation(housesTime, change, 3);
                    procreationTime = UpdateTimeAllocation(procreationTime, change, 3);
                }
            }
            else
            {
                foodTime = UpdateTimeAllocation(foodTime, change, 3);
                bridgesTime = newValue;
                housesTime = UpdateTimeAllocation(housesTime, change, 3);
                procreationTime = UpdateTimeAllocation(procreationTime, change, 3);
            }
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
            procreationTime = 0;
        }
        else
        {
            if (change <= 0)
            {
                if ((foodTime == 0 && bridgesTime == 0) ^ (foodTime == 0 && procreationTime == 0) ^ (procreationTime == 0 && bridgesTime == 0))
                {
                    foodTime = UpdateTimeAllocation(foodTime, change, 1);
                    bridgesTime = UpdateTimeAllocation(bridgesTime, change, 1);
                    housesTime = newValue;
                    procreationTime = UpdateTimeAllocation(procreationTime, change, 1);
                }
                else if ((foodTime == 0 ^ bridgesTime == 0) ^ procreationTime == 0)
                {
                    foodTime = UpdateTimeAllocation(foodTime, change, 2);
                    bridgesTime = UpdateTimeAllocation(bridgesTime, change, 2);
                    housesTime = newValue;
                    procreationTime = UpdateTimeAllocation(procreationTime, change, 2);
                }
                else
                {
                    foodTime = UpdateTimeAllocation(foodTime, change, 3);
                    bridgesTime = UpdateTimeAllocation(bridgesTime, change, 3);
                    housesTime = newValue;
                    procreationTime = UpdateTimeAllocation(procreationTime, change, 3);
                }
            }
            else
            {
                foodTime = UpdateTimeAllocation(foodTime, change, 3);
                bridgesTime = UpdateTimeAllocation(bridgesTime, change, 3);
                housesTime = newValue;
                procreationTime = UpdateTimeAllocation(procreationTime, change, 3);
            }
        }

        TimeDistributionUpdated();
    }

    public void OnProcreationSliderChange(float newValue)
    {
        float change = procreationTime - newValue;

        if (newValue >= dayLength)
        {
            foodTime = 0;
            bridgesTime = 0;
            housesTime = 0;
            procreationTime = newValue;
        }
        else
        {
            if (change <= 0)
            {
                if ((foodTime == 0 && bridgesTime == 0) ^ (foodTime == 0 && procreationTime == 0) ^ (procreationTime == 0 && bridgesTime == 0))
                {
                    foodTime = UpdateTimeAllocation(foodTime, change, 1);
                    bridgesTime = UpdateTimeAllocation(bridgesTime, change, 1);
                    housesTime = UpdateTimeAllocation(housesTime, change, 1);
                    procreationTime = newValue;
                }
                else if ((foodTime == 0 ^ bridgesTime == 0) ^ procreationTime == 0)
                {
                    foodTime = UpdateTimeAllocation(foodTime, change, 2);
                    bridgesTime = UpdateTimeAllocation(bridgesTime, change, 2);
                    housesTime = UpdateTimeAllocation(housesTime, change, 2);
                    procreationTime = newValue;
                }
                else
                {
                    foodTime = UpdateTimeAllocation(foodTime, change, 3);
                    bridgesTime = UpdateTimeAllocation(bridgesTime, change, 3);
                    housesTime = UpdateTimeAllocation(housesTime, change, 3);
                    procreationTime = newValue;
                }
            }
            else
            {
                foodTime = UpdateTimeAllocation(foodTime, change, 3);
                bridgesTime = UpdateTimeAllocation(bridgesTime, change, 3);
                housesTime = UpdateTimeAllocation(housesTime, change, 3);
                procreationTime = newValue;
            }
        }

        TimeDistributionUpdated();
    }

    private void AllocateDayTime()
    {
        dayLength = foodTime + bridgesTime + housesTime + procreationTime;
    }

    private void TimeDistributionUpdated()
    {
        UpdateSliders();
        //AlertActionSelection();
    }

    private void AlertActionSelection()
    {
        AgentsActionSelector agentsAS = gameObject.GetComponent("AgentsActionSelector") as AgentsActionSelector;
        agentsAS.ReStrategise();
    }

    private float UpdateTimeAllocation(float timeTobeUpdated, float change, float divider)
    {
        timeTobeUpdated = timeTobeUpdated + (change / divider);

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
        foodSlider.GetComponent<Slider>().value = foodTime;
        bridgesSlider.GetComponent<Slider>().value = bridgesTime;
        housesSlider.GetComponent<Slider>().value = housesTime;
        procreationSlider.GetComponent<Slider>().value = procreationTime;

        foodLabel.GetComponent<Text>().text = (foodTime / dayLength * 100).ToString("0.0") + "%";
        bridgesLabel.GetComponent<Text>().text = (bridgesTime / dayLength * 100).ToString("0.0") + "%";
        housesLabel.GetComponent<Text>().text = (housesTime / dayLength * 100).ToString("0.0") + "%";
        procreationLabel.GetComponent<Text>().text = (procreationTime / dayLength * 100).ToString("0.0") + "%";

        totalFoodTime += foodTime;
        totalBridgesTime += bridgesTime;
        totalHousesTime += housesTime;
        totalProcreationTime += procreationTime;

        numberOfChanges++;

        averageFoodTime = totalFoodTime / (numberOfChanges);
        averageBridgesTime = totalBridgesTime / (numberOfChanges);
        averageHousesTime = totalHousesTime / (numberOfChanges);
        averageProcreationTime = procreationTime / (numberOfChanges);

        foodSlider.GetComponent<Slider>().maxValue = dayLength;
        bridgesSlider.GetComponent<Slider>().maxValue = dayLength;
        housesSlider.GetComponent<Slider>().maxValue = dayLength;
        procreationSlider.GetComponent<Slider>().maxValue = dayLength;
    }
}
