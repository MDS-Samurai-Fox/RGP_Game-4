using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
 public CanvasGroup gameEndPanel;
 public CanvasGroup roundNumberPanel;
 public CanvasGroup resultPanel;
 public CanvasGroup hudPanel;

 [SpaceAttribute]
 public int Round = 1;
 public int TotalTurns = 3;

 [SpaceAttribute]
 public int Player1Score = 0;
 public int Player2Score = 0;

 [SpaceAttribute]
 public int NumCatsAlive = 3;
 public int NumDogsAlive = 3;

 [SpaceAttribute]
 public int TotalNumCats = 4;
 public int TotalNumDogs = 2;

 [SpaceAttribute]
 public GameObject catPrefab;
 public DogController dogPrefab;

 [SpaceAttribute]
 public float TimeToWait = 2;

 private CatController[] CatArray;
 private DogController[] DogArray;
 private TurretController[] TurretArray;
 private LaserTurretController[] LaserTurretArray;

 private bool bGameEnd = false;

 bool IsPlayer1aCat = false;

 public GameState gamestate;

 Camera mainCamera;

 void Awake() {
 //soundManager = FindObjectOfType<SoundManager>();
 mainCamera = FindObjectOfType<Camera> ();
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

        //if (particles != null)
        //    particles.Stop();

    }

    void UpdateHud() {
        
        CatArray = FindObjectsOfType<CatController> ();
        DogArray = FindObjectsOfType<DogController> ();
        TurretArray = FindObjectsOfType<TurretController> ();
        LaserTurretArray = FindObjectsOfType<LaserTurretController> ();
        NumDogsAlive = DogArray.Length - TurretArray.Length - LaserTurretArray.Length;
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

        //This is where the spawning placement function should be

        //to respawn dogs every instantiation
        // DogController dog1 = Instantiate(dogPrefab, new Vector3(0.0f, 0.0f, 0.0f), transform.rotation);
        // dog1.transform.DOScale(1, 0);
        // dog1.health = 1;

        SwitchPlayers();

        // StartCoroutine(StartGame());

        //easeLength = soundManager.GetLength(ClipType.Join);

        //leftSideOriginalPosition = leftSide.localPosition;
        //    middleSideOriginalPosition = middleSide.localPosition;
        //    rightSideOriginalPosition = rightSide.localPosition;

        //    // Move the face parent to the desired vector, after that enable the buoyancy
        //    faceParent.DOMove(new Vector3(0, -0.35f, 0), easeLength).SetDelay(0.2f).SetEase(Ease.OutBack).OnComplete(StartGame);

        //    FaceSplit();

    }

    void SwitchPlayers() {
        IsPlayer1aCat = !IsPlayer1aCat;
        UpdateHud();
    }

    public IEnumerator StartGame() {

        //yield return new WaitForSeconds(TimeToWait);
        // timeManager.Initialize();

        //GameObject cat1 = Instantiate(catPrefab, new Vector3(-12.0f, 0.0f, -3.8f), transform.rotation);
        //cat1.transform.DOScale(3, 0);
        ////cat1.layer = LayerMask.NameToLayer("Cat");

        //GameObject cat2 = Instantiate(catPrefab, new Vector3(10.3f, 0.0f, -5.4f), transform.rotation);
        //cat2.transform.DOScale(3, 0);
        ////cat2.layer = LayerMask.NameToLayer("Cat");

        //GameObject cat3 = Instantiate(catPrefab, new Vector3(-20.4f, 0.0f, 2.9f), transform.rotation);
        //cat3.transform.DOScale(3, 0);
        ////cat3.layer = LayerMask.NameToLayer("Cat");

        //GameObject cat4 = Instantiate(catPrefab, new Vector3(-16.2f, 0.0f, 7.6f), transform.rotation);
        //cat4.transform.DOScale(3, 0);
        /////cat4.layer = LayerMask.NameToLayer("Cat");

        //DogController dog1 = Instantiate(dogPrefab, new Vector3(0.0f, 0.0f, 0.0f), transform.rotation);
        //dog1.transform.DOScale(1, 0);
        //dog1.health = 1;

        //GameObject dog2 = Instantiate(dogPrefab, new Vector3(3.8f, 0.0f, 8.9f), transform.rotation);
        //dog2.transform.DOScale(1, 0);

        //canUpdate = true;

        yield return new WaitForSeconds(TimeToWait);

        canUpdate = true;
        gamestate = GameState.Play;

        yield break;

    }

    public void StopGame() {
        canUpdate = false;
        //gamestate = GameState.Stop;
        //FaceJoin();
        //soundManager.StopMusicSource();

        if (NumDogsAlive == 0) {
            //Invoke("Win", soundManager.GetLength(ClipType.Finish));
            Invoke("CatsWin", 0);
        }
        if (NumCatsAlive == 0) {
            //Invoke("Lose", soundManager.GetLength(ClipType.Finish));
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

            bGameEnd = true;
            //gamestate = GameState.GameOver;
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

            bGameEnd = true;
            //gamestate = GameState.GameOver;
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

            if (NumDogsAlive == 0 || NumCatsAlive == 0) // or the cat has reached the target
            {
                StopGame();
            }
        }

        if (bGameEnd) {
            if (Input.GetButtonDown("A Button")) {
                SceneManager.LoadScene(0);
            }
        }

    }

}