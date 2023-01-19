using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Potrebna je promenjiva za brzinu kretanja novčića ulevo
    // Brzina treba da bude jednaka brzini cevi
    public float speed = 4.5f;

    // Potrebno je da znamo krajnju ivicu
    private float leftEdge;
    void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 3;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Potrebno je da uništimo cevi kada oni više nisu u kadru
        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
}
