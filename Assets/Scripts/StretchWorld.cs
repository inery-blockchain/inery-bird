using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StretchWorld : MonoBehaviour
{
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private GameObject ground;
    [SerializeField]
    private float bg_scale_x = 22f;
    private float bg_scale_y = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v3ViewPort = new Vector3(1, 1, 10);
        float scaler;
        if (Camera.main.ViewportToWorldPoint(v3ViewPort).y / 5 > Camera.main.ViewportToWorldPoint(v3ViewPort).x / 10)
            scaler = Camera.main.ViewportToWorldPoint(v3ViewPort).y / 5;
        else
            scaler = Camera.main.ViewportToWorldPoint(v3ViewPort).x / 10;
        background.transform.localScale = new Vector3(bg_scale_x * scaler, bg_scale_y * scaler, 1);
        background.transform.position = new Vector3(background.transform.position.x, (background.transform.localScale.y - bg_scale_y + 1) / 2, background.transform.position.z);
        ground.transform.localScale = new Vector3(bg_scale_x * scaler, 1, 1);
    }
}
