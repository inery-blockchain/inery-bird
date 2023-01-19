using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    // Potrebno je da upravljamo komponentom Mesh Render(Mrežni render) za našu pozadinu, pa uvodimo sledeću promenjivu
    private MeshRenderer meshRenderer;
    // Potrebno je da uvedemo promenjivu koja predstavlja brzinu pomeranja pozadine
    public float animationSpeed = 0.05f;

    private void Awake()
    {
        meshRenderer= GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        // Vršimo pomeranje glavne teksture tj. pozadinske slike za 
        meshRenderer.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0);
    }
}
