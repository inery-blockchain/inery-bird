using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    public static HighscoreTable instance;

    // Potrebna je referenca na UI Panel
    public GameObject panel;

    // Potrebna je referenca na objekat u prefabu, koji sadrži ime i skor igrača
    public GameObject highscoreUIElementPrefab;

    // Takođe je potrebna promenjiva za roditeljski element u kome se nalazi ovaj prefab element
    public Transform elementWrapper;

    // Potrebna na mje referenca na skriptu highscoreHandler
    public HighscoreHandler highscoreHandler;

    // Potrebna je lista u koju se čuvaju objekti iz prefab foldera za imena i poene
    List<GameObject> uiElements = new List<GameObject>();


    private void Awake()
    {
        instance = this;
    }

    // OnEnable() se poziva kada objekat postane aktivan ili omogućen
    private void OnEnable()
    {
        HighscoreHandler.onHighscoreListChanged += UpdateUI;
    }

    private void OnDisable()
    {
        // Da bi sprečili utrošak memorije i resursa, kada se objekat ugasi ugasiće se i metoda
        HighscoreHandler.onHighscoreListChanged -= UpdateUI;
    }

    // Potrebno je definisati metode za otvaranje i zatvaranje panela
    public void ShowPanel()
    {
        highscoreHandler.GetHighscoreData();
        //HighscoreHandler
        panel.SetActive(true);

    }
    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    // Potrebna je metoda koja ažurira panel kad god se pozove
    private void UpdateUI(List<HighscoreElement> list)
    {
        // Potrebno je da prođemo kroz celu listu, koja sadrži imena i poene
        for (int i = 0; i < list.Count; i++)
        {
            // Čuvamo trenutni element u privremenu poziciju
            HighscoreElement el = list[i];
            // Proverićemo da kojim slučajem nije manji od 0
            if (el.points > 0)
            {
                // Kada dođemo do zadnjeg elementa u listu sledeći uslov biće ispunjen
                if (i >= uiElements.Count)
                {


                    // Instanciramo novi objekat iz prefaba
                    var inst = Instantiate(highscoreUIElementPrefab, Vector3.zero, Quaternion.identity);

                    // Sada postavljamo roditeljski objekat za instancirani objekat. True znači da ga pomeri na osnovu njega
                    inst.transform.SetParent(elementWrapper, false);

                    // Kada smo stvorili i pozicionirali prefab objekat za skor i ime, potrebno ga je dodati u listu objekata
                    uiElements.Add(inst);

                }
                // Uzimamo pojedinačno za svaki element ime igrača i poene iz trenutnog objekta iz liste
                // GetComponentsInChildren<Text>() Unutar liste uiElements objekta tražimo sve tekstualne objekte koji su "deca"
                var texts = uiElements[i].GetComponentsInChildren<Text>();
                // Pošto su nam objekti tako raspoređeni da imamo prvi tekstualni objekat koji se tiče imena igrača, pa ispod njega imamo broj poena naš niz koji sadrži tekstove će na sledeći način rasporediti elemente
                texts[0].text = el.playerName;
                texts[1].text = el.points.ToString();
            }
        }
    }
}
