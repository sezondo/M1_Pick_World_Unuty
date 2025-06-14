using UnityEngine;

public class MonsterHp : MonoBehaviour
{

    public int maxHP = 3;
    private int currentHP;
    public bool isDie = false;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            isDie = true;
            Die();
        }
        else animator.SetTrigger("hit"); 
    }

    void Die()
    {
        animator.SetTrigger("die");
        Destroy(gameObject,1.5f); // 나중엔 이펙트 넣어도 좋음
    }
    
}
