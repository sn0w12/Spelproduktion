using UnityEngine;

public class CamHandler : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform camobjekt;

    void FixedUpdate()
    {
        Vector3 pos = camobjekt.position;
        pos.z = -10;
        cam.transform.position = pos;

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}

