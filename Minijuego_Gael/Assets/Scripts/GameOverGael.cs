using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOverGael : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public GameObject playerSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetInt("Lives") > 0)
        {
            SetWinAnimation();
            resultText.text = "GANASTE!";
        }
        else
        {
            SetLoseAnimation();
            resultText.text = "PARA LA PROXIMA!";
        }
        
    }

    public void SetWinAnimation()
    {
        playerSprite.GetComponent<Animator>().SetTrigger("isWinning");
    }

    public void SetLoseAnimation()
    {
        playerSprite.GetComponent<Animator>().SetTrigger("isDying");
    }

    public void StartToPlay()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
