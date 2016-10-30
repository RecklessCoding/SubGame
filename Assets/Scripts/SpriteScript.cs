using UnityEngine;
using System.Collections.Generic;
using System;

public class SpriteScript : MonoBehaviour
{

    public Transform target;

    public float zOffset;

    // Use this for initialization
    void Start()
    {
        transform.GetComponent<NavMeshAgent>().updateRotation = false;
        target.GetComponentInParent<NavMeshAgent>().updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {

        transform.localPosition = new Vector3(target.localPosition.x, transform.localPosition.y, target.localPosition.z + zOffset);
    }
}
