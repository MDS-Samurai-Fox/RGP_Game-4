using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [HeaderAttribute("Audio Sources")]
    public AudioSource music;
    public AudioSource sfx;

    [HeaderAttribute("Audio Clips")]
    public AudioClip click;
    public AudioClip gridMove;
    public AudioClip select;
    public AudioClip catAttack1;
    public AudioClip catAttack2;
    public AudioClip catAttack3;
    public AudioClip catAttack4;

    public AudioClip missileShoot;
    public AudioClip turretShoot;
    public AudioClip plasmaShoot;
    public AudioClip teslaShoot;

    public void PlaySound(AudioClip _clip) {

        sfx.PlayOneShot(_clip);

    }

    public void PlayCatAttack() {

        int random = Random.Range(1, 4);

        if (random == 1) {
            sfx.PlayOneShot(catAttack1);
        } else if (random == 2) {
            sfx.PlayOneShot(catAttack2);
        } else if (random == 3) {
            sfx.PlayOneShot(catAttack3);
        } else if (random == 4) {
            sfx.PlayOneShot(catAttack4);
        }

    }

    public void StopMusic() {

        music.Stop();

    }

}