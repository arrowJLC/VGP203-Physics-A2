using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Button")]
    public Button playButton;
    public Button menuButton;
    public Button howToButton;
    public Button backButton;
    public Button quitButton;

    public GameObject mainMenu;
    public GameObject settingsMenu;

    void Start()
    {
        //Button Bindings
        if (menuButton) menuButton.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
        if (playButton) playButton.onClick.AddListener(() => SceneManager.LoadScene("FirstLevel"));
        if (howToButton) howToButton.onClick.AddListener(() => SetMenus(settingsMenu, mainMenu));
        if (backButton) backButton.onClick.AddListener(() => SetMenus(mainMenu, settingsMenu));
        if (quitButton) quitButton.onClick.AddListener(QuitGame);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    void SetMenus(GameObject menuToActivate, GameObject menuToDeactivate)
    {
        if (menuToActivate)
            menuToActivate.SetActive(true);

        if (menuToDeactivate)
            menuToDeactivate.SetActive(false);
    }
}

    
