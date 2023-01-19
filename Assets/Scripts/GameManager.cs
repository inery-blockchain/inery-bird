using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Pošto radimo sa korisničkim interfejsom potrebna nam je ova biblioteka
using UnityEngine.UI;
using IneryActions;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour
{
    // Uvodimo promenjivu koja je instanca ove klase (singlton patern)
    public static GameManager instance;

    // Potrebna nam je promenjiva za rezultat. Biće privatna jer nije potrebna u Unity editoru
    private int score;

    // Potrebna je referenca na nickname
    public Text nickname;

    // Potrebna nam je promenjiva za tekst na ekranu
    public Text scoreText;

    // Potreban je promenjiva koja obezbeđuje da se zvuk za oboreni rekord aktivira jedamput
    private bool soundCheck = false;

    // Potrebna je promenjiva koja će referencirati na dugme play
    // Referenciraćemo dugme kao objekat, zato što ima sliku i tekst sa kojim želimo da upravljamo
    public GameObject playButton;

    // Potrebna je referenca za dugme za skor, kako bi mogli da ga ugasimo tokom igre
    public GameObject highscoreButton;

    // Referenca na sliku GameOver
    public GameObject gameOver;

    // Takođe potrebna je i referenca na igrača
    public Player player;

    // Potrebna je promenjiva koja će uzimati vrednost najboljeg skora iz prefabs
    private int highScore;

    // Potrebna je referenca na tekst highScore
    public Text highscoreText;

    // Potrebna je promenjiva koja će da čuva najveći skor u prethodnim sesijama igranja
    private int initialScore;

    // Potrebna je referenca na input za unos imena
    public InputField nameInput;

    // Potrebna je referenca na klasu HighscoreHandler
    public HighscoreHandler highscoreHandler;

    // Provera da li je igra pokrenuta da bi ugasili playButton
    private bool gameStart;

    public GameObject yourNameText;
    public InputField emailInput;
    public Text yourEmail;
    public GameObject keyboard;
    public GameObject subscribeButton;

    // Potrebna je referenca na spawner, kako bi mu setovali lokaciju
    public GameObject spawner;
    private float rightEdge;

    // Potrebne su reference na broj prikupljenih novčića
    public Text coinCounterText;
    private int coinCounter;

    [SerializeField]
    private Button[] keyboard_part = new Button[12];

    [SerializeField]
    private GameObject MessagePanel;
    [SerializeField]
    private GameObject cancelButton;
    [SerializeField]
    private Text message;
    [SerializeField]
    private GameObject infoButton;
    [SerializeField]
    private GameObject infoPanel;
    [SerializeField]
    private GameObject hideButton;

    private void Awake()
    {
        // promenjiva instance koja predstavlja instancu je jednaka ovoj klasi
        instance = this;

        // Ograničavamo igru da radi najviše u 60fps-a
        Application.targetFrameRate = 60;

        // Igra će na početku biti pauzirana
        Pause();
    }

    private void Start()
    {
        // TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        // Potrebno je ograničiti unos karaktera u imenu
        nameInput.characterLimit = 5;


        // Na početku igra će biti pauzirana, zato je potrebno setovati promenjivu gameStart na false
        gameStart = false;

        // Za birsanje svih ključeva koristi se sledeći kod
        // PlayerPrefs.DeleteAll();

        initialScore = PlayerPrefs.GetInt("Highscore", 0);

        // Pri pokretanju igre potrebno je da odmah imamo uvid u najveći postignut skor
        SetLatestHighscore();

        SpawnerSetPosition();
    }

    void Update()
    {
        if (nameInput.text != "" && nameInput.text.Length > 1 && !gameStart)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }

        if (nameInput.isFocused)
        {
            ShowKeyboard(true, true);
            highscoreButton.SetActive(false);
        }
        else if (emailInput.isFocused)
        {
            ShowKeyboard(true, false);
            highscoreButton.SetActive(false);
        }



        // Čuvamo skor u koliko je oboren, tako da se povećava i skor i highscore
        OnConnectedToServer(score);

    }

    public void SpawnerSetPosition()
    {
        rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 10)).x + 1;
        spawner.transform.position = new Vector3(rightEdge, spawner.transform.position.y, spawner.transform.position.z);
    }

    public void ShowKeyboard(bool check, bool name)
    {
        if (name)
        {
            foreach (Button bt in keyboard_part)
            {
                bt.interactable = false;
            }
        }
        else
        {
            foreach (Button bt in keyboard_part)
            {
                bt.interactable = true;
            }
        }
        keyboard.SetActive(check);
    }

    /*public void OpenKeyboard()
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }*/
    // Funkcija koja se poziva kada igrač pogine
    public void GameOver()
    {
        if (score > 0)
        {
            highscoreHandler.SaveHighscoreData(nameInput.text, score.ToString(), coinCounter.ToString());
        }
        AudioManager.instance.bgm.Stop();
        AudioManager.instance.PlaySFX(0);

        playButton.transform.position = new Vector3(highscoreButton.transform.position.x, playButton.transform.position.y, playButton.transform.position.z);

        gameOver.SetActive(true);
        playButton.SetActive(true);
        highscoreButton.SetActive(true);




        Pause();
    }

    // Potrebna je funkcija za povećanje rezultata
    public void IncreaseScore()
    {
        score++;

        // Metoda proverava da li skor u igri prelazi highscore za 1 i aktivira zvučni efekat
        ScorHighscor();

        // Pristupamo tekstualnoj komponenti i setujemo vrednost iz promenjive score
        scoreText.text = score.ToString();
    }

    public void IncreaseScoreCoin()
    {
        AudioManager.instance.PlaySFX(1);
        score += 5;

        coinCounter++;

        // Metoda proverava da li skor u igri prelazi highscore za 1 i aktivira zvučni efekat
        ScorHighscor();

        scoreText.text = score.ToString();

        coinCounterText.text = coinCounter.ToString();
    }


    // Klikom na dugme pokrenuće se ova metoda Play()
    public void Play()
    {
        yourNameText.SetActive(false);

        // Klikom na dugme setujemo promenjivu gameStart na true, jer je igra počela
        gameStart = true;

        nameInput.gameObject.SetActive(false);

        initialScore = PlayerPrefs.GetInt("Highscore", 0);

        soundCheck = false;

        AudioManager.instance.bgm.Stop();
        AudioManager.instance.bgm.Play();


        // Kada počnemo sa igrom resetujemo skor na 0
        score = 0;
        scoreText.text = score.ToString();

        // Takođe broj pokupljenih novčića je potrebno setovati na 0
        coinCounter = 0;
        coinCounterText.text = coinCounter.ToString();

        // Potrebno je pogasiti sledeće komponente
        nickname.gameObject.SetActive(false);
        playButton.SetActive(false);
        gameOver.SetActive(false);
        highscoreButton.SetActive(false);
        keyboard.SetActive(false);
        emailInput.gameObject.SetActive(false);
        yourEmail.gameObject.SetActive(false);
        subscribeButton.SetActive(false);
        infoButton.SetActive(false);



        // Potrebno je vratiti relano vreme
        Time.timeScale = 1f;

        // Aktiviraćemo objekat igrača
        player.enabled = true;

        player.EnablePlayer();

        // Zatim je potrebno da uništimo svaki objekat cevi u kadru tj. glavni objekat u kom se cevi nalaze koji ima skriptu Pipes
        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }

        // Takođe isto je potrebno uraditi i za novčiće
        Coin[] coins = FindObjectsOfType<Coin>();
        for (int i = 0; i < coins.Length; i++)
        {
            Destroy(coins[i].gameObject);
        }
    }

    // Igra će na početku biti pauzirana
    public void Pause()
    {
        gameStart = false;
        // Zamrznućemo vreme, tako da se igra ne ažurira
        // Time.timeScale metoda utiče na sve Time.deltaTime vrenosti u igri
        Time.timeScale = 0f;

        player.enabled = false;

    }

    // SKOR----------------------------------------------------------------------------------------
    // Potrebna je metoda koja omogućava da se prikaže skor
    private void SetLatestHighscore()
    {
        // PlayerPrefs je klasa koja čuva određene podatke za našu igru, između sesija igranja
        // U koliko ne postoji vrednost onda će 0 biti podrazumevana
        highScore = PlayerPrefs.GetInt("Highscore", 0);

        highscoreText.text = highScore.ToString();
    }

    // Potrebna je metoda koja će omogućiti čuvanje postignutog skora
    private void SaveHighscore(int score)
    {
        PlayerPrefs.SetInt("Highscore", score);
    }

    // Potrebna je metoda koja će uporediti postignuti skor sa najvišim skorom
    public void OnConnectedToServer(int score)
    {
        if (score > highScore)
        {
            highScore = score;
            SaveHighscore(score);
            highscoreText.text = highScore.ToString();
        }
    }
    // SKOR----------------------------------------------------------------------------------------

    // Proveravamo da li je skor premašen
    private void ScorHighscor()
    {
        // U koliko trenutni skor premaši najviši skor aktiviraće se zvučni efekat, a promenjiva soundCheck je tu da osigura da se zvuk reprodukuje samo jednom
        if (score > initialScore && !soundCheck)
        {
            AudioManager.instance.PlaySFX(2);
            soundCheck = true;
        }

    }

    public void ClickCancel()
    {
        MessagePanel.SetActive(false);
    }

    // Klik na subscribe
    public void Subscribe()
    {
        Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);
        message.text = "";
        if (emailRegex.IsMatch(emailInput.text))
        {
            highscoreHandler.Subscribe(emailInput.text, emailInput, message);
        }
        else
        {
            message.text = "The email is not valid!";
        }
        MessagePanel.SetActive(true);
    }

    public void ShowPanelInfo()
    {
        infoPanel.SetActive(true);
    }
    public void HideInfoPanel()
    {
        infoPanel.SetActive(false);
    }
}