using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;
    public float velocidade = 5f;
    public BossGirl bossGirl;
    // Start is called before the first frame update
    private void Awake()
    {
        Destroy(gameObject, lifeTime);

    }
    public void Start()
    {
        GameObject girl = GameObject.Find("Girl");
        bossGirl = girl.GetComponent<BossGirl>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            bossGirl.damageCountPlayer++;
            Destroy(gameObject);
        }
    }
    void Update()
    {
        transform.Translate(Vector3.back * velocidade * Time.deltaTime);

        if (transform.position.y < -10f) // Ajuste esse valor conforme necessário
        {
            Destroy(gameObject);
        }
    }
}
