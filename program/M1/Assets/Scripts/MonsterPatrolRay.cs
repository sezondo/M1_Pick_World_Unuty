using UnityEngine;

public class MonsterPatrolRay : MonoBehaviour
{
    public float speed = 2f;
    public float rayDistance = 1f;
    private bool isGrounded = false;
    private bool movingLeft = true;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private MonsterAttack monsterAttack;
    private MonsterHp monsterHp;
    public float groundCheckPosRe = 0.02f;

    private bool isRun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        monsterHp = GetComponent<MonsterHp>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        monsterAttack = GetComponent<MonsterAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();

        if (monsterHp.isDie)
        {
            isRun = false;
            animator.SetBool("Run", isRun);
            return;
        }


        Vector2 dir = movingLeft ? Vector2.left : Vector2.right;
        if (isGrounded && !monsterAttack.isAttacking)
        {
            rb.linearVelocity = dir * speed;
        }


        int mask = LayerMask.GetMask("Dirt","Ground");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayDistance, mask);
        if (hit.collider != null)
        {
            movingLeft = !movingLeft;
        }

        if (spriteRenderer != null)
            spriteRenderer.flipX = movingLeft;

        animator.SetBool("isGrounded", isGrounded); 
        
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f && isGrounded)
        {
            isRun = true;
        }
        else if (Mathf.Abs(rb.linearVelocity.x) < 0.1f || !isGrounded)
        {
            isRun = false;
        }

        animator.SetBool("Run", isRun);

    }

    void CheckGrounded()
    {
        // 플레이어 발밑 기준, 살짝 아래쪽(플레이어 중심보다 -0.5, Sprite 따라 조정)
        Vector2 groundCheckPos = (Vector2)transform.position + Vector2.down * 0.6f;
        float radius = groundCheckPosRe; // 플레이어 폭에 맞게 조절

        // "Ground" (혹은 "Dirt") Layer에만 반응하도록
        isGrounded = Physics2D.OverlapCircle(groundCheckPos, radius, LayerMask.GetMask("Ground", "Dirt"));
        // LayerMask는 실제 프로젝트 설정에 맞게 조정!

    }

    
}
