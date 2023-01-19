using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Za upis i ispis podataka u datoteku potrebna je sledeća biblioteka
using System.IO;
using System;
using System.Linq;

public static class FileHendler
{
    // Ova skripta komunicira za JSON fajlom

    // Unutar ove metode imaćemo generičku listu određenog tipa i zato ima ovo <T>
    // Konkretno ova lista će biti tipa klase InputEntry, ali ovakvom deklaracijom možemo koristiti više tipova liste
    // fileName predstavlja ime datoteke, JER nekad može da bude drugačije u zavisnosti šta čuvamo
    public static void SaveToJSON<T>(List<T> toSave, string fileName)
    {
        // Štampamo putanju gde je datoteka kreirana pozivom definisane metode za to
        Debug.Log(GetPath(fileName));
        string content = JsonHelper.ToJson<T>(toSave.ToArray());
        WriteFile(GetPath(fileName), content);
    }

    // Kreiramo metodu za čitanje podataka iz JSON fajla smeštajući ih u niz
    public static List<T> ReadFromJSON<T>(string filename)
    {
        string content = ReadFile(GetPath(filename));

        // Potrebno je da sadržaj iz content konvertujemo u listu objeata
        // Pa je potrebno proveriti da li je string prazan
        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            // U koliko je prazn string vraća se prazna generička lista
            return new List<T>();
        }

        List<T> res = JsonHelper.FromJson<T>(content).ToList();
        return res;
    }

    // Potrebna je ovakva metoda koja vraća putanju. Kao parametar sadrži ime datoteke
    public static string GetPath(string filename)
    {
        // persistentDataPath predstavlja putanju na kojoj možemo čuvati podatke i upotrebiti ih prilikom pokretanja projekta
        return Application.persistentDataPath + "/" + filename;
    }
    // Metoda za upisivanje informacija u datoteku na osnovu putanje. Drugi parametar predstavlja sadržaj za upisivanje
    static void WriteFile(string path, string content)
    {
        // Sledeći kod kreira novu datoteku ili zamenjuje postojeću
        FileStream fileStream = new FileStream(path, FileMode.Create);
        // Kada datoteka postoji vrši se upisivanje
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.WriteLine(content);
        }
    }

    // Metoda za čitanje datoteke
    // Kao parametar imaćemo putanju datoteke
    private static string ReadFile(string path)
    {
        // U koliko na zadatoj putanji postoji fail sa zadatim imenom možemo početi da je čitamo
        if (File.Exists(path))
        {

            // Za čitanje datoteke koristi se StreamReader
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }

    // ---------------------------------------------------------------------------------------------------------------------------------------
    // Stackoverflow rešenje

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        // Kreiramo kontent za json šablon
        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        // Metoda kao parametre ima generički niz i ...
        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            // Uzimamo deklarisanu generičku listu i kreiramo objekat po tipu generičke klase Wrapper ispod
            Wrapper<T> wrapper = new Wrapper<T>();
            // Upsujemo parametar u promenjivu. Sada imamo vrednost koja sadrži elemente niza
            wrapper.Items = array;
            // Pristupamo sistemskoj klasi i formatiramo elemente
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        // Generička klasa
        [Serializable]
        private class Wrapper<T>
        {
            // U klasi imamo generički niz
            public T[] Items;
        }
    }

}
