using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindScript : MonoBehaviour
{
    public float windForce = 30f;
    public float reducedWind = 2f;
    public Vector3 windDirection = -Vector3.right;
    private LineRenderer lineRenderer;  // LineRenderer para desenhar a linha do vento
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        //// Configurações do LineRenderer
        //lineRenderer.startWidth = 0.1f;
        //lineRenderer.endWidth = 0.1f;
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //lineRenderer.startColor = Color.blue;
        //lineRenderer.endColor = Color.blue;
    }
    private void Update()
    {
        //// Atualiza a posição da linha do vento
        //Vector3 startPoint = transform.position;
        //Vector3 endPoint = startPoint + windDirection.normalized * windForce;
        //lineRenderer.SetPosition(0, startPoint);
        //lineRenderer.SetPosition(1, endPoint);
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();
        if (!other.gameObject.tag.Equals("CircleForm"))
        {
            if (rb != null)
            {
                rb.AddForce(windDirection.normalized * windForce);
            }
        }
        else if (other.gameObject.tag.Equals("CircleForm"))
        {
            if (rb != null)
            {
                rb.AddForce(windDirection.normalized * reducedWind);
            }
        }
    }
}
