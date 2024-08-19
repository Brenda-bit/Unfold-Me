using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isColliding;
    public TextMeshPro text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isColliding)
        {
            GameManager.isPicking = true;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
   {
        if (other.gameObject.CompareTag("HumanForm"))
        {
            isColliding = true;
            text.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HumanForm"))
        {
            isColliding = false;
            text.enabled = false;
        }
    }
}
