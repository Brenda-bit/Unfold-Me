using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RockController : MonoBehaviour
{
    public GameObject[] points;
    public float speed;
    private int index;
    public bool isMoving;
    private int lastIndex;
    public float moveDuration = 0.3f;
    public BossHand bossHand;
    public bool hasCollided;

    private Coroutine moveCoroutine; // Store the reference to the coroutine

    void Start()
    {
        isMoving = true;
        lastIndex = -1;
        ChooseRandomIndex();
        StartMoving();
    }

    private void OnEnable()
    {
        isMoving = true;
        lastIndex = -1;
        ChooseRandomIndex();
        StartMoving();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveAround();
        }
        else
        {
            bossHand.isMovingAround = true;
        }
    }

    public void MoveAround()
    {
        if (points.Length > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[index].transform.position, speed * Time.deltaTime);
            if (transform.position == points[index].transform.position)
            {
                lastIndex = index;
                ChooseRandomIndex();
            }
        }
    }

    private void ChooseRandomIndex()
    {
        if (points.Length > 1)
        {
            while (true)
            {
                index = Random.Range(0, points.Length);
                if (index != lastIndex)
                {
                    break;
                }
            }
        }
        else if (points.Length == 1)
        {
            index = 0;
        }
    }

    private void StartMoving()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveForDuration());
    }

    private IEnumerator MoveForDuration()
    {
        yield return new WaitForSeconds(moveDuration);
        isMoving = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            if (collision.gameObject.CompareTag("Player") && GameManager.CurrentForm == GameManager.PaperForm.AirPlaneForm)
            {
                Debug.Log("ai");
                bossHand.SetDamage();
                bossHand.isMovingAround = true;
            }
            hasCollided = true;
        }
    }
}
