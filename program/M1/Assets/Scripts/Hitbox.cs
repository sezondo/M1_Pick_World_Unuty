using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage = 20;
    public float knockbackForce = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("히트박스 뭔가에 닿음: " + other.name);
        if (other.CompareTag("Player"))
        {
            PlayerHp player = other.GetComponent<PlayerHp>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("플레이어에게 공격 데미지 줌!");
            }
        }

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
            knockbackDir.y = 0.3f; // 살짝 위로 튕기는 느낌 (선택사항)
            rb.linearVelocity = Vector2.zero; // 기존 속도 제거
            rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        }
    }
}
