using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    private TMP_Text gameOverText;

    void Start()
    {
        gameOverText = GetComponent<TMP_Text>();
        gameOverText.alpha = 1f;
        gameOverText.CrossFadeAlpha(0f, 0f, true);
    }

    // Update is called once per frame
    void Update()
    {
        gameOverText.CrossFadeAlpha(1f, 1.5f, false);
        if (Input.anyKey)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
