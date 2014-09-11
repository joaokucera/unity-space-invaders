using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    #region Fields

    public AudioClip clipKilled;
    public GUIText lifesGUIText;
    public AliensGroup scriptAliensGroup;
    public Renderer graphicsRenderer;

    private float speed = 2.5f;
    //private float gravity = 20f;
    //private CharacterController controller;
    private Vector3 velocity = Vector3.zero;
    private bool ready = false;
    private int lifes = 3;

    public bool enableShipStretch;
    private float stretchPower = 1f;
    private bool stretchUp = true;
    public bool enableKillSound;

    #endregion

    #region Methods

    void Awake()
    {
        scriptAliensGroup.enabled = false;
        GetComponent<PlayerShooter>().enabled = false;
    }

    void Start()
    {
        //controller = GetComponent<CharacterController>();
        transform.position = new Vector3(-4f, -3.25f, 0);
        graphicsRenderer.material.mainTextureOffset = new Vector2(0f, 0f);
    }

    void Update()
    {
        lifesGUIText.text = lifes.ToString();
        if (lifes <= 0)
        {
            ready = false;
            //AutoFade.LoadLevel("Level", 1.5f, 1.5f, Color.black);
        }

        if (ready)
        {
            velocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            velocity *= speed;
            //velocity.y -= gravity * Time.deltaTime;

            if (enableShipStretch)
            {
                if (velocity.x > 0.1f || velocity.x < -0.1f)
                {
                    if (stretchUp)
                    {
                        stretchPower += Time.deltaTime;
                        if (stretchPower > 1.2f)
                        {
                            stretchUp = false;
                        }
                    }
                    else
                    {
                        stretchPower -= Time.deltaTime;
                        if (stretchPower < 0.8f)
                        {
                            stretchUp = true;
                        }
                    }
                    transform.localScale = new Vector3(stretchPower, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), 1f);
                }
            }

            //controller.Move(velocity * Time.deltaTime);
            transform.Translate(velocity * Time.deltaTime);
        }
        else
        {
            Invoke("Ready", 3f);
            Invoke("Blink", 0.25f);
        }
    }

    private void Blink()
    {
        graphicsRenderer.enabled = !graphicsRenderer.enabled;
        CancelInvoke("Blink");
    }

    private void Wait()
    {
        lifes--;
        lifesGUIText.enabled = true;

        Invoke("Blink", 0.25f);
        CancelInvoke("Wait");
    }

    private void Ready()
    {
        if (lifes > 0)
        {
            lifesGUIText.enabled = false;
            graphicsRenderer.enabled = true;
            collider.enabled = true;
            ready = true;

            GetComponent<PlayerShooter>().enabled = true;
            graphicsRenderer.material.mainTextureOffset = new Vector2(0f, 0f);

            scriptAliensGroup.enabled = true;
            foreach (Transform item in scriptAliensGroup.transform)
            {
                item.GetComponent<AlienBehavior>().enableToShoot = true;
                item.GetComponent<AlienBehavior>().enabled = true;
            }
        }

        CancelInvoke("Blink");
        CancelInvoke("Ready");
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Ammo" && hit.GetComponent<AmmoBehavior>().side == Side.DOWN)
        {
            scriptAliensGroup.enabled = false;
            foreach (Transform item in scriptAliensGroup.transform)
            {
                item.GetComponent<AlienBehavior>().enableToShoot = false;
                item.GetComponent<AlienBehavior>().enabled = false;
            }

            GameObject[] ammos = GameObject.FindGameObjectsWithTag("Ammo");
            foreach (GameObject item in ammos)
            {
                Destroy(item);
            }

            if (enableKillSound)
            {
                audio.PlayOneShot(clipKilled);
            }
            collider.enabled = false;
            graphicsRenderer.material.mainTextureOffset = new Vector2(0.5f, 0f);

            ready = false;
            Invoke("Wait", 2f);
            Invoke("Ready", 4f);
        }
    }

    #endregion
}
