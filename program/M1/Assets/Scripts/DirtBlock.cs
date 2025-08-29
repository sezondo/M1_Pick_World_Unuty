using UnityEngine;

public class DirtBlock : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    // 0: 완전 건강, 1: 금 조금, 2: 금 중간, 3: 금 심함, 4: 거의 파괴
    public Sprite[] crackedSprites; // 인스펙터에 5개 등록

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
        spriteRenderer.enabled = false;
    }

    public void TakeDamage(int amount)
    {
        if (currentHP <= 0) return;

        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0);
        UpdateVisual();

        if (currentHP <= 0)
        {
            DestroyBlock();
        }
    }

    void UpdateVisual()
    {
        if (currentHP != 100)
        {
            spriteRenderer.enabled = true;
        }

        int spriteIndex = GetSpriteIndex();
        if (spriteIndex >= 0 && spriteIndex < crackedSprites.Length)
            spriteRenderer.sprite = crackedSprites[spriteIndex];
    }

    int GetSpriteIndex()
    {
        float ratio = (float)currentHP / maxHP;

        if (ratio > 0.8f)
            return 0; // 81~100
        else if (ratio > 0.6f)
            return 1; // 61~80
        else if (ratio > 0.4f)
            return 2; // 41~60
        else if (ratio > 0.2f)
            return 3; // 21~40
        else
            return 4; // 0~20 (위험, 거의 파괴)
    }

    void DestroyBlock()
    {
        // 파괴 효과 등 추가 가능
        Destroy(gameObject);
    }
}
