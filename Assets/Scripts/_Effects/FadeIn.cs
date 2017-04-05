
using DG.Tweening;
using UnityEngine;

public class FadeIn : MonoBehaviour
{

    public float waitTime = 0f;
    private float fadeDuration = 1f;
    private Vector3 scale;

    // Use this for initialization
    void Start()
    {

        scale = this.transform.localScale;
        this.transform.DOScale(Vector3.zero, 0);
        Invoke("Initialize", waitTime);

    }

    void Initialize()
    {

        this.transform.DOScale(scale, fadeDuration);

    }

}
