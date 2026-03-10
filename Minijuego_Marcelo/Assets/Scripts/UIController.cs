using System.Collections;
using System.Collections.Generic; // Necesario para filtrar preguntas pendientes
using TMPro; // Necesario para editar los textos
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject questionPanel; // Game Object del panel que despliega preguntas
    public Sprite spendLives; 
    public Sprite spendAmmo; 
    public Image[] livesImage; // Imágenes de vidas
    public Image[] ammoImages; // Imágenes de munición
    int lives = 3; 
    int ammo = 5; 

    // Variables para el XP (Tótems) component
    public TextMeshProUGUI xpText; 

    // Vínculos de componentes "TextMeshProUGUI" Unity a Backend con funcionalidad
    public TextMeshProUGUI questionUIText; 
    public TextMeshProUGUI leftButtonUIText; 
    public TextMeshProUGUI rightButtonUIText;

    // Datos locales para creación y gestión de preguntas
    public QuestionData[] questionsList; 
    private int currentQuestionIndex = -1; 
    private float questionStartTime; 

    // Variables para panel "final de juego"
    public GameObject endGamePanel; 
    public TextMeshProUGUI endTitleText; 
    public TextMeshProUGUI endXpText; 
    public TextMeshProUGUI endMedalText; 


    // int time; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // time = GameControl.Instance.timeToWin; 
        lives = PlayerPrefs.GetInt("Lives"); 
        // ActiveText(); 
        
    }

    // public void StartTimer()
    // {
    //     StartCoroutine(MatchTime());
    // }

    public void UpdateLives()
    {
        lives = GameControl.Instance.GetCurrentLives(); 
        if (lives >= 0 && lives < livesImage.Length)
        {
            livesImage[lives].sprite = spendLives; 
        }
        GameControl.Instance.CheckGameOver(); 
    }

    public void UpdateAmmoDisplay()
    {
        ammo = GameControl.Instance.GetCurrentAmmo(); 
        if (ammo >= 0 && ammo < ammoImages.Length)
        {
            ammoImages[ammo].sprite = spendAmmo; 
        }
    }


    // Función se encarga de mostrar el question panel y la pregunta pertinente ( se usa al interactuar con un cofre )
    public void OpenQuestionPanel()
    {
        // Creamos lista temporal para preguntas que siguen "pendientes"
        List<int> pendingQuestions = new List<int>(); 

       // Para cada pregunta en nuestra lista de preguntas
       for (int i = 0; i<questionsList.Length; i++)
        {
            // Para cada pregunta que no haya sido respondida correctamente
            if (questionsList[i].isAnsweredCorrectly == false) 
            {
                // Agregar pregunta a lista de "preguntas pendientes"
                pendingQuestions.Add(i); 
            }
        }

        if (pendingQuestions.Count > 0)
        {
            // Seleccionar pregunta aleatoria dentro de lista de preguntas posibles a preguntar
            int randomIndex = Random.Range(0, pendingQuestions.Count); 
            currentQuestionIndex = pendingQuestions[randomIndex]; 

            QuestionData selectedQuestion = questionsList[currentQuestionIndex]; 

            questionUIText.text = selectedQuestion.questionText; 
            leftButtonUIText.text = selectedQuestion.leftAnswerText; 
            rightButtonUIText.text = selectedQuestion.rightAnswertext; 

            questionPanel.SetActive(true);  // Activamos el panel de preguntas si tenemos preguntas pendientes
            questionStartTime = Time.realtimeSinceStartup; // Registra el segundo exacto en el cual se abrío el questions panel
        }
        else
        {
            // Caso especial para ganar, significa no matar a los jefes y responder TODAS las preguntas para este area. 
            Debug.Log("Felicidades! Ya respondiste todas las preguntas de Awaq Correctamente. "); 
            Time.timeScale = 1f; 
            ShowEndGameScreen(true, 1000, "Platino (Pacifista)"); 
        }
    }

    public void SelectLeftBtn()
    {
        VerifyAnswer(1); 
    }

    public void SelectRightBtn()
    {
        VerifyAnswer(2); 
    }

    private void VerifyAnswer(int selectedOption)
    {
        float timeTaken = Time.realtimeSinceStartup - questionStartTime; // Diferencia de tiempo entre que se respondío la pregunta y que se abrío el panel. 
        bool correct; 

        // Si respuesta es correcta, marcar pregunta como "isAnsweredCorrectly" y poner correct = true
        if (questionsList[currentQuestionIndex].correctButtonIndex == selectedOption)
        {
            Debug.Log("RESPUESTA CORRECTA"); 
            questionsList[currentQuestionIndex].isAnsweredCorrectly = true; 
            correct = true; 
        }
        // Si respuesta es incorrecta, poner correct = false ( esto sirve para los tótems )
        else
        {
            Debug.Log("RESPUESTA INCORRECTA");
            correct = false; 
        }

        GameControl.Instance.AddCalculatedXP(timeTaken, correct); 

        CloseQuestionPanelAndResume(); 
    }

    // IEnumerator MatchTime()
    // {
    //     yield return new WaitForSeconds(1); 
    //     time -=1; 
    //     ActiveText();
    //     if (time == 0)
    //     {
    //         SceneManager.LoadScene("EndScene"); 
    //     }
    //     else
    //     {
    //      StartCoroutine(MatchTime());   
    //     } 
    // }

    // public void ActiveText()
    // {
    //     timeText.text = "Remaining time: " + time; 
    // }

    public void CloseQuestionPanel() // Función se encarga de apagar el panel ( se usara al momento en el cuál el usuario conteste )
    {
        questionPanel.SetActive(false); 
    }

    public void SelectCorrectAnswer()
    {
        Debug.Log("Correct Answer Registered"); 
        CloseQuestionPanelAndResume(); 
    }

    private void CloseQuestionPanelAndResume()
    {
        if(questionPanel != null)
        {
            questionPanel.SetActive(false); 
        }

        Time.timeScale = 1f; 
    }

    public void ShowEndGameScreen(bool isWin, int xpGained, string medal)
    {
        // Pausamos juego 
        Time.timeScale = 0f; 

        // Definimos si gano o perdió
        if (isWin)
        {
            endTitleText.text = "¡GANASTE!"; 
        }
        else
        {
            endTitleText.text = "¡FIN DEL JUEGO!"; 
        }

        // Adignamos valores de XP y Medalla
        endXpText.text = "XP Total: " + xpGained; 
        endMedalText.text = "Medalla: " + medal; 

        // Activamos tableta de "endGamePanel"
        endGamePanel.SetActive(true); 
    }

    // Función para "Replay"
    public void ClickReplay()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    // Función para "Exit"
    public void ClickExit()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }

    public void UpdateXPBar()
    {
        xpText.text = GameControl.Instance.currentXP + " / " + GameControl.Instance.xpForGold; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
