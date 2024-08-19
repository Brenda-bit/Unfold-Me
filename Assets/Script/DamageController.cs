using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public DamageScriptableObject damageScriptObject;
    public Transform playerTransform;
    private Camera mainCamera; // Referência à Cinemachine
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Reproduzir a animação de contato específica do inimigo
            //animator.Play(enemyScriptObject.contactAnimation.name);
            Debug.Log("Died");

            var offset = Camera.main.transform.position - playerTransform.position;

            playerTransform.position = damageScriptObject.spawnBack;
            Camera.main.transform.position = playerTransform.position + offset; // Ajuste o offset conforme necessário
        }
    }
}
