using UnityEngine;
using System.Collections;

public class ForestFood : MonoBehaviour
{

    public GameObject foodObjectTemplate;

    private int restockTime = 60;

    public const double MAX_FOOD = 10;

    private double currentFoodLevel = 0;

    private ArrayList foodAvailable;

    public double CurrentFoodLevel
    {
        get { return currentFoodLevel; }
    }

    void Start()
    {
        UpdateFoodList();
        int i = foodAvailable.Count;

        while (i < MAX_FOOD)
        {
            SpawnFood();
            i++;
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
            if (isAppleOnThisForest(food.transform.position)){
                foodAvailable.Add(food);
            }
        }
    }

    private void RefreshStock()
    {
        if (Time.time >= restockTime)
        {
            restockTime = Mathf.FloorToInt(Time.time) + 1;

            UpdateFoodList();
            int i = foodAvailable.Count;

            while (i < MAX_FOOD)
            {
                SpawnFood();
                i++;
            }
        }
    }

    private bool isAppleOnThisForest(Vector3 pos, float radius =  1f)
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