using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XboxCtrlrInput;

public enum GameState { Loading, Placement, Play, Stop, GameOver };

public class GameManager : MonoBehaviour {

     // Classes
     //[HideInInspector]
     // public SoundManager soundManager;

     [HideInInspector]
     public bool canUpdate = false;

     //[HeaderAttribute("Intro Animation")]
     //[SerializeField]
     //private Ease easeType = Ease.InSine;
     //private float easeLength = 0;

     [SpaceAttribute]
     public CanvasGroup gameEndPanel;   //Panel during Game Over
     public CanvasGroup roundNumberPanel;   //Panel displaying which round the game is in
     public CanvasGroup resultPanel;    //Panel displaying the score at the end of each round
     public CanvasGroup hudPanel;   //Panel displaying the scores and number of cats and dogs during the game

     [SpaceAttribute]
     public int Round = 1;  // the current round number
     public int TotalTurns = 3; // the total number of rounds for the game

     [SpaceAttribute]
     public int Player1Score = 0;
     public int Player2Score = 0;

     [SpaceAttribute]
     public int NumCatsAlive = 3;   
     public int NumDogsAlive = 3;

     [SpaceAttribute]
     public int TotalNumCats = 4;     //initial number of cats when the game starts - this needs to match to how many the Spawn Manager spawns
     public int TotalNumDogs = 2;   //initial number of cats when the game starts - this needs to match to how many the Spawn Manager spawns

     [SpaceAttribute]
     public float TimeToWait = 2;   //wait for spawning before starting the game

     public bool IsPlayer1aCat = false; //used for the players switching sides

     public GameState gamestate;

     public bool HasCatReachedTarget = false;

    private CatController[] CatArray;  //stores all the live cats in the game
    private DogController[] DogArray;  //stores all the live dogs in the game
    private TurretController[] TurretArray; //stores all the live turrets in the game
    private LaserTurretController[] LaserTurretArray; //stores all the live lasers in the game

    void Awake() {

        //soundManager = FindObjectOfType<SoundManager>();

    }

    // Use this for initialization
    void Start() {

        gamestate = GameState.Loading;

        gameEndPanel.DOFade(0, 0);
        gameEndPanel.blocksRaycasts = false;
        resultPanel.DOFade(0, 0);

        roundNumberPanel.GetComponentInChildren<TextMeshProUGUI> ().text = "Round " + Round;

        roundNumberPanel.DOFade(1, 1).SetDelay(0);
        roundNumberPanel.DOFade(0, 1).SetDelay(1);

        UpdateHud();

        hudPanel.DOFade(0, 0);
        hudPanel.DOFade(1, 1).SetDelay(2).OnComplete(Initialize);

    }

    void UpdateHud() {
        
        CatArray = FindObjectsOfType<CatController> ();
        DogArray = FindObjectsOfType<DogController> ();
        TurretArray = FindObjectsOfType<TurretController> ();
        LaserTurretArray = FindObjectsOfType<LaserTurretController> ();

        //NumDogsAlive = DogArray.Length - TurretArray.Length - LaserTurretArray.Length;
        NumDogsAlive = DogArray.Length;
        NumCatsAlive = CatArray.Length;

        if (IsPlayer1aCat) {
            hudPanel.GetComponentsInChildren<TextMeshProUGUI> ()[0].text = "Number of Cats Alive : " + NumCatsAlive;
            hudPanel.GetComponentsInChildren<TextMeshProUGUI> ()[1].text = "Number of Dogs Alive : " + NumDogsAlive;

        } else {
            hudPanel.GetComponentsInChildren<TextMeshProUGUI> ()[0].text = "Number of Dogs Alive : " + NumDogsAlive;
            hudPanel.GetComponentsInChildren<TextMeshProUGUI> ()[1].text = "Number of Cats Alive : " + NumCatsAlive;
        }

        hudPanel.GetComponentsInChildren<TextMeshProUGUI> ()[2].text = "Player A Score : " + Player1Score;
        hudPanel.GetComponentsInChildren<TextMeshProUGUI> ()[3].text = "Player B Score : " + Player2Score;
    }

    void Initialize() {
        
        gamestate = GameState.Placement;

        NumCatsAlive = TotalNumCats;
        NumDogsAlive = TotalNumDogs;

        HasCatReachedTarget = false;

        SwitchPlayers();

        //This is where the spawning placement function should be

        //to respawn dogs every initialization
        // DogController dog1 = Instantiate(dogPrefab, new Vector3(0.0f, 0.0f, 0.0f), transform.rotation);
        // dog1.transform.DOScale(1, 0);
        // dog1.health = 1;

        //after spawning is finished start the game
        StartCoroutine(StartGame());

    }

    void SwitchPlayers() {
        IsPlayer1aCat = !IsPlayer1aCat;
        UpdateHud();
    }

    public IEnumerator StartGame() {

        yield return new WaitForSeconds(TimeToWait);

        canUpdate = true;
        gamestate = GameState.Play;

        yield break;

    }

    public void StopGame() {

        canUpdate = false;
        gamestate = GameState.Stop;

        //soundManager.StopMusicSource();

        if (NumDogsAlive == 0 || HasCatReachedTarget) {
            //Invoke("CatsWin", soundManager.GetLength(ClipType.Finish));
            Invoke("CatsWin", 0);
        }
        if (NumCatsAlive == 0) {
            //Invoke("DogsWin", soundManager.GetLength(ClipType.Finish));
            Invoke("DogsWin", 0);
        }

    }

    void CatsWin() {
        if (IsPlayer1aCat) {
            Player1Score++;
        } else {
            Player2Score++;
        }

        Round++;
        UpdateHud();

        if (Round == TotalTurns + 1) {
            if (Player1Score > Player2Score) {
                gameEndPanel.GetComponentInChildren<TextMeshProUGUI> ().text = "Cats Win. Player 1 WON OVERALL \n PlayerA  " + Player1Score + " : PlayerB  " + Player2Score;
            } else if (Player1Score > Player2Score) {
                gameEndPanel.GetComponentInChildren<TextMeshProUGUI> ().text = "Cats Win. Player 2 WON OVERALL \n PlayerA  " + Player1Score + " : PlayerB  " + Player2Score;
            } else {
                gameEndPanel.GetComponentInChildren<TextMeshProUGUI> ().text = "Cats Win. OVERALL It's a tie. \n PlayerA  " + Player1Score + " : PlayerB  " + Player2Score;
            }

            gameEndPanel.DOFade(1, 1).OnComplete(EnableBlockRaycasts);

            gamestate = GameState.GameOver;
        } else {
            resultPanel.GetComponentInChildren<TextMeshProUGUI> ().text = "CATS WON THIS ROUND \n PlayerA  " + Player1Score + " : PlayerB  " + Player2Score;
            resultPanel.DOFade(0, 0);
            resultPanel.DOFade(1, 1).SetDelay(1);
            resultPanel.DOFade(0, 1).SetDelay(3);

            roundNumberPanel.GetComponentInChildren<TextMeshProUGUI> ().text = "Round " + Round + "\n Switch Players";
            roundNumberPanel.DOFade(0, 0);
            roundNumberPanel.DOFade(1, 1).SetDelay(5);
            roundNumberPanel.DOFade(0, 1).SetDelay(7).OnComplete(Initialize);
        }

    }

    void DogsWin() {
        if (IsPlayer1aCat) {
            Player2Score++;
        } else {
            Player1Score++;
        }
        Round++;
        UpdateHud();

        if (Round == TotalTurns + 1) {
            if (Player1Score > Player2Score) {
                gameEndPanel.GetComponentInChildren<TextMeshProUGUI> ().text = "Dogs Win. Player 1 WON OVERALL \n PlayerA  " + Player1Score + " : PlayerB  " + Player2Score;
            } else if (Player1Score > Player2Score) {
                gameEndPanel.GetComponentInChildren<TextMeshProUGUI> ().text = "Dogs Win. Player 2 WON OVERALL \n PlayerA  " + Player1Score + " : PlayerB  " + Player2Score;
            } else {
                gameEndPanel.GetComponentInChildren<TextMeshProUGUI> ().text = "Dogs Win. OVERALL It's a tie. \n PlayerA  " + Player1Score + " : PlayerB  " + Player2Score;
            }

            gameEndPanel.DOFade(1, 1).OnComplete(EnableBlockRaycasts);

            gamestate = GameState.GameOver;
        } else {
            resultPanel.GetComponentInChildren<TextMeshProUGUI> ().text = "DOGS WON THIS ROUND \n PlayerA  " + Player1Score + " : PlayerB  " + Player2Score;
            resultPanel.DOFade(0, 0);
            resultPanel.DOFade(1, 1).SetDelay(1);
            resultPanel.DOFade(0, 1).SetDelay(3);

            roundNumberPanel.GetComponentInChildren<TextMeshProUGUI> ().text = "Round " + Round + "\n Switch Players";
            roundNumberPanel.DOFade(0, 0);
            roundNumberPanel.DOFade(1, 1).SetDelay(5);
            roundNumberPanel.DOFade(0, 1).SetDelay(7).OnComplete(Initialize);
        }

    }

    void EnableBlockRaycasts() {
        gameEndPanel.blocksRaycasts = true;
    }

    // Update is called once per frame
    void Update() {
        if (canUpdate) {
            UpdateHud();

            if (NumDogsAlive == 0 || NumCatsAlive == 0 || HasCatReachedTarget) // or the cat has reached the target
            {
                StopGame();
            }
        }

        if (gamestate == GameState.GameOver) {
            if (XCI.GetButtonDown(XboxButton.A)) { 
                SceneManager.LoadScene(0);
            }
        }

    }

}