using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    // Classes
    //[HideInInspector]
    // public SoundManager soundManager;
    //[HideInInspector]
    //public FaceChecker faceChecker;
    //private Buoyancy buoyancy;
    // private MoveAstro moveAstronaut;

    [HideInInspector]
    public bool canUpdate = false;

    [HeaderAttribute("Intro Animation")]
    [SerializeField]
    private Ease easeType = Ease.InSine;
    private float easeLength = 0;

    [SpaceAttribute]
    //public Transform faceParent;
    //public Transform leftSide;
    //public Transform middleSide;
    //public Transform rightSide;
    //public ParticleSystem particles;
    //public Transform faceToMatch;
    public CanvasGroup gameEndPanel;
    public CanvasGroup roundNumberPanel;
    public CanvasGroup resultPanel;
    public CanvasGroup hudPanel;

    //[SpaceAttribute]
    //public Vector3 leftSideSplitPosition;
    //public Vector3 middleSideSplitPosition;
    //public Vector3 rightSideSplitPosition;

    //private Vector3 leftSideOriginalPosition;
    //private Vector3 middleSideOriginalPosition;
    //private Vector3 rightSideOriginalPosition;

    // Obsolete
    //private bool areSidesJoined = true;

    public int Round = 1;
    // public int GameState;
    public int Player1Score = 0;
    public int Player2Score = 0;
    public int NumCatsAlive = 3;
    public int NumDogsAlive = 3;
    public int TotalNumCats = 3;
    public int TotalNumDogs = 3;

    void Awake()
    {

        //timeManager = GetComponent<TimeManager>();
        //soundManager = FindObjectOfType<SoundManager>();
        //buoyancy = FindObjectOfType<Buoyancy>();
        //faceChecker = GetComponent<FaceChecker>();

    }

    // Use this for initialization
    void Start()
    {

        // easeLength = soundManager.GetLength(ClipType.Join);

        gameEndPanel.DOFade(0, 0);
        gameEndPanel.blocksRaycasts = false;
        resultPanel.DOFade(0, 0);

        //roundNumberPanel.GetComponentInChildren<Text>().text = "Round " + Round;
        roundNumberPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Round " + Round;
        //roundNumberPanel.DOFade(0, 0);
        roundNumberPanel.DOFade(1, 1).SetDelay(0);
        roundNumberPanel.DOFade(0, 1).SetDelay(1);

        //hudPanel.GetComponentsInChildren<Text>()[0].text = "Number of Cats Alive : " + NumCatsAlive;  
        //hudPanel.GetComponentsInChildren<Text>()[1].text = "Number of Dogs Alive : " + NumDogsAlive;
        //hudPanel.GetComponentsInChildren<Text>()[2].text = "Player 1 Score : " + Player1Score;
        //hudPanel.GetComponentsInChildren<Text>()[3].text = "Player 2 Score : " + Player2Score;

        hudPanel.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Number of Cats Alive : " + NumCatsAlive;
        hudPanel.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Number of Dogs Alive : " + NumDogsAlive;
        hudPanel.GetComponentsInChildren<TextMeshProUGUI>()[2].text = "Player 1 Score : " + Player1Score;
        hudPanel.GetComponentsInChildren<TextMeshProUGUI>()[3].text = "Player 2 Score : " + Player2Score;


        hudPanel.DOFade(0, 0);
        hudPanel.DOFade(1, 1).SetDelay(2);


        //if (particles != null)
        //    particles.Stop();

        //faceToMatch.DOScale(1, easeLength);
        //faceToMatch.DOScale(0.25f, 1).SetDelay(easeLength + 2);
        //faceToMatch.DOMove(Vector3.zero, 1).SetDelay(easeLength + 2).From().OnComplete(Initialize);

    }

    void Initialize()
    {
        NumCatsAlive = TotalNumCats;
        NumDogsAlive = TotalNumDogs;


        //easeLength = soundManager.GetLength(ClipType.Join);

        //leftSideOriginalPosition = leftSide.localPosition;
        //    middleSideOriginalPosition = middleSide.localPosition;
        //    rightSideOriginalPosition = rightSide.localPosition;

        //    // Move the face parent to the desired vector, after that enable the buoyancy
        //    faceParent.DOMove(new Vector3(0, -0.35f, 0), easeLength).SetDelay(0.2f).SetEase(Ease.OutBack).OnComplete(StartGame);

        //    FaceSplit();

    }

    void StartGame()
    {

        // timeManager.Initialize();
        canUpdate = true;

        // if (buoyancy != null)
        //     buoyancy.Float();

    }

    public void StopGame()
    {

        canUpdate = false;
        //FaceJoin();
        //soundManager.StopMusicSource();
        //soundManager.StopJetpackSource();

        //if (faceChecker.HasMatchedFace() == true)
        //{
        //    Invoke("Win", soundManager.GetLength(ClipType.Finish));
        //}
        //else
        //{
        //    Invoke("Lose", soundManager.GetLength(ClipType.Finish));
        //}

        if (NumCatsAlive == 0)
        {
            Invoke("Win", 0);
        }
        if (NumDogsAlive == 0)
        {
            Invoke("Lose", 0);
        }

    }

    public void ToggleJoin()
    {

        //if (areSidesJoined)
        //{

        //    FaceSplit();

        //}
        //else
        //{

        //    FaceJoin();

        //}

        //areSidesJoined = !areSidesJoined;

    }

    void FaceJoin()
    {

        //easeLength = soundManager.GetLength(ClipType.Join);
        //soundManager.Play(ClipType.Finish);
        PlayAnimationBlastStart();

        //leftSide.DOLocalMove(leftSideOriginalPosition, easeLength).SetEase(easeType);
        //middleSide.DOLocalMove(middleSideOriginalPosition, easeLength).SetEase(easeType);
        //rightSide.DOLocalMove(rightSideOriginalPosition, easeLength).SetEase(easeType).OnComplete(PlayAnimationBlastEnd);

    }

    void FaceSplit()
    {

        //easeLength = soundManager.GetLength(ClipType.Split);
        //soundManager.Play(ClipType.Split);
        PlayAnimationBlastStart();

        //leftSide.DOLocalMove(leftSideSplitPosition, easeLength).SetEase(easeType);
        //middleSide.DOLocalMove(middleSideSplitPosition, easeLength).SetEase(easeType);
        //rightSide.DOLocalMove(rightSideSplitPosition, easeLength).SetEase(easeType).OnComplete(PlayAnimationBlastEnd);

    }

    void PlayAnimationBlastStart()
    {
        //soundManager.Play(ClipType.BlastStart);
    }

    void PlayAnimationBlastEnd()
    {
        //soundManager.Play(ClipType.BlastEnd);
    }

    void Win()
    {
        Player1Score++;
        Round++;

        if (Player1Score >= 2)
        {
            //gameEndPanel.GetComponentInChildren<Text>().text = "CATS WON OVERALL";
            gameEndPanel.GetComponentInChildren<TextMeshProUGUI>().text = "CATS WON OVERALL";
            gameEndPanel.DOFade(1, 1).OnComplete(EnableBlockRaycasts).OnComplete(Initialize);
        }
        else
        {
            //resultPanel.GetComponentInChildren<Text>().text = "CATS WON THIS ROUND \n Cats " + Player1Score + " : Dogs " + Player2Score;
            resultPanel.GetComponentInChildren<TextMeshProUGUI>().text = "CATS WON THIS ROUND \n Cats " + Player1Score + " : Dogs " + Player2Score;
            resultPanel.DOFade(0, 0);
            resultPanel.DOFade(1, 1).SetDelay(1);
            resultPanel.DOFade(0, 1).SetDelay(3);

            //roundNumberPanel.GetComponentInChildren<Text>().text = "Round " + Round;
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

        if (Player2Score > 2)
        {
            //gameEndPanel.GetComponentInChildren<Text>().text = "DOGS WON OVERALL";
            gameEndPanel.GetComponentInChildren<TextMeshProUGUI>().text = "DOGS WON OVERALL";
            gameEndPanel.DOFade(1, 1).OnComplete(EnableBlockRaycasts);
        }
        else
        {
           // resultPanel.GetComponentInChildren<Text>().text = "DOGS WON THIS ROUND \n Cats " + Player1Score + " : Dogs " + Player2Score;
            resultPanel.GetComponentInChildren<TextMeshProUGUI>().text = "DOGS WON THIS ROUND \n Cats " + Player1Score + " : Dogs " + Player2Score;
            resultPanel.DOFade(0, 0);
            resultPanel.DOFade(1, 1).SetDelay(1);
            resultPanel.DOFade(0, 1).SetDelay(3);

            //roundNumberPanel.GetComponentInChildren<Text>().text = "Round " + Round;
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

    }

}
