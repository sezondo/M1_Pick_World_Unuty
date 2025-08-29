using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public bool isDead = false;
    public float hp = 100f;
    public Image imgBar;


    public AudioClip playerHurtSound;
    public AudioSource playerHurtSoundSource;
    public AudioClip playerDieSound;
    public AudioSource playerDieSoundSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            GameManager.instance.OnPlayerDead();
        }
        SetHp();
    }

    public void SetHp(){
        imgBar.transform.localScale = new Vector3(hp/100.0f,1,1);
    }

    public void TakeDamage(int damage)
    {
        if (GameManager.instance.isGameover)
        {
            return;
        }
        Debug.Log("플레이어 피해 입음");

        hp -= damage;

        if (hp > 0)
        {
            playerHurtSoundSource.PlayOneShot(playerHurtSound);
            GetComponent<Animator>().SetTrigger("isHurt");
        }

        if (hp <= 0)
        {
            hp = 0;
            //GameObject.Find("GameManager").GetComponent<Spawn>().count--;
        }

        if (hp <= 0f)
        {
            GameManager.instance.isGameover = true;
            isDead = true;
            playerDieSoundSource.PlayOneShot(playerDieSound);
            GetComponent<Animator>().SetTrigger("isDie");
            //Destroy(gameObject, 1);
        }
        if (hp >= 0f)
        {
//            imgBar.transform.localScale = new Vector3(hp / 100.0f, 1, 1);
        }

    }
    
}
