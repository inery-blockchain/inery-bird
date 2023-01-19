using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Promenjive za igrača
    private Vector3 direction;
    // Hoćemo da podesimo gravitaciju objekta igrača
    public float gravity = -10.8f;
    // Koliko jako želimo da odbacimo igrača na gore, pritiskom na dugme
    public float strenght = 5f;

    // Potrebno je da menjamo sprajtove, pa nam je potrebna komponenta sprajt render
    private SpriteRenderer spriteRenderer;
    // Imaćemo niz sprajtova koji će se smenjivati u određenom trenutku
    public Sprite[] sprites;
    // Potrebna nam je još jedna promenjiva koja će upravljati indeksima sprajtova iz niza [] sprites
    private int spriteIndex;

    // Awake() - Funkcija se poziva samo jednom kada igra počne, dok se Start() poziva kada se pokrene prvi frejm. Dakle funkcija budnosti Awake() se pre poziva
    // Takođe funkcija Awake() u bilo kojoj skripti će se pozvati pri pokretanju igre, čak i da nije dodeljena kao komponenta ni jednom objektu
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // InvokeRepeating() - nam omogućava da pozivamo određenu funkciju konstantno za određeno vreme
        // InvokeRepeating() sadrži 3 parametra: naziv metode, vreme čekanja pre nego što se izvrši i kašnjenje nakon svakog sledećeg izvršenja u sekundama
        // InvokeRepeating("AnimateSprite", 0.15f, 0.15f);
    }

    private void Update()
    {
        // Kada pritisnemo SPEJS ili LEVI KLIK miša ovaj uslov će biti ispunjen
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strenght;
        }

        // U koliko igru kreiramo za MOBILNE UREĐAJE
        // --------------------------------------------------------------------
        // U koliko je broj dodira veći od 0 uslov će biti ispunjen. Dakle proveravamo da li uopšte postoje dodiri na ekranu
        if (Input.touchCount > 0)
        {
            // Input.GetTouch(0) predstavlja tj. vraća dodir na ekranu, kao i poziciju dodira na ekranu
            Touch touch = Input.GetTouch(0);

            // Kada je phase (faza) jednaka dodiru tj. fazi u kojoj je dodir počeo
            // Pored Began faze koja predstavlja fazu stavljanja prsta na ekran, postoji i Moved(pomeranje prsta po ekranu), Stationary(kada stacioniramo tj. dodirnemo ekran i prst ostane na toj tačci), Ended(Kada prst prestane da dodiruje ekran), Cancelled(Kada se poništi dodir)
            if (touch.phase == TouchPhase.Began)
            {
                // Određuje se sila odbacivanja za igrača
                direction = Vector3.up * strenght;
            }
        }
        // --------------------------------------------------------------------

        // Potrebno je da dodamo gravitaciju koja deluje po Y osi na igrača
        direction.y += gravity * Time.deltaTime;

        // Kada smo definisali gravitaciju i jačinu odbacivanja, utičemo na poziciju i da bi bila konstantna brzina pozicije množimo sa Time.deltaTime (Time.deltaTime = 1/FPS)
        transform.position += direction * Time.deltaTime;
    }

    private void AnimateSprite()
    {
        // Nakon svakog poziva metode indeks se povećava i menja sprajtove
        spriteIndex++;
        if (spriteIndex >= sprites.Length)
        {
            // U koliko indeks dođe do kraja niza setovaćemo ga ponovo na 0
            spriteIndex = 0;
        }
        // Menjaće se sprajtovi igraču
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    // Definišemo šta se događa kada igrač dođe u dodir sa nekim drugim kolajderom
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Proveravamo o kom tagu tj. objektu igre se radi
        if (other.gameObject.tag == "Obstacle")
        {
            GameManager.instance.GameOver();
        }

        else if (other.gameObject.tag == "Scoring")
        {
            GameManager.instance.IncreaseScore();
        }

        else if (other.gameObject.tag == "Coin")
        {
            AudioManager.instance.PlaySFX(1);
            GameManager.instance.IncreaseScoreCoin();
            Destroy(other.gameObject);
        }
    }

    
    // Ova funkcija se izvršava kada se objekat u igri aktivira, dakle izvršiće se kada kliknemo na Play dugme
    public void EnablePlayer()
    {
        // Potrebno je da poziciju igrača setujemo na 0 po y osi
        Vector3 position =  transform.position;
        position.y = 0f;
        position.x = -1.5f;
        transform.position = position;

        direction = Vector3.zero;
    }
    
}