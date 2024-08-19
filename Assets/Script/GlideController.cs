using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideController : MonoBehaviour
{
    // Start is called before the first frame update
    public float glidindSpeed;
    private Rigidbody rb;
    public PlayerController player;
    private Vector3 originalGravity;
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        originalGravity = Physics.gravity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

       Vector3 customGravity = Physics.gravity * (1 - glidindSpeed);
       rb.AddForce(-customGravity, ForceMode.Acceleration);


       Debug.Log(customGravity);
    }
}
