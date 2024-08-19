using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class NpcController : MonoBehaviour
{
    public GameObject canvasShow;
    public GameObject canvasShowAchieviement;
    public bool newHability;
    private bool needHelp;
    public PaperForm form;
    private bool isColliding;
    public bool isUnlockedDoor;
    public bool hasReceivedItem; // Nova variável para controlar se o NPC recebeu o item

    void Start()
    {
        needHelp = true;
        hasReceivedItem = false; // Inicializa como false
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && isColliding)
        {
            if (needHelp && !GameManager.isPicking)
            {
                canvasShow.SetActive(true);
            }
            else if (GameManager.isPicking && newHability)
            {
                canvasShowAchieviement.SetActive(true);
                needHelp = false;
                GameManager.UnlockForm(form);
                hasReceivedItem = true; // Marca como item entregue
            }
            else if (GameManager.isPicking && !newHability)
            {
                canvasShowAchieviement.SetActive(true);
                needHelp = false;
                if (isUnlockedDoor)
                {
                    // Ação para destrancar a porta ou qualquer outra
                }
                hasReceivedItem = true; // Marca como item entregue
            }

            // Se o item foi entregue, faz o NPC sumir
            if (hasReceivedItem)
            {
                // Aqui você pode destruir o NPC ou desativá-lo
                Destroy(gameObject); // Destrói o NPC
                // gameObject.SetActive(false); // Alternativamente, desativa o NPC
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HumanForm"))
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HumanForm"))
        {
            isColliding = false;
        }
    }
}
