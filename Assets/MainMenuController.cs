using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(moveToGame);
        quitButton.onClick.AddListener(quitGame);
    }

    // Update is called once per frame
    void Update() { }

    void moveToGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    void quitGame()
    {
        Application.Quit();
    }
}
