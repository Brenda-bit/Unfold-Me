using UnityEngine;
using UnityEngine.UIElements;

public class Buoyancy : MonoBehaviour
{
    public float buoyancyForce = 10f;
    public float reducedGravityScale = 0.5f;
    public float maxUpwardSpeed = 2f; // Limite para a velocidade vertical
    public GameObject deadPoint;

    private void Update()
    {
        var player = GetComponentInParent<PlayerController>();
        if (player != null && GameManager.CurrentForm != GameManager.PaperForm.BoatForm)
        {
            Rigidbody rb = player.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Min(rb.velocity.y, maxUpwardSpeed), rb.velocity.z); // Limita a velocidade vertical
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoatForm"))
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false; // Desativa a gravidade padrão
            }
        }
        else
        {
            var offset = Camera.main.transform.position - other.transform.position;
            other.transform.position = deadPoint.transform.position;
            Camera.main.transform.position = other.transform.position + offset; // Ajuste o offset conforme necessário
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BoatForm"))
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                // Limita a velocidade vertical para evitar saltos excessivos
                if (rb.velocity.y < maxUpwardSpeed)
                {
                    Vector3 buoyancy = -Physics.gravity * reducedGravityScale; // Simula a flutuação
                    rb.AddForce(buoyancy, ForceMode.Acceleration);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BoatForm"))
        {
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true; // Restaura a gravidade padrão
            }
        }
    }
}
