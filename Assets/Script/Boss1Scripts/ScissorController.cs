using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScissorController : MonoBehaviour
{
    public GameObject[] points;
    public GameObject startObject;
    public BossHand bossHand;
    public float speed;
    public int index;
    private bool atStartAttack;
    private Vector3 startPosition;     // Posição inicial do boss
    public int damageCount;
    private Vector3 spawnPosition;
    public float cutSpeed = 1.5f;
    public bool hasCollided;

    private void OnEnable()
    {
        // Initialize atStartPosition based on the initial position of the object
        spawnPosition = transform.position;
        atStartAttack = transform.position == startObject.transform.position;
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    void Start()
    {
        // Initialize atStartPosition based on the initial position of the object
        atStartAttack = transform.position == startObject.transform.position;
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (!atStartAttack)
        {
            // Move to the start object first
            transform.position = Vector3.MoveTowards(transform.position, startObject.transform.position, speed * Time.deltaTime);
            if (transform.position == startObject.transform.position)
            {
                atStartAttack = true;
            }
        }
        else
        {
            // Move around the points
            if (index < points.Length)
            {
                transform.position = Vector3.MoveTowards(transform.position, points[index].transform.position, cutSpeed * Time.deltaTime);
                if (transform.position == points[index].transform.position)
                {
                    index++;
                }
                if (index == points.Length)
                {
                    StartCoroutine(MoveToSpawnPosition(spawnPosition));
                }
            }
        }
        Debug.Log(transform.position);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            if (collision.gameObject.CompareTag("Player") && damageCount < 2)
            {
                damageCount++;
                Debug.Log("bateu");
            }
            else if (collision.gameObject.CompareTag("Player") && damageCount == 2)
            {
                SceneManager.LoadScene("Boss1");
            }
            
            hasCollided = true;
        }
    }
    private IEnumerator MoveToSpawnPosition(Vector3 spawnPosition)
    {
        while (transform.position != spawnPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, spawnPosition, speed * Time.deltaTime);
            yield return null; // Espera um frame antes de continuar o loop
        }

        index = 0;
        atStartAttack = false;
        bossHand.isMovingAround = true;
    }

    void OnTriggerExit(Collider other)
    {
        // Reseta a variável quando a colisão terminar
        hasCollided = false;
    }
}
