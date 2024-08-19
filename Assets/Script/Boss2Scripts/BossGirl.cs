using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossGirl : MonoBehaviour
{
    public Transform[] pontosDeMovimento; // Array de pontos de movimento
    public GameObject projetilPrefab; // Prefab do projétil
    public float velocidadeInicial = 2f; // Velocidade inicial do boss
    public float incrementoVelocidadeProjetil = 1f; // Aumento de velocidade do projétil ao passar por um ponto
    public float tempoEntreDisparos = 0.5f; // Tempo entre cada disparo
    public int numeroDeDisparos = 4; // Número de disparos por ponto

    private int indicePontoAtual = 0;
    private float velocidadeProjetilAtual;
    private List<int> pontosRestantes;
    public GameObject WaveObject;
    public bool isMoving = true;
    private int damageBoss = 0;
    public int damageCountPlayer = 0;
    void Start()
    {
        velocidadeProjetilAtual = incrementoVelocidadeProjetil; // Inicializa a velocidade dos projéteis
        pontosRestantes = new List<int> { 0, 1, 2, 3, 4 };
    }

    void Update()
    {
        if (isMoving)
        {
            // Move o boss em direção ao ponto atual com a velocidade constante
            transform.position = Vector3.MoveTowards(transform.position, pontosDeMovimento[indicePontoAtual].position, velocidadeInicial * Time.deltaTime);

            // Verifica se chegou ao ponto
            if (Vector3.Distance(transform.position, pontosDeMovimento[indicePontoAtual].position) < 0.1f)
            {
                AtingiuPonto();
            }
        }
        if(damageBoss == 3)
        {
            Debug.Log("You killed me");
        }
        if (damageCountPlayer >= 2)
        {
            SceneManager.LoadScene("Boss2");
        }
    }

    void AtingiuPonto()
    {
        // Dispara projéteis
        StartCoroutine(DispararProjetil());

        // Aumenta a velocidade do projétil
        velocidadeProjetilAtual += incrementoVelocidadeProjetil;

        // Remove o ponto da lista e sorteia o próximo
        pontosRestantes.Remove(indicePontoAtual);

        if (pontosRestantes.Count > 0)
        {
            indicePontoAtual = pontosRestantes[Random.Range(0, pontosRestantes.Count)];
        }
        else
        {
            WaveObject.SetActive(true);
            // Reinicia a lista de pontos
            pontosRestantes = new List<int> { 0, 1, 2, 3, 4 };
            indicePontoAtual = pontosRestantes[Random.Range(0, pontosRestantes.Count)];
            isMoving = false;
        }
    }

    IEnumerator DispararProjetil()
    {
        for (int i = 0; i < numeroDeDisparos; i++)
        {
            // Instancia o projétil
            GameObject projetil = Instantiate(projetilPrefab, transform.position, transform.rotation);
            // Define a velocidade do projétil
            projetil.GetComponent<Bullet>().velocidade = velocidadeProjetilAtual;
            yield return new WaitForSeconds(tempoEntreDisparos);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player") && GameManager.CurrentForm == GameManager.PaperForm.BoatForm)
        {
            damageBoss++;
        }
    }
}
