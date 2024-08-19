using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] lines;
    private int index = 0;
    public float speed;
    public GameObject canvasShow;

    void Start()
    {
        
        dialogueText.text = string.Empty;
        StartCoroutine(WriteLine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManager.isPaused = true;
            NextLine();
        }
    }

    void NextLine()
    {
        if (index < lines.Length)
        {
            dialogueText.text = "";
            StartCoroutine(WriteLine());
        }
        else
        {
            dialogueText.text = "";
            index = 0;
            canvasShow.SetActive(false);
            GameManager.isPaused = false;
        }
    }
    IEnumerator WriteLine()
    {
        foreach (char letter in lines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speed);
        }
        index++;
    }
}
