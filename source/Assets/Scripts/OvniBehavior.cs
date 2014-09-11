using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OvniBehavior : MonoBehaviour
{
    #region Fields

    public List<AudioClip> listClipOvni;
    public AudioClip clipKilled;

    private List<GUIText> listPointsGuiText;
    private Material materialExplosion;
    private float speed = 1f;
    private int index = 0;
    private float delay = 0.5f;

    private GameObject sideBar;
    private GameObject prefabSparks;

    #endregion

    #region Methods

    void Start()
    {
        listPointsGuiText = new List<GUIText>();
        listPointsGuiText.Add(GameObject.FindGameObjectWithTag("GUI Points").GetComponent<GUIText>());
        listPointsGuiText.Add(GameObject.FindGameObjectWithTag("GUI Records").GetComponent<GUIText>());

        prefabSparks = (GameObject)Resources.Load("Little Sparks");
        materialExplosion = (Material)Resources.Load("explosion");

        sideBar = GameObject.Find("Side Bar");
    }

    void Update()
    {
        if (sideBar.GetComponent<SideBar>().sounds.Effects)
        {
            PlaySound();
        }

        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerExit(Collider hit)
    {
        if (hit.name == "Wall Left")
        {
            Death(0);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Ammo" && hit.GetComponent<AmmoBehavior>().side == Side.UP)
        {
            if (sideBar.GetComponent<SideBar>().sounds.Kill)
            {
                audio.PlayOneShot(clipKilled);
            }

            if (sideBar.GetComponent<SideBar>().particles.AmmoCollision)
            {
                GameObject sparks = (GameObject)Instantiate(prefabSparks, transform.position, transform.rotation);
                sparks.transform.parent = transform;
            }
            else
            {
                renderer.material = materialExplosion;
            }

            if (sideBar.GetComponent<SideBar>().colors.BackgroundColorEffect)
            {
                sideBar.GetComponent<SideBar>().RandomBackgroundColor();
            }

            Death(clipKilled.length);
        }
        else if (hit.name == "Wall Right")
        {
            foreach (GUIText item in listPointsGuiText)
            {
                item.enabled = false;
            }
        }
    }

    public void Death(float delay)
    {
        foreach (GUIText item in listPointsGuiText)
        {
            item.enabled = true;
        }
        Destroy(gameObject, delay);
    }

    private void PlaySound()
    {
        if (!audio.isPlaying)
        {
            index++;
            if (index >= listClipOvni.Count)
            {
                index = 0;
            }
            audio.clip = listClipOvni[index];
            audio.loop = false;
            audio.PlayDelayed(delay);
        }
    }

    #endregion
}