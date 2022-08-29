using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Debug.Log(gameOverText.alpha);
        gameOverText.CrossFadeAlpha(1f, 1.5f, false);
    }
}
