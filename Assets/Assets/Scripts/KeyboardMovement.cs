using UnityEngine;
using System.Collections;

public class KeyboardMovement : MonoBehaviour {

    Rigidbody rbody;

    Animator anim;

	// Use this for initialization
	void Start () {

        rbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
        Vector3 movementVector = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));

        if (movementVector != Vector3.zero)
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("input_x", movementVector.x);
            anim.SetFloat("input_y", movementVector.z);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        rbody.MovePosition(rbody.position + movementVector * Time.deltaTime);
	}
}
