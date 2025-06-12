using System.Collections.Generic;
using UnityEngine;

public class PlayerBoomSpawn : MonoBehaviour
{
    public GameObject boomPrefab; 
    PlayerController playerController;
    private float checkRadius = 0.01f;
    Vector2 spawnPos;
    public int addCurrentBoom = 0;
    private List<GameObject> boom = new List<GameObject>();
    public int maxBoomCount = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController.playerDirection) //이거 스폰 방향 정하는거
        {
            spawnPos = transform.position + Vector3.right * 0.7f;
        }
        else
        {
            spawnPos = transform.position + Vector3.left * 0.7f;
        }

        Collider2D hit = Physics2D.OverlapCircle(spawnPos, checkRadius, LayerMask.GetMask("Dirt"));

        if (hit == null && boom.Count < maxBoomCount)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                boom.Add(Instantiate(boomPrefab, spawnPos, Quaternion.identity));
            }
        }

        for (int i =  boom.Count - 1; i >= 0; i--)
        {

            if (boom[i] == null)
            {
                boom.RemoveAt(i);
            }
            
        }


    }
}
