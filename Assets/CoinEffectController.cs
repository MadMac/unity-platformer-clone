using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEffectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BringUp());
    }

    // Update is called once per frame
    void Update() { }

    IEnumerator BringUp()
    {
        for (int i = 0; i < 10f; i++)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + 0.3f,
                transform.position.z
            );
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i < 10f; i++)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - 0.3f,
                transform.position.z
            );
            yield return new WaitForSeconds(0.02f);
        }
    }
}
