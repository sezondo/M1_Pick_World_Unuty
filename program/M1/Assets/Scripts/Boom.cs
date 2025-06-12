using UnityEngine;

public class Boom : MonoBehaviour
{
    public float boomTime = 5f;
    public float boomTimerShining = 1.2f;

    public float firstDamageRadius = 0.5f;
    public int firstDamageAmount = 100;
    public float secondDamageRadius = 1.5f;
    public int secondDamageAmount = 50;
    SpriteRenderer sr;
    Color originColor;

    //사운드 이펙트

    public AudioClip boomSound;
    public GameObject boomPrefab;

    private float n = 0;
    private float time = 0;
    bool colorChange = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originColor = sr.color;

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        n += Time.deltaTime;


        if (boomTimerShining < time)
        {
            if (colorChange)
            {
                sr.color = Color.red;
                colorChange = false;
            }
            else
            {
                sr.color = originColor;
                colorChange = true;
            }
            time = 0;
            boomTimerShining *= 0.7f;
        }

        if (boomTime < n)
        {
            Debug.LogFormat("폭탄 터짐");
            Explode();
        }
    }


    void Explode()
    {
        // 범위 데미지 - Physics.OverlapSphere 사용 (2D면 OverlapCircleAll)
        Collider2D[] firstTargets = Physics2D.OverlapCircleAll(transform.position, firstDamageRadius);
        foreach (var target in firstTargets)
        {

            target.GetComponent<PlayerHp>()?.TakeDamage(firstDamageAmount);
            target.GetComponent<DirtBlock>()?.TakeDamage(firstDamageAmount);

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 폭탄 중심에서 해당 오브젝트까지의 방향 벡터 계산
                Vector2 direction = (target.transform.position - transform.position).normalized;
                float force = 400f; // 힘의 세기(적당히 조절)

                rb.AddForce(direction * force);
            }
        }

        Collider2D[] secondTargets = Physics2D.OverlapCircleAll(transform.position, secondDamageRadius);
        foreach (var target in secondTargets)
        {

            target.GetComponent<PlayerHp>()?.TakeDamage(secondDamageAmount);
            target.GetComponent<DirtBlock>()?.TakeDamage(secondDamageAmount);

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 폭탄 중심에서 해당 오브젝트까지의 방향 벡터 계산
                Vector2 direction = (target.transform.position - transform.position).normalized;
                float force = 200f; // 힘의 세기(적당히 조절)

                rb.AddForce(direction * force);
            }
        }

        BoomSound();

        // 폭발 이펙트 & 삭제
        Destroy(gameObject);
    }
    
    public void BoomSound(){
        GameObject soundObj = new GameObject("BoomSound");
        AudioSource audio = soundObj.AddComponent<AudioSource>();
        audio.clip = boomSound;
        audio.Play();
        Destroy(soundObj, boomSound.length);
        GameObject playerDieeffect =  Instantiate(boomPrefab, transform.position, transform.rotation);
        Destroy(playerDieeffect, 0.3f);
    }
}
