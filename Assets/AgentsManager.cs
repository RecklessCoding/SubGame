using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void ChangeSpeed(int factor)
    {
        Agent[] agents = GetAllAgents();

        foreach (Agent agent in agents)
        {
            
        }
    }

    private Agent[] GetAllAgents()
    {
        return gameObject.GetComponentsInChildren<Agent>(); ;
    }
}
