using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitWalkingScript : MonoBehaviour
{
    EnemyController enemyController;

    private void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
        transform.parent = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameManager.CurrentForm.ToString()) && enemyController.formToKill.Contains(GameManager.CurrentForm))
        {
            Debug.Log("Following player in form: " + GameManager.CurrentForm.ToString());
            enemyController.setShouldFollowPlayer(true);
            enemyController.setShouldResetPosition(false);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(GameManager.CurrentForm.ToString()) && !enemyController.formToKill.Contains(GameManager.CurrentForm))
        {
            enemyController.setShouldFollowPlayer(false);
            enemyController.setShouldResetPosition(true);
        }
        else if (other.gameObject.CompareTag(GameManager.CurrentForm.ToString()) && enemyController.formToKill.Contains(GameManager.CurrentForm))
        {
            enemyController.setShouldFollowPlayer(true);
            enemyController.setShouldResetPosition(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       enemyController.setShouldFollowPlayer(false);
       enemyController.setShouldResetPosition(true);
    }
}
