using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaperController : MonoBehaviour
{
    public List<GameObject> groundPositions;  // Array para as posi��es no ch�o
    public float floatHeight = 10f;    // Altura de flutua��o do boss
    public float floatSpeed = 2f;      // Velocidade de flutua��o do boss
    public float fallSpeed = 10f;      // Velocidade de queda do boss

    private Vector3 startPosition;     // Posi��o inicial do boss
    private Vector3 targetPosition;    // Pr�xima posi��o alvo do boss
    private bool isFalling = false;    // Se o boss est� caindo
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
            // Move o boss rapidamente para baixo em dire��o � posi��o alvo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);

            // Verifica se o boss chegou na posi��o alvo
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
        // Move o boss flutuando para a posi��o alvo antes de cair
        StartCoroutine(FluctuateToPosition());
    }

    IEnumerator FluctuateToPosition()
    {
        while (transform.position != new Vector3(targetPosition.x, floatHeight, targetPosition.z))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, floatHeight, targetPosition.z), floatSpeed * Time.deltaTime);
            yield return null;
        }
        // Quando chegar na posi��o de flutua��o acima do alvo, come�a a queda
        isFalling = true;
    }

    void FloatBackUp()
    {
        // Move o boss de volta para a posi��o de flutua��o
        StartCoroutine(FluctuateBackUp());
    }

    IEnumerator FluctuateBackUp()
    {
        while (transform.position != startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, floatSpeed * Time.deltaTime);
            yield return null;
        }
        // Quando chegar na posi��o de flutua��o inicial, escolhe uma nova posi��o e come�a o movimento novamente
        ChooseRandomPosition();
        MoveToTarget();
    }

    void ChooseRandomPosition()
    {
        // Escolhe uma posi��o aleat�ria entre as posi��es definidas no ch�o
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
        // Reseta a vari�vel quando a colis�o terminar
        hasCollided = false;
    }
}
