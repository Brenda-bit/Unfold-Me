using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WavesController : MonoBehaviour
{
    private float moveDistance = 1f; // Distância que as ondas se movem para fora e para dentro
    private float moveSpeed = 2f; // Velocidade de movimento das ondas
    private bool wavesOut = false;
    public List<Transform> waveTransforms = new List<Transform>(); // Lista de transformações das ondas
    private int index = 0; // Inicializa com -1 para evitar problemas com a primeira onda
    private int waveCount = -1;
    private int indexGirl = 0;
    public GameObject bossGirl;
    private Vector3 originalPosition;
    public List<Transform> flyTransforms = new List<Transform>(); // Lista de transformações das ondas

    void OnEnable()
    {
        StartCoroutine(WaveRoutine());
        waveCount = 0;
    }

    IEnumerator WaveRoutine()
    {
        while (waveCount < 2)
        {
            ToggleWaves();
            yield return new WaitForSeconds(3f); // Aguarda 3 segundos antes de alternar novamente
            
        }
        if (waveCount == 2)
        {
            waveCount = -1;
            bossGirl.GetComponent<BossGirl>().isMoving = true;
            gameObject.SetActive(false);
        }
    }

    void ToggleWaves()
    {
        if (!wavesOut)
        {
            MoveWavesOut();
        }
        else
        {
            MoveWavesIn();
        }

        wavesOut = !wavesOut;
    }

    void MoveWavesOut()
    {
        waveCount++;
        Debug.Log(waveCount);
        index = ChooseRandomWave();
        originalPosition = waveTransforms[index].position; // Armazena a posição original
        Vector3 targetPosition = originalPosition + Vector3.up * moveDistance;
        StartCoroutine(MoveObject(waveTransforms[index], targetPosition, moveSpeed));
    }
    void Update()
    {

        bossGirl.transform.position = Vector3.MoveTowards(bossGirl.transform.position, flyTransforms[indexGirl].transform.position, 2 * Time.deltaTime);
        if (Vector3.Distance(bossGirl.transform.position, flyTransforms[indexGirl].transform.position) < 0.01f)
        {
            indexGirl++;
            if (indexGirl >= flyTransforms.Count)
            {
                indexGirl = 0;
                bossGirl.GetComponent<BossGirl>().isMoving = true;
                //this.gameObject.SetActive(false);
            }
        }
    }
    void MoveWavesIn()
    {
        if (index >= 0) // Certifica-se de que há uma onda a ser movida de volta
        {
            StartCoroutine(MoveObject(waveTransforms[index], originalPosition, moveSpeed));
        }
    }

    IEnumerator MoveObject(Transform objTransform, Vector3 targetPosition, float moveSpeed)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = objTransform.position;

        while (elapsedTime < moveSpeed)
        {
            objTransform.position = Vector3.Lerp(startingPosition, targetPosition, (elapsedTime / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objTransform.position = targetPosition;
    }

    int ChooseRandomWave()
    {
        int randomIndex = Random.Range(0, waveTransforms.Count);
        return randomIndex;
    }
}

