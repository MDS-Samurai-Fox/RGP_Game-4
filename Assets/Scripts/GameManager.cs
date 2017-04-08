using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

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
    // public int GameState;

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

    


    void Awake()
    {
        //soundManager = FindObjectOfType<SoundManager>();
    }

    // Use this for initialization
    void Start()
    {
        // easeLength = soundManager.GetLength(ClipType.Join);

        gameEndPanel.DOFade(0, 0);
        gameEndPanel.blocksRaycasts = false;
        resultPanel.DOFade(0, 0);

        roundNumberPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Round " + Round;
        //roundNumberPanel.DOFade(0, 0);
        roundNumberPanel.DOFade(1, 1).SetDelay(0);
        roundNumberPanel.DOFade(0, 1).SetDelay(1);

        UpdateHud();

        hudPanel.DOFade(0, 0);
        hudPanel.DOFade(1, 1).SetDelay(2).OnComplete(Initialize);

        //if (particles != null)
        //    particles.Stop();

    }

    void UpdateHud()
    {
        CatArray = FindObjectsOfType<CatController>();
        DogArray = FindObjectsOfType<DogController>();
        TurretArray = FindObjectsOfType<TurretController>();
        LaserTurretArray = FindObjectsOfType<LaserTurretController>();
        NumDogsAlive = DogArray.Length - TurretArray.Length - LaserTurretArray.Length;
        NumCatsAlive = CatArray.Length;

        hudPanel.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Number of Cats Alive : " + NumCatsAlive;
        hudPanel.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Number of Dogs Alive : " + NumDogsAlive;
        hudPanel.GetComponentsInChildren<TextMeshProUGUI>()[2].text = "Player 1 Score : " + Player1Score;
        hudPanel.GetComponentsInChildren<TextMeshProUGUI>()[3].text = "Player 2 Score : " + Player2Score;
    }

    void Initialize()
    {
        NumCatsAlive = TotalNumCats;
        NumDogsAlive = TotalNumDogs;
        
        StartCoroutine(StartGame());

        //easeLength = soundManager.GetLength(ClipType.Join);

        //leftSideOriginalPosition = leftSide.localPosition;
        //    middleSideOriginalPosition = middleSide.localPosition;
        //    rightSideOriginalPosition = rightSide.localPosition;

        //    // Move the face parent to the desired vector, after that enable the buoyancy
        //    faceParent.DOMove(new Vector3(0, -0.35f, 0), easeLength).SetDelay(0.2f).SetEase(Ease.OutBack).OnComplete(StartGame);

        //    FaceSplit();

    }

    public IEnumerator StartGame()
   // void StartGame()
    {
        yield return new WaitForSeconds(TimeToWait);
        // timeManager.Initialize();

        GameObject cat1 = Instantiate(catPrefab, new Vector3(-12.0f, 0.0f, -3.8f), transform.rotation);
        cat1.transform.DOScale(3, 0);
        //cat1.layer = LayerMask.NameToLayer("Cat");

        GameObject cat2 = Instantiate(catPrefab, new Vector3(10.3f, 0.0f, -5.4f), transform.rotation);
        cat2.transform.DOScale(3, 0);
        //cat2.layer = LayerMask.NameToLayer("Cat");

        GameObject cat3 = Instantiate(catPrefab, new Vector3(-20.4f, 0.0f, 2.9f), transform.rotation);
        cat3.transform.DOScale(3, 0);
        //cat3.layer = LayerMask.NameToLayer("Cat");

        GameObject cat4 = Instantiate(catPrefab, new Vector3(-16.2f, 0.0f, 7.6f), transform.rotation);
        cat4.transform.DOScale(3, 0);
        ///cat4.layer = LayerMask.NameToLayer("Cat");

        DogController dog1 = Instantiate(dogPrefab, new Vector3(0.0f, 0.0f, 0.0f), transform.rotation);
        dog1.transform.DOScale(1, 0);
        dog1.health = 1;

        //GameObject dog2 = Instantiate(dogPrefab, new Vector3(3.8f, 0.0f, 8.9f), transform.rotation);
        //dog2.transform.DOScale(1, 0);

        canUpdate = true;

    }

    public void StopGame()
    {
        canUpdate = false;
        //FaceJoin();
        //soundManager.StopMusicSource();

        if (NumDogsAlive == 0)
        {
            //Invoke("Win", soundManager.GetLength(ClipType.Finish));
            Invoke("Win", 0);
        }
        if (NumCatsAlive == 0)
        {
            //Invoke("Lose", soundManager.GetLength(ClipType.Finish));
            Invoke("Lose", 0);
        }

    }

    void Win()
    {
        Player1Score++;
        Round++;
        UpdateHud();

        if (Player1Score >= 2)
        {
            gameEndPanel.GetComponentInChildren<TextMeshProUGUI>().text = "CATS WON OVERALL";
            gameEndPanel.DOFade(1, 1).OnComplete(EnableBlockRaycasts);
            bGameEnd = true;
        }
        else
        {
            resultPanel.GetComponentInChildren<TextMeshProUGUI>().text = "CATS WON THIS ROUND \n Cats " + Player1Score + " : Dogs " + Player2Score;
            resultPanel.DOFade(0, 0);
            resultPanel.DOFade(1, 1).SetDelay(1);
            resultPanel.DOFade(0, 1).SetDelay(3);

            roundNumberPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Round " + Round;
            roundNumberPanel.DOFade(0, 0);
            roundNumberPanel.DOFade(1, 1).SetDelay(5);
            roundNumberPanel.DOFade(0, 1).SetDelay(7).OnComplete(Initialize);
        }
    }

    void Lose()
    {
        Player2Score++;
        Round++;
        UpdateHud();

        if (Player2Score > 2)
        {
            gameEndPanel.GetComponentInChildren<TextMeshProUGUI>().text = "DOGS WON OVERALL";
            gameEndPanel.DOFade(1, 1).OnComplete(EnableBlockRaycasts);

            bGameEnd = true;
        }
        else
        {
            resultPanel.GetComponentInChildren<TextMeshProUGUI>().text = "DOGS WON THIS ROUND \n Cats " + Player1Score + " : Dogs " + Player2Score;
            resultPanel.DOFade(0, 0);
            resultPanel.DOFade(1, 1).SetDelay(1);
            resultPanel.DOFade(0, 1).SetDelay(3);

            roundNumberPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Round " + Round;
            roundNumberPanel.DOFade(0, 0);
            roundNumberPanel.DOFade(1, 1).SetDelay(5);
            roundNumberPanel.DOFade(0, 1).SetDelay(7).OnComplete(Initialize);
        }

    }

    void EnableBlockRaycasts()
    {
        gameEndPanel.blocksRaycasts = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canUpdate)
        {
            UpdateHud();

            if (NumDogsAlive == 0 || NumCatsAlive == 0)
            {
                StopGame();
            }
        }
        
        if (bGameEnd)
        {
            if (Input.GetButtonDown("A Button"))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

}
