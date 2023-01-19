using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Potrebno je da metode u skripti budu dostupne svim ostalim skriptama
    public static AudioManager instance;

    // Potreban nam je niz tipa AudioSource za zvučne efekte, jer je svaki audio fail ovog tipa
    public AudioSource[] soundEffects;

    // Potrebne su promenjive za kontrolu pozadinske muzike
    public AudioSource bgm;

    private void Awake()
    {
        instance = this;
    }
    void Starft()
    {

    }

    void Update()
    {

    }

    // Potrebna je funkcija za reprodukciju određenog zvučnog efekta. Funkcija ima parametar koji predstavlja zvučni efekat u nizu efekata soundEffects
    public void PlaySFX(int soundToPlay)
    {
        // Da bi krenuo zvučni efekat ispočetka, potrebno je da se stopira
        soundEffects[soundToPlay].Stop();
        // Reprodukcija zvučnog efekta
        soundEffects[soundToPlay].Play();
    }
}
