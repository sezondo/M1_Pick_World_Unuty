using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StratScene : MonoBehaviour
{

    public TextMeshProUGUI uiText;
    public float fadeSpeed = 1.5f;
    private bool fadingOut = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color color = uiText.color;
        float alphaChange = fadeSpeed * Time.deltaTime;
        if (fadingOut)
        {
            color.a -= alphaChange;
            if (color.a <= 0f)
            {
                color.a = 0f;
                fadingOut = false;
            }
        }
        else
        {
            color.a += alphaChange;
            if (color.a >= 1f)
            {
                color.a = 1f;
                fadingOut = true;
            }
        }

        uiText.color = color;

        if (Input.anyKeyDown) // 화면 터치 or 마우스 클릭
        {
            SceneManager.LoadScene("Main");
        }
    }
    
}
