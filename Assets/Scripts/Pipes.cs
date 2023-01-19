using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    public static Pipes instance;

    private void Awake()
    {
        instance = this;
    }

    // Uvodimo brzinu kretanja cevi
    public float speed = 4.5f;
    // Potrebna nam je pozicija leve strane ekrana
    private float leftEdge;

    void Start()
    {
        // Uzimamo poziciju krajnje leve tačke kadra tj. kamere, koja ima vrednost 0 unutar ove funkcije ScreenToWorldPoint i to po x osi i umanjujemo je za 1 kako bi ceo objekat cevi izašao iz kadra a ne pola
        // Nakon toga će se objekat cevi uništiti
        // Camera.main.ScreenToWorldPoint(Vector3.zero).x = krajnjoj levoj tačci na ivici kamere
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 3;
    }

    void Update()
    {
        // Pomeramo cevi zadatom brzinom
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Potrebno je da uništimo cevi kada oni više nisu u kadru
        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
}
