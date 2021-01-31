using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public GameObject _mainMenuCanvas;
    public GameObject _creditsCanvas;
    public GameObject _teamPickerCanvas;
    public GameObject _preFightCanvas;
    public GameObject _victoryCanvas;
    public GameObject _defeatCanvas;

    public Image _victoryImage;

    public Button FIGHTBUTTON;
    public Button backToMainMenuVBtn;
    public Button backToMainMenuDBtn;

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
        FIGHTBUTTON.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
        {
            GameManager.Instance.StartBattle();
        }));

        backToMainMenuVBtn.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
        {
            _victoryCanvas.SetActive(false);
            ShowMainMenu();
        }));

        backToMainMenuDBtn.onClick.AddListener((UnityEngine.Events.UnityAction)(() =>
        {
            _defeatCanvas.SetActive(false);
            ShowMainMenu();
        }));

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

    public void ShowTeams()
    {
        _mainMenuCanvas.SetActive(false);
        _teamPickerCanvas.SetActive(true);
    }

    public void ShowPreFight()
    {
        _teamPickerCanvas.SetActive(false);
        _preFightCanvas.SetActive(true);
    }

    public void ShowVictory()
    {
        GameManager.Instance.battleField.SetActive(false);
        _victoryImage.sprite = GameManager.Instance.SelectedTeam.VictoryImage;
        _victoryCanvas.SetActive(true);       
    }

    public void ShowDefeat()
    {
        GameManager.Instance.battleField.SetActive(false);
        _defeatCanvas.SetActive(true);
    }

    public void ChangePreFight(bool show)
    {
        _preFightCanvas.SetActive(show);
    }

    public void ExitButtonClick()
    {
        GameManager.Instance.Exit();
    }

}
