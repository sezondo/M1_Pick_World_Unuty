using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour
{

    public float jumpForce = 450; // 점프 힘

    private int jumpCount = 0; // 누적 점프 횟수
    private bool isGrounded = false; // 바닥에 닿았는지 나타냄
    private bool isDead = false; // 사망 상태
    private bool isRun = false;
    private bool isDoubleJumping = false;
    private bool isDig = false;
    private int playerHP = 100;
    private float playerSpeed = 5;
    private bool playerDirection = false; // false일때 오른쪽 true일떄 왼쪽

    public float digCooldown = 0.5f; // 곡괭이질 쿨타임 (초)
    private float lastDigTime = -10f;


    private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
    private Animator animator; // 사용할 애니메이터 컴포넌트

    private SpriteRenderer spriteRenderer; //캐릭터 본체임 이게 그래픽

    private void Start()
    {
        // 초기화

        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        // 사용자 입력을 감지하고 점프하는 처리
        if (isDead)
        {
            return;
        }

        CheckGrounded();


        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)//점프 2단까지 가능능
        {
            jumpCount++;
            playerRigidbody.linearVelocity = Vector2.zero;

            playerRigidbody.AddForce(new Vector2(0, jumpForce));

        }
        else if (Input.GetKeyUp(KeyCode.Space) && playerRigidbody.linearVelocity.y > 0)//스페이스바로 점프 높이 조절가능
        {
            playerRigidbody.linearVelocity = playerRigidbody.linearVelocity * 0.5f;
        }

        if (jumpCount >= 2) isDoubleJumping = true;
        else isDoubleJumping = false;

        animator.SetBool("DoubleJumping", isDoubleJumping);

        animator.SetBool("Grounded", isGrounded);//이걸로 점프 했냐 안했냐 애니메이션

        float xInput = Input.GetAxisRaw("Horizontal");
        //float zInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.RightArrow) && !isDig)//우측이동동
        {
            playerDirection = false;
            playerRigidbody.linearVelocity = new Vector2(xInput * playerSpeed, playerRigidbody.linearVelocity.y);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !isDig)//좌측이동동
        {
            playerDirection = true;
            playerRigidbody.linearVelocity = new Vector2(xInput * playerSpeed, playerRigidbody.linearVelocity.y);
        }


        if (!playerDirection) //이게 애니메이션 클립을 좌우 반전 시켜줌줌
        {
            spriteRenderer.flipX = false;
        }
        else if (playerDirection)
        {
            spriteRenderer.flipX = true;
        }

        if (Mathf.Abs(playerRigidbody.linearVelocity.x) > 0.1f && isGrounded)
        {
            isRun = true;
        }
        else if (Mathf.Abs(playerRigidbody.linearVelocity.x) < 0.1f || !isGrounded)
        {
            isRun = false;
        }

        animator.SetBool("Run", isRun);

        //클라이밍 여따가 추가











        //여기부턴 땅파기
        isDig = false;

        if (Time.time - lastDigTime < digCooldown)
            return;


        Vector2 digDir = Vector2.zero;


        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.RightArrow))
                digDir = Vector2.right;

            else if (Input.GetKey(KeyCode.LeftArrow))
                digDir = Vector2.left;

            else if (Input.GetKey(KeyCode.DownArrow))
                digDir = Vector2.down;

            if (digDir != Vector2.zero)
            {
                TryDig(digDir);
                lastDigTime = Time.time; // 쿨타임 갱신
            }
        }

    }

    private void Die()
    {
        animator.SetTrigger("Die");

        playerRigidbody.linearVelocity = Vector2.zero;
        isDead = true;

        GameManager.instance.OnPlayerDead();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
                jumpCount = 0;
    }


    void TryDig(Vector2 direction)
    {
        // 2D 마인크래프트 박스형 기준: 내 위치 + 방향*0.5f 만큼 offset(블록 중앙~중앙 맞춤)
        Vector2 origin = (Vector2)transform.position + direction * 0.5f;
        float digDistance = 0.6f; // 1칸 딱 맞도록

        // "Dirt" 레이어에만 맞게 (블록 오브젝트에 Dirt 레이어 지정)
        int mask = LayerMask.GetMask("Dirt");

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, digDistance, mask);

        // 맞은 게 있으면 Damage
        if (hit.collider != null)
        {
            DirtBlock block = hit.collider.GetComponent<DirtBlock>();
            if (block != null)
            {
                block.TakeDamage(20);   // 곡괭이로 데미지
                isDig = true;
                animator.SetTrigger("Dig");
            }
        }

        // 디버깅용: 씬 뷰에서 Ray 표시
        Debug.DrawRay(origin, direction * digDistance, Color.red, 0.2f);

    }


    void CheckGrounded()
    {
        // 플레이어 발밑 기준, 살짝 아래쪽(플레이어 중심보다 -0.5, Sprite 따라 조정)
        Vector2 groundCheckPos = (Vector2)transform.position + Vector2.down * 0.6f;
        float radius = 0.23f; // 플레이어 폭에 맞게 조절

        // "Ground" (혹은 "Dirt") Layer에만 반응하도록
        isGrounded = Physics2D.OverlapCircle(groundCheckPos, radius, LayerMask.GetMask("Ground", "Dirt"));
        // LayerMask는 실제 프로젝트 설정에 맞게 조정!

    }


}