using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Potrebna je promenjiva sa kojom pristupamo folderu Prefabs, gde se nalazi objekat cevi
    public GameObject[] prefabs;

    // CEVI
    // Promenjiva koja određuje vreme za koje se cevi spavnuju
    public float spawnRatePipes = 1f;
    // Potrebne su promenjive koje će određivati pozicije cevi
    public float minHeight = -1f;
    public float maxHeight = 1f;

    // NOVČIĆI Inery
    public float spawnRateCoinInery = 1f;


    // NOVČIĆI Bitcoin
    //public float spawnRateCoinBitcoinMin = 1f;
    //public float spawnRateCoinBitcoinMax = 2f;
    // private int rand = 0;

    // OnEnable() funkcija se poziva kada objekat postane aktivan, što znači da će se isključiti kada se deaktivira - kada igrač pogine u ovom slučaju
    private void OnEnable()
    {
        // Pozivamo metodu Spawn() za zadato vreme
        InvokeRepeating(nameof(SpawnPipes), spawnRatePipes, spawnRatePipes);
        InvokeRepeating(nameof(SpawnCoinInery), spawnRateCoinInery, spawnRateCoinInery);
    }
    // Uvodimo metodu koja se aktivira kada objekat nestane
    private void OnDisable()
    {
        CancelInvoke(nameof(SpawnPipes));
        CancelInvoke(nameof(SpawnCoinInery));
        //CancelInvoke(nameof(SpawnCoinBitcoin));
    }

    void Update()
    {
        // rand = Random.Range(0, 2);
    }
    // Metoda za stvaranje cevi
    private void SpawnPipes()
    {
        // Potrebno je da instanciramo objekat cevi iz prefabs foldera
        // Quaternion.identity - označava da nema rotacije tj. rotacija je 0,0,0
        GameObject pipes = Instantiate(prefabs[0], transform.position, Quaternion.identity);
        // Određujemo nasumičnu poziciju po y osi za naš objekat cevi
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
    }

    private void SpawnCoinInery()
    {
        // Isto je i sa spawnovanjem koina
        // if (rand == 0)
        // {
            GameObject coin = Instantiate(prefabs[1], transform.position, Quaternion.identity);
            coin.transform.position += Vector3.up * Random.Range(0f, 0.5f);
        // }
        /*
        else
        {
            GameObject coin = Instantiate(prefabs[2], transform.position, Quaternion.identity);
            coin.transform.position += Vector3.up * Random.Range(0f, 0.9f);
        }
        */
    }

    private void SpawnCoinBitcoin()
    {
        GameObject coin = Instantiate(prefabs[2], transform.position, Quaternion.identity);
        coin.transform.position += Vector3.up * Random.Range(0f, 0.9f);
    }
}
