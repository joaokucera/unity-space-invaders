using UnityEngine;
using System.Collections;

public class PlayerShooter : MonoBehaviour
{
    #region Fields

    public GUIText pointsGUIText;
    public AudioClip clipShoot;
    public GameObject prefabAmmo;
    public bool canShoot = true;
    public Transform gunTransform;
    public int points = 0;

    private const float AmmoSpeed = 10f;

    public bool enableAmmoStretch;
    public bool enableAmmoSound { get; set; }


    #endregion

    #region Update

    void Start()
    {
        gunTransform.localPosition = new Vector3(0, 0.75f, 0);
        prefabAmmo = (GameObject)Resources.Load("Ammo");
    }

    void Update()
    {
        if (canShoot && Input.GetButtonDown("Jump"))
        {
            canShoot = false;
            if (enableAmmoSound)
            {
                audio.PlayOneShot(clipShoot);
            }

            GameObject go = (GameObject)Instantiate(prefabAmmo, gunTransform.position, gunTransform.rotation);
            go.renderer.material.color = GetComponent<PlayerMovement>().graphicsRenderer.material.color;
            //go.transform.localScale = new Vector3(0.1f, 0.4f, 0.1f);
            go.GetComponent<AmmoBehavior>().SetBehavior(PlayerShooter.AmmoSpeed, Side.UP);
            go.GetComponent<AmmoBehavior>().enableAmmoStretch = enableAmmoStretch;
        }
    }

    public void AddPoints(string alienName)
    {
        if (alienName.Contains("1"))
        {
            points += 30;
        }
        else if (alienName.Contains("2"))
        {
            points += 25;
        }
        else if (alienName.Contains("3"))
        {
            points += 20;
        }
        else if (alienName.Contains("4"))
        {
            points += 15;
        }
        else if (alienName.Contains("5"))
        {
            points += 10;
        }
        else if (alienName.Contains("6"))
        {
            points += 5;
        }
        else if (alienName.Contains("Ovni"))
        {
            points += 100;
        }

        // 180 + 150 + 120 + 90 + 60 + 30 = 630
        pointsGUIText.text = points.ToString("0000");
    }

    #endregion
}
