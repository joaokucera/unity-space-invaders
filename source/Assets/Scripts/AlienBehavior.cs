using UnityEngine;
using System.Collections;

public class AlienBehavior : MonoBehaviour
{
    #region Fields

    public AudioClip clipKilled;
    public bool enableToShoot = true;

    private GameObject prefabAmmo;
    private Transform gunTransform;
    private GameObject prefabSparks;
    private AliensGroup scriptAliensGroup;
    private AnimationSprite sprite;
    private Material materialExplosion;
    private const float AmmoSpeed = 1.5f;
    private int counterShooter = 0;

    private GameObject sideBar;
    public bool enableStretch;

    private Color progressiveColor;
    private int speedToChangeColor = 2;

    #endregion

    #region Methods

    void Start()
    {
        enableToShoot = true;
        progressiveColor = renderer.material.color;

        foreach (Transform item in transform)
        {
            gunTransform = item;
            gunTransform.localPosition = new Vector3(0, -0.75f, 0);
        }

        scriptAliensGroup = transform.parent.GetComponent<AliensGroup>();
        sprite = GetComponent<AnimationSprite>();

        prefabAmmo = (GameObject)Resources.Load("Ammo");
        prefabSparks = (GameObject)Resources.Load("Little Sparks");
        materialExplosion = (Material)Resources.Load("explosion");

        sideBar = GameObject.Find("Side Bar");
    }

    void Update()
    {
        sprite.AnimateSprite(renderer, 2, 1, 0, 0, 2, 2);

        //int layer8 = LayerMask.NameToLayer("Player");
        //int layer9 = LayerMask.NameToLayer("Shield");
        //int layerMask8 = 1 << layer8 << layer9;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 100f))//, layerMask8))
        {
            if (hit.transform.tag == "Player")
            {
                counterShooter++;
                if (counterShooter == 20)
                {
                    counterShooter = 0;
                    Invoke("Shot", 0.5f);
                }
            }
        }
    }

    private void Shot()
    {
        GameObject go = (GameObject)Instantiate(prefabAmmo, gunTransform.position, gunTransform.rotation);
        go.renderer.material.color = renderer.material.color;
        //go.transform.localScale = new Vector3(0.1f, 0.4f, 0.1f);
        go.GetComponent<AmmoBehavior>().SetBehavior(AlienBehavior.AmmoSpeed, Side.DOWN);

        CancelInvoke("Shot");
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.name == "Wall Left")
        {
            scriptAliensGroup.ChangeSide(Side.RIGHT);
        }
        else if (hit.name == "Wall Right")
        {
            scriptAliensGroup.ChangeSide(Side.LEFT);
        }
        else if (hit.tag == "Slice")
        {
            Destroy(hit.gameObject);
        }
        else if (hit.tag == "Ammo" && hit.GetComponent<AmmoBehavior>().side == Side.UP)
        {
            if (sideBar.GetComponent<SideBar>().sounds.Kill)
            {
                audio.PlayOneShot(clipKilled);
            }
            //renderer.enabled = false;

            if (!sideBar.GetComponent<SideBar>().particles.FallOff)
            {
                if (enableStretch)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.5f, 0.1f, 3f), clipKilled.length);
                }
                else if (sideBar.GetComponent<SideBar>().particles.AmmoCollision)
                {
                    GameObject sparks = (GameObject)Instantiate(prefabSparks, transform.position, transform.rotation);
                    sparks.transform.parent = transform;
                }
                else
                {
                    materialExplosion.color = transform.renderer.material.color;
                    renderer.material = materialExplosion;
                }
            }
            //renderer.material.mainTextureScale = new Vector2(0f, 0f);
            //renderer.material.mainTextureOffset = new Vector2(0f, 0f);

            if (sideBar.GetComponent<SideBar>().colors.BackgroundColorEffect)
            {
                sideBar.GetComponent<SideBar>().RandomBackgroundColor();
            }

            if (sideBar.GetComponent<SideBar>().screenShake.Shake)
            {
                Camera.main.GetComponent<CameraShake>().Shake();
            }

            if (sideBar.GetComponent<SideBar>().particles.FallOff)
            {
                gunTransform.gameObject.SetActive(false);

                Destroy(collider);
                //collider.isTrigger = true;

                rigidbody.useGravity = true;
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

                Destroy(gameObject, clipKilled.length * 5);
            }
            else
            {
                Destroy(gameObject, clipKilled.length);
            }

            if (sideBar.GetComponent<SideBar>().particles.Dark)
            {
                renderer.material.color = Color.Lerp(renderer.material.color, Color.black, clipKilled.length * 2.5f);
            }
        }
    }

    #endregion
}
