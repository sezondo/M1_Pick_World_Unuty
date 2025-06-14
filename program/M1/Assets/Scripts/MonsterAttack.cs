using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public float detectionRange = 1f;
    public Transform hitboxLeft;
    public Transform hitboxRight;

    private Animator animator;
    public bool isAttacking = false;
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        hitboxLeft.gameObject.SetActive(false);
        hitboxRight.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking || GameManager.instance.isGameover) return;

        int mask = LayerMask.GetMask("Player");

        Vector2 dir = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, detectionRange, mask);

        if (hit.collider != null)
        {
            animator.SetTrigger("Attack"); // 애니메이션 트리거
            isAttacking = true;
        }

        Debug.DrawRay(transform.position, dir * detectionRange, Color.red);
    }

    public void EnableHitbox()
    {
        if (spriteRenderer.flipX)
        {
            hitboxLeft.gameObject.SetActive(true);
        }
        else
        {
            hitboxRight.gameObject.SetActive(true);
        }
        

    }

    public void DisableHitbox()
    {
        hitboxRight.gameObject.SetActive(false);
        hitboxLeft.gameObject.SetActive(false);
        isAttacking = false;

    }

    
    
}
