using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public bool isDead = false;
    public float hp = 100f;
    public RawImage imgBar;
    public Canvas inmBarAll;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetHp(int value){
        hp = value;
        imgBar.transform.localScale = new Vector3(hp/100.0f,1,1);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("플레이어 피해 입음");

        hp -= damage;

        if (hp > 0)
        {
            GetComponent<Animator>().SetTrigger("isHurt");
        }

        if (hp <= 0)
        {
            hp = 0;
            //GameObject.Find("GameManager").GetComponent<Spawn>().count--;
        }

        if (hp <= 0f)
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("isDie");
            //Destroy(gameObject, 1);
        }
        if (hp >= 0f)
        {
//            imgBar.transform.localScale = new Vector3(hp / 100.0f, 1, 1);
        }

    }
    
}
