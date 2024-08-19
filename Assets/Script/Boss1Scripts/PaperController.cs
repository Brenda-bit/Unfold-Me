using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaperController : MonoBehaviour
{
    public List<GameObject> groundPositions;  // Array para as posições no chão
    public float floatHeight = 10f;    // Altura de flutuação do boss
    public float floatSpeed = 2f;      // Velocidade de flutuação do boss
    public float fallSpeed = 10f;      // Velocidade de queda do boss

    private Vector3 startPosition;     // Posição inicial do boss
    private Vector3 targetPosition;    // Próxima posição alvo do boss
    private bool isFalling = false;    // Se o boss está caindo
    private int countTimes = 0;
    public BossHand bossHand;
    private int damageCount;
    public bool hasCollided;
    void Start()
    {
       
    }
    void OnEnable()
    {
        // Reinicializar o estado quando o objeto for reativado
        startPosition = new Vector3(transform.position.x, floatHeight, transform.position.z);
        countTimes = 0;
        isFalling = false;
        ChooseRandomPosition();
        MoveToTarget();
    }
    void Update()
    {
        if (isFalling)
        {
            // Move o boss rapidamente para baixo em direção à posição alvo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);

            // Verifica se o boss chegou na posição alvo
            if (transform.position == targetPosition)
            {
                isFalling = false;
                // Aguardar um segundo antes de flutuar de volta
                Invoke("FloatBackUp", 1f);
            }
        }
        if (countTimes == 3) 
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, floatSpeed * Time.deltaTime);
            countTimes = 0;
            bossHand.isMovingAround = true;
            
        }
        if(countTimes == 0)
        {
            startPosition = new Vector3(transform.position.x, floatHeight, transform.position.z);
            ChooseRandomPosition();
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        // Move o boss flutuando para a posição alvo antes de cair
        StartCoroutine(FluctuateToPosition());
    }

    IEnumerator FluctuateToPosition()
    {
        while (transform.position != new Vector3(targetPosition.x, floatHeight, targetPosition.z))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, floatHeight, targetPosition.z), floatSpeed * Time.deltaTime);
            yield return null;
        }
        // Quando chegar na posição de flutuação acima do alvo, começa a queda
        isFalling = true;
    }

    void FloatBackUp()
    {
        // Move o boss de volta para a posição de flutuação
        StartCoroutine(FluctuateBackUp());
    }

    IEnumerator FluctuateBackUp()
    {
        while (transform.position != startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, floatSpeed * Time.deltaTime);
            yield return null;
        }
        // Quando chegar na posição de flutuação inicial, escolhe uma nova posição e começa o movimento novamente
        ChooseRandomPosition();
        MoveToTarget();
    }

    void ChooseRandomPosition()
    {
        // Escolhe uma posição aleatória entre as posições definidas no chão
        int randomIndex = Random.Range(0, groundPositions.Count);
        targetPosition = groundPositions[randomIndex].transform.position;
        countTimes++;
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
    void OnTriggerExit(Collider other)
    {
        // Reseta a variável quando a colisão terminar
        hasCollided = false;
    }
}
