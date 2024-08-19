using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 5f;
    private float jumpForce = 10f;
    private Rigidbody rb;

    public GameObject[] formas = new GameObject[5]; // Array de 5 formas
    private int formaAtualIndex = 0; // Índice da forma atual
    private Vector3 lastPosition; // Última posição válida do jogador
    public ParticleSystem changeFormParticles; // Referência ao ParticleSystem
    public bool isGrounded;

    private bool isStretching;
    private Vector3 originalScale;
    private Vector3 targetScale;

    // Parâmetros de escala
    private float maxHeight = 1.2f;
    private float stretchSpeed = 2f;
    private GameObject childPopUp;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;

        // Garantir que a primeira forma esteja ativada no início
        TrocarForma(0);

        // Armazenar a escala inicial do jogador
        originalScale = transform.localScale;
        Debug.Log("Original Scale set to: " + originalScale);
    }

    void Update()
    {
        if (!GameManager.isPaused)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (GameManager.CurrentForm == GameManager.PaperForm.CircleForm)
            {
                // Movimentação específica para a forma de bola (rolamento)
                Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * speed * Time.deltaTime;
                transform.Translate(movement);

                // Adicionar rotação à esfera
                formas[formaAtualIndex].transform.Rotate(0, horizontalInput * 80f * Time.deltaTime, 0);
            }
            else if (GameManager.CurrentForm == GameManager.PaperForm.PopUpForm)
            {
                // Verifica se o player ainda está espichando
                if (isStretching)
                {
                    // Cresce gradualmente até a altura máxima
                    formas[formaAtualIndex].transform.localScale = Vector3.Lerp(formas[formaAtualIndex].transform.localScale, targetScale, stretchSpeed * Time.deltaTime);

                    if (Vector3.Distance(formas[formaAtualIndex].transform.localScale, targetScale) < 0.01f)
                    {
                        formas[formaAtualIndex].transform.localScale = targetScale; // Garante que a escala final seja exata
                        isStretching = false; // Para o crescimento
                        Debug.Log("Stretching completed. Final scale: " + formas[formaAtualIndex].transform.localScale);
                    }
                }
            }
            else
            {
                // Movimentação padrão para outras formas
                Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * speed * Time.deltaTime;
                transform.Translate(movement);
            }

            isGrounded = Physics.Raycast(formas[formaAtualIndex].transform.position, Vector3.down, 2f);
            Debug.DrawRay(formas[formaAtualIndex].transform.position, Vector3.down * 10f, Color.red);

            // Pular
            if (isGrounded && Input.GetButtonDown("Jump") && GameManager.CurrentForm != GameManager.PaperForm.AirPlaneForm)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        // Trocar entre formas de 1 a 5 (índices 0 a 4)
        for (int i = 1; i <= 5; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                TrocarForma(i - 1);
            }
        }

        // Atualizar a última posição após o movimento
        lastPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (GameManager.CurrentForm == GameManager.PaperForm.BoatForm && collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            GameManager.isPaused = true;
        }
        else
        {
            GameManager.isPaused = false;
        }
    }

    void TrocarForma(int novaFormaIndex)
    {
        // Verificar se o índice é válido
        if (novaFormaIndex < 0 || novaFormaIndex >= formas.Length)
        {
            Debug.LogError("Índice de forma inválido.");
            return;
        }

        // Verificar se a forma está desbloqueada
        if (!GameManager.IsFormUnlocked((GameManager.PaperForm)novaFormaIndex))
        {
            Debug.LogError("Forma não está desbloqueada.");
            return;
        }

        // Guardar a posição atual antes de trocar de forma
        lastPosition = transform.position;

        // Desativar todas as formas
        for (int i = 0; i < formas.Length; i++)
        {
            formas[i].SetActive(false);
        }

        // Ativar a nova forma
        formas[novaFormaIndex].SetActive(true);

        // Atualizar o índice da forma atual
        formaAtualIndex = novaFormaIndex;

        // Atualizar a forma atual no GameManager
        GameManager.CurrentForm = (GameManager.PaperForm)novaFormaIndex;

        // Se a forma anterior foi PopUpForm, use o ponto de spawn para posicionar a nova forma
        if (GameManager.PreviousForm == GameManager.PaperForm.PopUpForm)
        {

            transform.position = childPopUp.transform.position;
        }
        else
        {
            // Restaurar a última posição válida do jogador
            transform.position = lastPosition;
        }

        // Ativa as partículas ao trocar de forma
        changeFormParticles.Play();

        // Resetar a escala e começar a espichar se a forma for PopUpForm
        if (GameManager.CurrentForm == GameManager.PaperForm.PopUpForm)
        {
            originalScale = formas[formaAtualIndex].transform.localScale;  // Certifique-se de armazenar a escala correta ao trocar para esta forma
            targetScale = new Vector3(maxHeight, originalScale.y, originalScale.z);
            isStretching = true;
            childPopUp = formas[formaAtualIndex].transform.GetChild(0).gameObject;
            Debug.Log("Started stretching. Original Scale: " + originalScale + " Target Scale: " + targetScale);
        }
        else
        {
            // Somente redefinir a escala se não for a forma PopUpForm e se não estiver no meio do espichamento
            if (isStretching)
            {
                isStretching = false;
                formas[formaAtualIndex].transform.localScale = originalScale;
                Debug.Log("Reset to original scale: " + originalScale);
            }
        }

        // Atualizar a forma anterior no GameManager
        GameManager.PreviousForm = (GameManager.PaperForm)novaFormaIndex;
    }
}
