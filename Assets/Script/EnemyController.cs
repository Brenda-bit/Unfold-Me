using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyScriptObject enemyScriptObject;
    private Animator animator;
    public Transform playerTransform;
    bool shouldFollowPlayer;
    private bool shouldResetPosition;
    public bool shouldPatrol; // Flag para definir se o inimigo deve patrulhar
    public List<GameManager.PaperForm> formToKill;
    public Vector3 spawnBack;
    private float enemyHeight; // Altura fixa do inimigo
    private Camera mainCamera; // Referência à Cinemachine

    // Variáveis de patrulha
    public GameObject patrolStartPoint;
    public GameObject patrolEndPoint;
    private bool movingToPatrolEnd;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = GetComponent<Camera>();
        enemyHeight = transform.position.y; // Armazena a altura inicial do inimigo
        movingToPatrolEnd = true; // Inicialmente, o inimigo se move para o ponto final da patrulha
    }

    public void setShouldFollowPlayer(bool newVar)
    {
        shouldFollowPlayer = newVar;
    }

    public void setShouldResetPosition(bool newVar)
    {
        shouldResetPosition = newVar;
    }

    void Update()
    {
        if (shouldFollowPlayer)
        {
            Vector3 direction = (playerTransform.position - transform.position);
            direction.y = 0; // Ignora a componente y para movimento horizontal
            direction.Normalize(); // Agora normalize após definir a componente y como zero

            transform.position += direction * enemyScriptObject.moveSpeed * Time.deltaTime;
        }
        else if (shouldResetPosition)
        {
            ResetPosition();
        }
        else if (shouldPatrol)
        {
            Patrol();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (formToKill.Contains(GameManager.CurrentForm))
        {
            Debug.Log("Gotcha");

            shouldFollowPlayer = false;
            shouldResetPosition = true;

            var offset = Camera.main.transform.position - playerTransform.position;
            playerTransform.position = enemyScriptObject.spawnBackPlayer;
            Camera.main.transform.position = playerTransform.position + offset; // Ajuste o offset conforme necessário
        }
    }

    void ResetPosition()
    {
        if (shouldPatrol)
        {
            transform.position = Vector3.MoveTowards(transform.position, patrolStartPoint.transform.position, enemyScriptObject.moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, patrolStartPoint.transform.position) < 0.1f)
            {
                shouldResetPosition = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, spawnBack, enemyScriptObject.moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, spawnBack) < 0.1f)
            {
                shouldResetPosition = false;
            }
        }
    }

    void Patrol()
    {
        Vector3 targetPoint = movingToPatrolEnd ? patrolEndPoint.transform.position : patrolStartPoint.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, enemyScriptObject.moveSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, enemyHeight, transform.position.z); // Mantém a altura constante

        if (Mathf.Abs(transform.position.x - targetPoint.x) < 0.1f)
        {
            movingToPatrolEnd = !movingToPatrolEnd; // Inverte a direção da patrulha
        }
    }
}
