using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerGael : MonoBehaviour
{
    public TextMeshProUGUI DistanceText;
    public Sprite SpendLives;
    public Image[] livesImage;
    int lives = 3;
    int distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distance = GameManager.Instance.distanceToWin;
        lives = PlayerPrefs.GetInt("Lives", lives);
        ActiveText();

    }

    public void ActiveText()
    {
        DistanceText.text = "Distancia Restante: " + distance;
    }

    public void StartDistance()
    {
        distance = PlayerPrefs.GetInt("distanceToWin");
        DistanceText.text = "Distancia Restante: " + distance;
    }

    public void UpdateDistance(int newDistance)
    {
        distance = newDistance;
        DistanceText.text = "Distancia Restante: " + distance;
    }

    public void UpdateLives()
    {
        lives = GameManager.Instance.GetCurrentLives();
        if(lives >= 0 && lives < livesImage.Length)
            livesImage[lives].sprite = SpendLives;
        GameManager.Instance.CheckGameOver();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
