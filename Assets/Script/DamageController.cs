using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public DamageScriptableObject damageScriptObject;
    public Transform playerTransform;
    private Camera mainCamera; // Refer�ncia � Cinemachine
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Reproduzir a anima��o de contato espec�fica do inimigo
            //animator.Play(enemyScriptObject.contactAnimation.name);
            Debug.Log("Died");

            var offset = Camera.main.transform.position - playerTransform.position;

            playerTransform.position = damageScriptObject.spawnBack;
            Camera.main.transform.position = playerTransform.position + offset; // Ajuste o offset conforme necess�rio
        }
    }
}
