using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{

    private AudioSource audioSource;

    private static int sceneToChange;

    private int menuSelection = -1;

    public bool canUpdate = true;


    void Awake()
    {

        audioSource = GetComponent<AudioSource>();

    }

    // Use this for initialization
    void Start()
    {

        if (SceneManager.GetActiveScene().name == "Test - Charmaine Menu")
        {
            CanvasGroup creditsPanel = GameObject.Find("Credits Panel").GetComponent<CanvasGroup>();
            creditsPanel.alpha = 0;
            creditsPanel.blocksRaycasts = false;
        }

    }

    /// Loads the scene in an ordered manner
    public void LoadScene(int _scene)
    {

        sceneToChange = _scene;

        if (audioSource != null)
        {

            audioSource.PlayOneShot(audioSource.clip);
            //Invoke("ChangeScene", audioSource.clip.length / 2);
            //uncomment when audio is added to the game
            Invoke("ChangeScene", 0);
        }
        else
        {

            Invoke("ChangeScene", 0);

        }

    }

    void ChangeScene()
    {
        SceneManager.LoadScene(sceneToChange);
    }

    public void Exit()
    {

        audioSource.PlayOneShot(audioSource.clip);

        //Invoke("Quit", audioSource.clip.length - 0.2f);
        //uncomment when audio is added to the game
        Invoke("Quit", 0);
    }

    void Quit()
    {
        Application.Quit();
    }

    public void SetPanel(CanvasGroup _panel)
    {

        EventSystem.current.SetSelectedGameObject(null);

        audioSource.PlayOneShot(audioSource.clip);

        _panel.DOFade(1, 0.35f);
        _panel.blocksRaycasts = true;

        foreach (CanvasGroup cg in GameObject.FindObjectsOfType<CanvasGroup>())
        {

            if (cg != _panel)
            {

                cg.alpha = 0;
                cg.blocksRaycasts = true;

            }

        }

    }

    private void Update()
    {
        if (canUpdate)
        {


            CanvasGroup menuPanel = GameObject.Find("Menu Panel").GetComponent<CanvasGroup>();
            CanvasGroup creditsPanel = GameObject.Find("Credits Panel").GetComponent<CanvasGroup>();
            menuPanel.GetComponentsInChildren<Image>()[0].color = Color.grey;
            menuPanel.GetComponentsInChildren<Image>()[1].color = Color.grey;
            menuPanel.GetComponentsInChildren<Image>()[2].color = Color.grey;

            if (menuSelection == -1)
            {
                menuPanel.GetComponentsInChildren<Image>()[0].color = Color.white;
                menuPanel.GetComponentsInChildren<Image>()[1].color = Color.white;
                menuPanel.GetComponentsInChildren<Image>()[2].color = Color.white;
            }
            if (menuSelection == 0)
            {
                menuPanel.GetComponentsInChildren<Image>()[menuSelection].color = Color.white;

            }
            else if (menuSelection == 1)
            {
                menuPanel.GetComponentsInChildren<Image>()[menuSelection].color = Color.white;

            }
            else if (menuSelection == 2)
            {
                menuPanel.GetComponentsInChildren<Image>()[menuSelection].color = Color.white;
            }
            else if (menuSelection == 3)
            {

            }


            if (Input.GetButtonDown("A Button"))
            {
                if (menuSelection == 0)
                {
                    LoadScene(1);

                }
                else if (menuSelection == 1)
                {
                    menuSelection = 3;
                    SetPanel(creditsPanel);

                }
                else if (menuSelection == 2)
                {
                    Exit();
                }
                else if (menuSelection == 3)
                {
                    SetPanel(menuPanel);
                    menuSelection = 0;
                }
            }
            if (Input.GetButtonDown("B Button"))
            {
                menuSelection++;
                if (menuSelection > 2)
                {
                    menuSelection = 0;
                }
                Debug.Log("DPad UP");
            }



            if (Input.GetAxis("DPadVertical") > 0)
            {
                Debug.Log("DPad UP");

                menuSelection--;

                if (menuSelection < 0)
                {
                    menuSelection = 0;
                }
            }
            if (Input.GetAxis("DPadVertical") < 0)
            {
                //CanvasGroup creditsPanel = GameObject.Find("Credits Panel").GetComponent<CanvasGroup>();
                //SetPanel(creditsPanel);
                Debug.Log(menuSelection);

                menuSelection++;

                if (menuSelection > 2)
                {
                    menuSelection = 2;
                }

            }
            // AddDelay (5.0f);
        }

    }


    IEnumerator AddDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
