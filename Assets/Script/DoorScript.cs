using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public string doorID; // Um identificador único para cada porta

    private bool isOpen = false;

    private void Start()
    {
        // Checa se a porta já foi aberta anteriormente
        if (PlayerPrefs.GetInt(doorID, 0) == 1)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        // Implementa a lógica para abrir a porta, como tocar a animação de abertura
        isOpen = true;
        PlayerPrefs.SetInt(doorID, 1); // Salva o estado da porta como aberta
        PlayerPrefs.Save(); // Garante que os dados sejam salvos permanentemente
    }

    public void CloseDoor()
    {
        // Implementa a lógica para fechar a porta, se necessário
        isOpen = false;
        PlayerPrefs.SetInt(doorID, 0); // Salva o estado da porta como fechada
        PlayerPrefs.Save(); // Garante que os dados sejam salvos permanentemente
    }
}
