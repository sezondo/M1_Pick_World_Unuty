using UnityEngine;
using System.Collections;

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
    private bool Underground = false;
    private bool isClimbing = false;
    private bool isClimbingIdle = false;
    private int playerHP = 100;
    private float playerSpeed = 3f;
    private bool playerDirection = false; // false일때 오른쪽 true일떄 왼쪽

    private float digCooldown = 0.7f; // 곡괭이질 쿨타임 (초)
    private bool isDigging = false;
    private float digTimer = 0f;
    private DirtBlock currentBlock = null;
    


    private Rigidbody2D playerRigidbody; // 사용할 리지드바디 컴포넌트
    private Animator animator; // 사용할 애니메이터 컴포넌트

    private SpriteRenderer spriteRenderer; //캐릭터 본체임 이게 그래픽


    private GameObject booster1;//부스터 1
    private GameObject booster2;//부스터 2

    private void Start()
    {
        // 초기화

        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        booster1 = transform.Find("vfx_SciFiThruster01").gameObject;
        booster2 = transform.Find("vfx_SciFiThruster02").gameObject;

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
        float yInput = Input.GetAxisRaw("Vertical");
        //float zInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.RightArrow) && !isDig )//우측이동동
        {
            playerDirection = false;
            if (!isClimbing)
            {
                playerRigidbody.linearVelocity = new Vector2(xInput * playerSpeed, playerRigidbody.linearVelocity.y);

            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !isDig )//좌측이동동
        {
            playerDirection = true;
            if (!isClimbing)
            {
                playerRigidbody.linearVelocity = new Vector2(xInput * playerSpeed, playerRigidbody.linearVelocity.y);

            }
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
/*
        Vector2 ClimbingDir = Vector2.zero;
        if (Input.GetKey(KeyCode.RightArrow)) ClimbingDir = Vector2.right;
        else if (Input.GetKey(KeyCode.LeftArrow)) ClimbingDir = Vector2.left;
        else if (Input.GetKey(KeyCode.DownArrow)) ClimbingDir = Vector2.down;
        else if (Input.GetKey(KeyCode.DownArrow)) ClimbingDir = Vector2.up;
*/
        if (Underground)
        {

            if ((Input.GetKeyDown(KeyCode.UpArrow) && !isGrounded) || isClimbing)
            {

                if (!isClimbing)
                {
                    
                    isClimbing = true;
                    playerRigidbody.gravityScale = 0.5f;
                    //playerRigidbody.linearVelocity = Vector2.zero;
                    jumpCount = 1;
                }
            }

            if (isClimbing)
            {

                Vector2 move = Vector2.zero;
                if (Input.GetKey(KeyCode.RightArrow)) move.x = 1;
                if (Input.GetKey(KeyCode.LeftArrow)) move.x = -1;

                float moveSpeed = playerSpeed;
                playerRigidbody.linearVelocity = new Vector2(move.x * moveSpeed, playerRigidbody.linearVelocity.y);

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    float rocketPower = 1.5f; // 로켓 추친력
                    playerRigidbody.AddForce(Vector2.up * rocketPower);
                    animator.speed = 1;

                    booster1.gameObject.SetActive(true);
                    booster2.gameObject.SetActive(true);
                }
                else
                {
                    animator.speed = 0;
                    booster1.gameObject.SetActive(false);
                    booster2.gameObject.SetActive(false);
                }

            }
            
            if (isGrounded || Input.GetKeyDown(KeyCode.Space))
            {
                StopClimbing();
            }
            
        }
        else StopClimbing();
        
        animator.SetBool("isClimbing", isClimbing);









        //여기부턴 땅파기
        //isDig = false;

        Vector2 digDir = Vector2.zero;
        if (Input.GetKey(KeyCode.RightArrow)) digDir = Vector2.right;
        else if (Input.GetKey(KeyCode.LeftArrow)) digDir = Vector2.left;
        else if (Input.GetKey(KeyCode.DownArrow)) digDir = Vector2.down;

        if (digDir != Vector2.zero && !isClimbing)//내일 여기 이즈 클라이밍 추가
        {
            // 벽 감지
            Vector2 origin = (Vector2)transform.position + digDir * 0.5f;
            float digDistance = 0.7f;
            int mask = LayerMask.GetMask("Dirt");
            RaycastHit2D hit = Physics2D.Raycast(origin, digDir, digDistance, mask);//레이시스트 삐융

            if (hit.collider != null && isGrounded)
            {
                DirtBlock block = hit.collider.GetComponent<DirtBlock>();
                if (block != null)
                {
                    if (!isDigging || block != currentBlock) // 첫 블럭럭
                    {
                        isDigging = true;
                        digTimer = 0f; // 즉시 데미지 X (애니만 실행)
                        currentBlock = block; //이걸로 다른블럭인지 체크
                        animator.SetBool("Dig", true);
                    }

                    digTimer += Time.deltaTime;
                    if (digTimer >= digCooldown)
                    {
                        block.TakeDamage(20);
                        digTimer = 0f;
                        // 사운드/이펙트도 여기!
                    }
                }
                else
                {
                    StopDig();
                }
            }
            else
            {
                StopDig();
            }
        }
        else
        {
            StopDig();

        }

    }

    void StopDig()
    {
        if (isDigging)
        {
            isDigging = false;
            digTimer = 0f;
            currentBlock = null;
            animator.SetBool("Dig", false);
        }

    }

    void StopClimbing()
    {
        if (isClimbing)
        {
            animator.speed = 1;
            isClimbing = false;
            playerRigidbody.gravityScale = 1f;
            animator.SetBool("isClimbing", isClimbing);
            booster1.gameObject.SetActive(false);
            booster2.gameObject.SetActive(false);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Underground"))
        {
            Underground = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Underground"))
        {
            Underground = false;

        }
    }



    void CheckGrounded()
    {
        // 플레이어 발밑 기준, 살짝 아래쪽(플레이어 중심보다 -0.5, Sprite 따라 조정)
        Vector2 groundCheckPos = (Vector2)transform.position + Vector2.down * 0.6f;
        float radius = 0.02f; // 플레이어 폭에 맞게 조절

        // "Ground" (혹은 "Dirt") Layer에만 반응하도록
        isGrounded = Physics2D.OverlapCircle(groundCheckPos, radius, LayerMask.GetMask("Ground", "Dirt"));
        // LayerMask는 실제 프로젝트 설정에 맞게 조정!

    }


}