using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{

    public int timeToWin = 15; 
    static public GameControl Instance; 
    public UIController uiController; 
    public int currentXP = 0; 
    public int xpForBronze = 300; 
    public int xpForSilver = 600; 
    public int xpForGold = 1000; 

    // Función que corre el momento que inicia el juego
    public void Awake()
    {
        StopAllCoroutines(); 
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("Ammo", 5);
        // PlayerPrefs.SetInt("TimeToWin", PlayerPrefs.GetInt("TimeToWin", timeToWin)); 
        Instance = this; 
        Instance.SetReferences(); 
        DontDestroyOnLoad(this.gameObject); 
    }

    void SetReferences()
    {
        if (uiController == null)
        {
            uiController = FindAnyObjectByType<UIController>(); 
        }
        // timeToWin = PlayerPrefs.GetInt("TimeToWin"); 
        // init(); 
    
    }

    // void init()
    // {
    //     if(uiController != null)
    //     {
    //         uiController.StartTimer(); 
    //     }
    // }

    public int GetCurrentLives()
    {
        return PlayerPrefs.GetInt("Lives"); 
    }

    public int GetCurrentAmmo()
    {
        return PlayerPrefs.GetInt("Ammo"); 
    }

    public void SpendLives()
    {
        if (GetCurrentLives() > 0)
        {
            int newLives = GetCurrentLives() - 1;
            PlayerPrefs.SetInt("Lives", newLives); // PlayerPrefs
            uiController.UpdateLives();  
        }
        else
        {
            // El jugador perdió, mostrar tablet de derrota
            uiController.ShowEndGameScreen(false, 0, "Ninguna"); 
        }
        
    }

    public void SpendAmmo()
    {
        if (GetCurrentAmmo() > 0)
        {
            int newAmmo = GetCurrentAmmo() - 1; 
            PlayerPrefs.SetInt("Ammo", newAmmo); 
            uiController.UpdateAmmoDisplay(); 
        }
        else
        {
            Debug.Log("NO TIENES BALAS RESTANTES"); 
        }
    }

    public void CheckGameOver()
    {
        if(GetCurrentLives() == 0)
        {
            // El jugador perdió, mostrar tablet de derrota
            uiController.ShowEndGameScreen(false, 0, "Ninguna");
        }
    }

    // función para agregar "Tótems" 
    public void AddXP(int amount)
    {
        currentXP += amount; 
        if(currentXP > xpForGold)
        {
            currentXP = xpForGold; 
        }

        uiController.UpdateXPBar(); // Actualizamos "Tótems" en Barra
    }

    public void AddCalculatedXP(float questionTime, bool isCorrect)
    {

        if (!isCorrect) // Si pregunta se respondío incorrectamente, dar solo 50 totems
        {
            AddXP(50); 
            return; 
        }

        // Si sacaron respuesta correcta, pueden obtener hasta 250 totems (dependiendo de tiempo de contestación)
        int baseScore = 250; 
        int penalty = 0; 

    
        if (questionTime > 20f) penalty = 100; // Si tardo más de 20 segundos, le restamos 100 tótems
        else if (questionTime > 10f) penalty = 50; // Si tardo más de 10 segundos pero MENOS de 20, quitar 50 tótems

        int finalScore = baseScore - penalty;
        AddXP(finalScore); // Agregar tótems finales correspondientes


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
