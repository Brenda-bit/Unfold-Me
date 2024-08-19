using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour
{
    public enum Forms
    {
        Paper = 0,
        Scissors = 1,
        Rock = 2,
    }

    public GameObject[] points;
    public float speed;
    private int index = 0;
    public bool isMovingAround = true;
    private int timesTransformed = 0;
    public List<GameObject> forms;
    public int totalDamagePlayer;

    void Start()
    {
        transform.position = points[index].transform.position;
    }

    void Update()
    {
        if (isMovingAround)
        {
            SetFormActive(3);
            MoveAround();
        }
    }

    public void MoveAround()
    {
        if (points.Length == 0) return;

        transform.position = Vector3.MoveTowards(transform.position, points[index].transform.position, speed * Time.deltaTime);
        if (transform.position == points[index].transform.position)
        {
            index++;
            if (index >= points.Length)
            {
                index = 0;
                isMovingAround = false;
                StartJoken();
            }
        }
    }

    public void StartJoken()
    {
        int result = UnityEngine.Random.Range(0, 2); // 0 or 1

        if(timesTransformed < 2) 
        {
            if (result == 0)
            {
                SetFormActive(0);
                timesTransformed++;
            }
            else
            {
                SetFormActive(1);
                timesTransformed++;
            }
        }
        else 
        {
            SetFormActive(2);
            timesTransformed = 0;
        }
    }

    private void SetFormActive(int activeIndex)
    {
        for (int i = 0; i < forms.Count; i++)
        {
            forms[i].SetActive(i == activeIndex);
        }
    }

    public void SetDamage()
    {
        if (totalDamagePlayer > 2)
        {
            //playdeadCutscene
            Debug.Log("KILLED ME AA");
        }
        else {
            totalDamagePlayer++;
        }
    }
}
