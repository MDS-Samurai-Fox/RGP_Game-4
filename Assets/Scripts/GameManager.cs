using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XboxCtrlrInput;

public enum GameState { Loading, Placement, Play, Stop, GameOver };

public class GameManager : MonoBehaviour {

 private SpawnManager sm;
 private SoundManager soundManager;

 [HideInInspector] public bool canUpdate = false;

 [SpaceAttribute]
 public CanvasGroup gameEndPanel; //Panel during Game Over
 public CanvasGroup roundNumberPanel; //Panel displaying which round the game is in
 public CanvasGroup resultPanel; //Panel displaying the score at the end of each round
 public CanvasGroup splitPanel; //Panel displaying the score at the end of each round

 [SpaceAttribute]
 public int Round = 1; // the current round number
 public int TotalTurns = 3; // the total number of rounds for the game

 [SpaceAttribute]
 public int Player1Score = 0;
 public int Player2Score = 0;

 public int NumCatsAlive = 3;
 public int NumDogsAlive = 3;

 [SpaceAttribute]
 public float TimeToWait = 2; //wait for spawning before starting the game

 public bool IsPlayer1aCat = false; //used for the players switching sides

 public GameState gamestate;

 public bool HasCatReachedTarget = false;

 private CatController[] CatArray; //stores all the live cats in the game
 private DogController[] DogArray; //stores all the live dogs in the game
 private TurretController[] TurretArray; //stores all the live turrets in the game
 private LaserTurretController[] LaserTurretArray; //stores all the live lasers in the game

 void Awake() {

 sm = GetComponent<SpawnManager> ();
 soundManager = FindObjectOfType<SoundManager> ();

    }

    // Use this for initialization
    void Start() {

        gamestate = GameState.Loading;

        gameEndPanel.DOFade(0, 0);
        gameEndPanel.blocksRaycasts = false;
        resultPanel.DOFade(0, 0);

        roundNumberPanel.GetComponentInChildren<TextMeshProUGUI> ().text = "Round " + Round;

        roundNumberPanel.DOFade(1, 1).SetDelay(0);
        roundNumberPanel.DOFade(0, 1).SetDelay(1).OnComplete(Initialize);

    }

    void Initialize() {

        gamestate = GameState.Placement;

        HasCatReachedTarget = false;

        SwitchPlayers();

    }

    void SwitchPlayers() {

        IsPlayer1aCat = !IsPlayer1aCat;
        // UpdateHud();

    }

    public IEnumerator StartGame() {

        yield return new WaitForSeconds(TimeToWait);

        canUpdate = true;
        gamestate = GameState.Play;
        splitPanel.DOFade(0, 0);

        yield break;

    }

    public void StopGame() {

        canUpdate = false;
        gamestate = GameState.Stop;

        //soundManager.StopMusicSource();

        if (NumDogsAlive == 0 || HasCatReachedTarget)
        {
            CatsWin();
            // Invoke("CatsWin", 0);
        }
        if (NumCatsAlive == 0)
        {
            DogsWin();
            // Invoke("DogsWin", 0);
        }

    }

    void CatsWin() {
        if (IsPlayer1aCat) {
            Player1Score++;
        } else {
            Player2Score++;
        }

        Round++;
        // UpdateHud();

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
        // UpdateHud();

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

            // UpdateHud();

            CatArray = FindObjectsOfType<CatController>();
            DogArray = FindObjectsOfType<DogController>();

            NumCatsAlive = CatArray.Length;
            NumDogsAlive = DogArray.Length;

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