using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        float camera_x = player.transform.position.x;
        if (camera_x <= 0)
        {
            camera_x = 0.0f;
        }
        transform.position = new Vector3(camera_x, transform.position.y, transform.position.z);
    }
}
