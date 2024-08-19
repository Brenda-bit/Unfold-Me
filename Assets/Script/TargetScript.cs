using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public string animName; // O nome do trigger da animação
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
           int layer = other.gameObject.layer;
           string layerName = LayerMask.LayerToName(layer);

            if (layerName == "TurnOffKinematic")
             {
               Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
               if (rb != null)
               {
                   rb.isKinematic = false;
               }
            }
            else
            {
                other.GetComponent<Animator>().SetTrigger(animName);
            }
        }
    }
}
