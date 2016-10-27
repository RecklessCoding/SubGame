using UnityEngine;
using System.Collections;

public class ForestFood : MonoBehaviour
{

    public GameObject foodObjectTemplate;

    private int restockTime = 1;

    public const double MAX_FOOD = 10;

    private double currentFoodLevel = 0;

    private GameObject[] foodAvailable;

    public double CurrentFoodLevel
    {
        get { return currentFoodLevel; }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateFoodList();
        RefreshStock();
    }

    private void UpdateFoodList()
    {
        foodAvailable = GameObject.FindGameObjectsWithTag("Food");
    }

    private void RefreshStock()
    {
        if (Time.time >= restockTime)
        {
            restockTime =  Mathf.FloorToInt(Time.time) + 1;
            int i = foodAvailable.Length;
            while (i < MAX_FOOD)
            {
                SpawnFood();
                i++;
            }
        }
    }

    private void SpawnFood()
    {
        Vector3 pos = GetRandomPos();

        GameObject food = Instantiate(foodObjectTemplate, pos, Quaternion.Euler(90, 0, 0)) as GameObject;
    }

    private Vector3 GetRandomPos()
    {
        float minX = gameObject.transform.position.x - (gameObject.transform.localScale.x / 2) + 0.75f;
        float maxX = gameObject.transform.position.x + (gameObject.transform.localScale.x / 2) - 0.75f;

        float minZ = gameObject.transform.position.z - (gameObject.transform.localScale.y / 2) + 0.75f;
        float maxZ = gameObject.transform.position.z + (gameObject.transform.localScale.y / 2) - 0.75f;

        Vector3 newVec = new Vector3(Random.Range(minX, maxX),
                                     0,
                                     Random.Range(minZ, maxZ));

        return newVec;
    }
}