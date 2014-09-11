using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Side
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public class AliensGroup : MonoBehaviour
{
    #region Fields

    public List<AudioClip> listClipAliens;
    private int index = 0;
    private Side side = Side.LEFT;
    private float speed;
    private float[] incrementSpeed = { 5f, 20f, 35f, 50f };
    private float delay;
    private float[] decrementDelay = { 0.5f, 0.2f, 0.15f, 0.1f };
    private float fall;
    private float[] incrementFall = { 5f, 10f, 15f, 20f };
    public bool enabledMovementSound;

    #endregion

    #region Methods

    void Start()
    {
        speed = incrementSpeed[0];
        delay = decrementDelay[0];
        fall = incrementFall[0];
    }

    void Update()
    {
        if (transform.childCount > 0)
        {
            if (enabledMovementSound)
            {
                PlaySound();
            }
        }

        Invoke("AliensGroupMovement", 0.25f);

        if (transform.childCount <= 36 && transform.childCount > 7)
        {
            speed = incrementSpeed[0];
            delay = decrementDelay[0];
            fall = incrementFall[0];
        }
        else if (transform.childCount <= 7 && transform.childCount > 3)
        {
            speed = incrementSpeed[1];
            delay = decrementDelay[1];
            fall = incrementFall[1];
        }
        else if (transform.childCount <= 3 && transform.childCount > 1)
        {
            speed = incrementSpeed[2];
            delay = decrementDelay[2];
            fall = incrementFall[2];
        }
        else if (transform.childCount <= 1 && transform.childCount > 0)
        {
            speed = incrementSpeed[3];
            delay = decrementDelay[3];
            fall = incrementFall[3];
        }
    }

    private void PlaySound()
    {
        if (!audio.isPlaying)
        {
            index++;
            if (index >= listClipAliens.Count)
            {
                index = 0;
            }
            audio.clip = listClipAliens[index];
            audio.loop = false;
            audio.PlayDelayed(delay);
        }
    }

    private void AliensGroupMovement()
    {
        if (side == Side.LEFT)
        {
            transform.Translate(-Vector3.right * speed * Time.deltaTime);
        }
        else if (side == Side.RIGHT)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        CancelInvoke("AliensGroupMovement");
    }

    #endregion

    #region Properties

    public bool AllowAlienToShoot
    {
        get
        {
            int halfAliens = transform.childCount / 2;
            int random = Random.Range(0, halfAliens);
            return (random <= halfAliens);
        }
    }

    public void ChangeSide(Side pSide)
    {
        side = pSide;
        transform.Translate(-Vector3.up * fall * Time.deltaTime);
    }

    public void EnablingAlienStretch(bool alienStretch)
    {
        foreach (Transform item in transform)
        {
            item.GetComponent<AlienBehavior>().enableStretch = alienStretch;
        }
    }

    #endregion
}
