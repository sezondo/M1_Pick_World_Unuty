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
    private int playerHP = 100;
    private float playerSpeed = 5;
    private bool playerDirection = false; // false일때 오른쪽 true일떄 왼쪽
    

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

        if (Input.GetKey(KeyCode.RightArrow))//우측이동동
        {
            playerDirection = false;
            playerRigidbody.linearVelocity = new Vector2(xInput * playerSpeed, playerRigidbody.linearVelocity.y);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))//좌측이동동
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
        // 바닥에 닿았음을 감지하는 처리
        if (collision.contacts[0].normal.y > 0.7f) // -0.7하면 반대의 경우도 가능
        {
            isGrounded = true;
            //lastGroundedY = transform.position.y;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }


}