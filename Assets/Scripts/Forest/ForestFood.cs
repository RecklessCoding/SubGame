using UnityEngine;
using System.Collections;

public class ForestFood : MonoBehaviour
{
    public GameObject foodObjectTemplate;

    public GameObject agentsManager;

    public float restockTime = 500;

    public int maxFood;

    public int currentFoodLevel = 0;

    private ArrayList foodAvailable;

    private TimeDistribution td;

    public int CurrentFoodLevel
    {
        get { return currentFoodLevel; }
    }

    void Start()
    {
        UpdateFoodList();
        int i = foodAvailable.Count;

        while (i < maxFood)
        {
            SpawnFood();
            i++;
        }

        if (agentsManager)
        {
            td = agentsManager.GetComponent("TimeDistribution") as TimeDistribution;

            restockTime = td.TimeInDay * 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RefreshStock();
    }

    private void UpdateFoodList()
    {
        foodAvailable = new ArrayList();

        foreach (GameObject food in GameObject.FindGameObjectsWithTag("Food"))
        {
            if (isAppleOnThisForest(food.transform.position))
            {
                foodAvailable.Add(food);
            }
        }
    }

    private void RefreshStock()
    {
        if (Time.time >= restockTime)
        {
            restockTime = (Time.time) + td.TimeInDay;

            UpdateFoodList();
            int i = foodAvailable.Count;

            while (i < maxFood)
            {
                if (Random.RandomRange(0, 10) <= 4)
                    SpawnFood();

                i++;
            }
        }
    }

    private bool isAppleOnThisForest(Vector3 pos, float radius = 1f)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, radius);

        foreach (Collider collider in colliders)
        {
            GameObject go = collider.gameObject;

            if (go.CompareTag(gameObject.tag))
            {
                return true;
            }
        }

        return false;
    }

    private void SpawnFood()
    {
        Vector3 pos = GetRandomPos();

        GameObject food = Instantiate(foodObjectTemplate, pos, Quaternion.Euler(90, 0, 0)) as GameObject;
        food.transform.SetParent(gameObject.transform.parent.GetChild(2).transform);

        currentFoodLevel++;

    }

    private Vector3 GetRandomPos()
    {
        float minX = gameObject.transform.position.x - (gameObject.transform.localScale.x / 2);
        float maxX = gameObject.transform.position.x + (gameObject.transform.localScale.x / 2);

        float minZ = gameObject.transform.position.z - (gameObject.transform.localScale.z / 2);
        float maxZ = gameObject.transform.position.z + (gameObject.transform.localScale.z / 2);

        Vector3 newVec = new Vector3(Random.Range(minX, maxX),
                                     0,
                                     Random.Range(minZ, maxZ));

        return newVec;
    }
}