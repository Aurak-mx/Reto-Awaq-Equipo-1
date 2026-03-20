using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;


public class GameManager : MonoBehaviour
{
    
    public int distanceToWin = 200;
    static public GameManager Instance;
    public UIControllerGael UIController;
    public SFXManagerGael sFXManager;


    private void Awake()
    {
        StopAllCoroutines();
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("distanceToWin", distanceToWin);
        Instance = this;
        Instance.SetReferences();
        DontDestroyOnLoad(this.gameObject);

    }

    void SetReferences()
    {
        if (UIController == null)
        {
            UIController = FindFirstObjectByType<UIControllerGael>();
        }
        if (sFXManager == null)
        {
            sFXManager = FindFirstObjectByType<SFXManagerGael>();
        }
        distanceToWin = PlayerPrefs.GetInt("distanceToWin");
        init();

        
    }


    void init()
    {
        if(UIController != null)
        {
            UIController.StartDistance();
        }
    }

    
    public int GetCurrentLives()
    {
        return PlayerPrefs.GetInt("Lives", 3);
    }

    public void SpendLives()
    {
        if (GetCurrentLives() > 0)
        {
            int newLives = GetCurrentLives()-1;
            PlayerPrefs.SetInt("Lives", newLives);
            UIController.UpdateLives();
        }
        else
        {
            ActiveEndScene();
        }
        
        
        

    }

    public void ReduceDistance(int amount)
    {
        distanceToWin -= amount;

        if (distanceToWin <= 0)
        {
             distanceToWin = 0;
            GameManager.Instance.ActiveEndScene();
        }
       


        PlayerPrefs.SetInt("distanceToWin", distanceToWin);

        if (UIController != null)
            UIController.UpdateDistance(distanceToWin);
    }

    
    

    public void CheckGameOver()
    {
        if (GetCurrentLives() == 0)
        {
            ActiveEndScene();
        }
    }
    public void ActiveEndScene()
    {
        SceneManager.LoadScene("EndScene");
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
