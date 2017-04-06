using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {
	
	// [Header("Animations")]

    private Animator animator;
	
	

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
		
        animator = GetComponent<Animator> ();
		
    }

    // Use this for initialization
    void Start() {
		
		// animator.Play("Idle");

    }

    // Update is called once per frame
    void Update() {
		
		

    }
	
	public void PlayAnimation() {
		
		
		
	}
	
	private IEnumerator WaitForAnimation(Animation a) {
		
		do {
			yield return null;
		} while (a.isPlaying);
		
	}
	
}