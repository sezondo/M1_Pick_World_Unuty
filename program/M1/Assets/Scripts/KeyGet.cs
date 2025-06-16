using UnityEngine;

public class KeyGet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.gameClear = true;
            Debug.Log("열쇠 획득!");
            
            Destroy(gameObject);
        }
    }
}
