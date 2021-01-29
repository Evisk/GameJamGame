using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public GameObject _mainMenuCanvas;
    public GameObject _creditsCanvas;
    public GameObject _teamPickerCanvas;

    public GameObject _teamPanel;

    public Color statisticEmpty;
    public Color statisticFull;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (UIManager)FindObjectOfType(typeof(UIManager));

                if (_instance == null)
                {
                    GameObject sObject = new GameObject();
                    _instance = sObject.AddComponent<UIManager>();
                    sObject.name = "UIManager";
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void ShowCredits()
    {
        _mainMenuCanvas.SetActive(false);
        _creditsCanvas.SetActive(true);
    }

    public void ShowMainMenu()
    {
        _mainMenuCanvas.SetActive(true);
        _creditsCanvas.SetActive(false);
    }

    public void ExitButtonClick()
    {
        GameManager.Instance.Exit();
    }

}
