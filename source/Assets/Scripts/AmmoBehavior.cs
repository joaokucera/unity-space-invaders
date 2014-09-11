using UnityEngine;
using System.Collections;

public class AmmoBehavior : MonoBehaviour
{
    #region Fields

    public Side side = Side.UP;
    //private float speed = 7.5f;
    //private float startSpeed;
    private float speed;
    private PlayerShooter scriptPlayerShoot;

    //private float stretchPower = 0.375f;
    //private bool stretchUp = true;
    public bool enableAmmoStretch;
    private GameObject sideBar;
    private GameObject prefabSmoke;

    #endregion

    #region Methods

    void Start()
    {
        //startSpeed = speed;

        scriptPlayerShoot = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShooter>();
        InvokeRepeating("ShowAndHide", 0.1f, 0.1f);

        sideBar = GameObject.Find("Side Bar");
        prefabSmoke = (GameObject)Resources.Load("SparkleParticles");
    }

    void Update()
    {
        if (side == Side.UP)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else if (side == Side.DOWN)
        {
            transform.Translate(-Vector3.up * speed * Time.deltaTime);
        }

        //if (enableAmmoStretch)
        //{
        //    if (stretchUp)
        //    {
        //        stretchPower += Time.deltaTime;
        //        if (stretchPower > 0.5f)
        //        {
        //            stretchUp = false;
        //        }
        //    }
        //    else
        //    {
        //        stretchPower -= Time.deltaTime;
        //        if (stretchPower < 0.05f)
        //        {
        //            stretchUp = true;
        //        }
        //    }
        //}
        //else
        //{
        //    speed = startSpeed;
        //}

        //transform.localScale = new Vector3(transform.localScale.x, stretchPower, transform.localScale.z);
    }

    public void SetBehavior(float pSpeed, Side pSide)
    {
        speed = pSpeed;
        side = pSide;
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Alien" && side == Side.UP)
        {
            scriptPlayerShoot.AddPoints(hit.name);
            Death();
        }
        else if (hit.tag == "Player" && side == Side.DOWN)
        {
            Death();
        }
        else if (hit.tag == "Shield")
        {
            Destroy(hit.gameObject);
            Death();
        }
        else if (hit.tag == "Kill Box")
        {
            Death();
        }
        else if (hit.tag == "Rock")
        {
            if (sideBar.GetComponent<SideBar>().particles.AmmoSmoke)
            {
                GameObject smoke = (GameObject)Instantiate(prefabSmoke, transform.position, transform.rotation);
                Destroy(smoke, 5f);
            }
            //    transformRock = hit.transform;
            //    hit.transform.localScale = Vector3.Lerp(hit.transform.localScale, new Vector3(1f, 0.5f, 1f), 0.1f);
            //    //Invoke("BackScale", 0.1f);
            //    transformRock.localScale = Vector3.Lerp(transformRock.localScale, new Vector3(1f, 1f, 1f), 0.1f);
        }
    }

    //private Transform transformRock;
    //private void BackScale()
    //{
    //    transformRock.localScale = Vector3.Lerp(transformRock.localScale, new Vector3(1f, 1f, 1f), 0.1f);
    //}

    void ShowAndHide()
    {
        renderer.enabled = !renderer.enabled;
    }

    public void Death()
    {
        scriptPlayerShoot.canShoot = true;
        Destroy(gameObject);
    }

    #endregion
}
