using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Za komunikaciju sa serverom dodati
using System.IO;
using System.Net;

public static class APIHelper
{
    // Metoda vraća elemente sa servera
    public static string GetNewElement()
    {
        // Potrebno je definisati zahtev koji će pokupiti informacije
        // Javiće grešku pa je potrebno pristupiti sa http zahtevom i zato se navodi (HttpWebRequest)
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3000/Items");

        // Potrebno je definisati odgovor koji nam dostavlja informacije po zahtevu
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        // Potreban je čitač strimova za čitanje odgovora tj requesta
        StreamReader reader = new StreamReader(response.GetResponseStream());

        // Potrebno je da preuzmemo sav sadržaj sada iz readera
        string json = reader.ReadToEnd();
        return json;

        // Potrebno je da u JSON formatu vratimo pokupljene podatke
        //return JsonUtility.FromJson<HighscoreElement>(json); 
    }
}
