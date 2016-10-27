using UnityEngine;
using System.Collections;

public class AgentResourcesManager : MonoBehaviour {

    public int food = 0; //TODO: Make it private
    private int foodIncreaser = 1;
    private int foodDecreaser = 1;

    public int stamina = 10; //TODO: Make it private
    private bool isAlive = true;
    private int nextStaminaUpdate = 1;

    private int rocks = 0;
    private int rocksIncreaser = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextStaminaUpdate)
        {
            nextStaminaUpdate = Mathf.FloorToInt(Time.time) + 1;             // Change the next update (current second+1)
            Tired();
        }

        if (!isAlive)
        {
            KillItself();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Food")
        {
            GatherFood();
        }
        else if (other.gameObject.tag == "Rock")
        {
            GatherRock();
        }
    }
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

    private void GatherFood()
    {
        food = food + foodIncreaser;
    }

    private void GatherRock()
    {
        rocks = rocks + 1;
    }

    public void Eat()
    {
        food -= foodDecreaser;
        stamina += 100;
    }


    public void KillItself()
    {
        Destroy(gameObject, 10f);
    }

    private void Tired()
    {
        stamina--;

        if (stamina == 0)
        {
            isAlive = false;
        }
    }
  
}
