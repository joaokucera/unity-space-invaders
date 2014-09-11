using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;

public enum ItemMenu
{
    Colors,
    Tweening,
    StretchAndSqueezem,
    Sounds,
    Particles,
    ScreenShake,
    Personality,
    FinishHim,
    Other
}

public class SideBar : MonoBehaviour
{
    public bool showSideBar = false;
    public ItemMenu itemMenu = ItemMenu.Colors;

    public GUISkin skinSideBar;
    public Texture2D textureSideBar;

    public Colors colors = new Colors();
    public Tweening tweening = new Tweening();
    public StretchAndSqueeze stretchAndSqueeze = new StretchAndSqueeze();
    public Sounds sounds = new Sounds();
    public Particles particles = new Particles();
    public ScreenShake screenShake = new ScreenShake();
    public Personality personality = new Personality();
    public FinishHim finishHim = new FinishHim();
    public Other other = new Other();

    // 25,0,25,255
    // 180,0.80,255
    // 255,125,0,255
    public Color[] kindOfColors;
    public Color[] groupOfBackgroundColors;

    // Tweening
    public GameObject aliensGroup;
    public GameObject shieldGroup;
    public GameObject ship;
    private float timer = 1f;

    public List<Vector3> aliensOriginPosition;

    public float shake_decay = 0.001f;
    public float shake_intensity;
    public float coef_shake_intensity = 1f;

    //public AudioClip music;
    private bool controlTweeningY = false;
    private GameObject[] listObjectsRock;

    public GameObject shipEyes;

    void Start()
    {
        colors.ScreenColors = System.Convert.ToBoolean(PlayerPrefs.GetInt("SCREEN_COLORS", 0));

        tweening.Enabled = System.Convert.ToBoolean(PlayerPrefs.GetInt("TWEENING ENABLE", 0));
        tweening.Bounce_Y = System.Convert.ToBoolean(PlayerPrefs.GetInt("TWEENING BOUNCE Y", 0));
        tweening.Rotation_Smooth = System.Convert.ToBoolean(PlayerPrefs.GetInt("TWEENING ROTATION SMOOTH", 0));
        tweening.Time_Scale = System.Convert.ToBoolean(PlayerPrefs.GetInt("TWEENING TIME SCALE", 0));
        tweening.Shake = System.Convert.ToBoolean(PlayerPrefs.GetInt("TWEENING SHAKE", 0));

        if (tweening.Enabled)
        {
            aliensGroup.transform.position = new Vector3(1.25f, 10.25f, 0f);
            shieldGroup.transform.position = new Vector3(0, 8f, 0f);
        }
        else
        {
            aliensGroup.transform.position = new Vector3(1.25f, 0.5f, 0f);
            shieldGroup.transform.position = new Vector3(0, -1.75f, 0f);
        }

        // STRETCH
        //ChangeShipStretch();
        //ChangeAlienStretch();
        listObjectsRock = GameObject.FindGameObjectsWithTag("Rock");
        //ChangeWallStretch();

        // COLORS
        ChangeColors();

        // TWEENING DELAY
        if (tweening.Shake)
        {
            shake_intensity = coef_shake_intensity;

            foreach (Transform item in aliensGroup.transform)
            {
                aliensOriginPosition.Add(item.localPosition);
            }
        }

        // PERSONALITY
        shipEyes.SetActive(personality.ShipFace);
    }

    private void ChangeShipStretch()
    {
        ship.GetComponent<PlayerMovement>().enableShipStretch = stretchAndSqueeze.ShipStretch;
    }

    //private void ChangeAmmoStretch()
    //{
    //    ship.GetComponent<PlayerShooter>().enableAmmoStretch = stretchAndSqueeze.AmmoStretch;
    //}

    private void ChangeAlienStretch()
    {
        aliensGroup.GetComponent<AliensGroup>().EnablingAlienStretch(stretchAndSqueeze.AlienStretch);
    }

    private void TweeningDisable()
    {
        Time.timeScale = 1f;
        Debug.Log(Time.timeScale);

        tweening.Enabled = false;
        tweening.Bounce_Y = false;
        tweening.Rotation_Smooth = false;
        tweening.Time_Scale = false;
        tweening.Shake = false;

        aliensGroup.transform.position = new Vector3(1.25f, 0.5f, 0f);
        shieldGroup.transform.position = new Vector3(0, -1.75f, 0f);
        
        CancelInvoke("TweeningDisable");
    }

    void Update()
    {
        if (tweening.Enabled)
        {
            float timer = GetTimeTweeningScale();

            if (tweening.Bounce_Y && shieldGroup.transform.position.y < -1.5f)
            {
                controlTweeningY = true;
            }

            if (controlTweeningY)
            {
                aliensGroup.transform.position = Vector3.Lerp(aliensGroup.transform.position, new Vector3(1.25f, 1.25f, 0f), Time.deltaTime * timer * 5f);
                shieldGroup.transform.position = Vector3.Lerp(shieldGroup.transform.position, new Vector3(0, -1f, 0f), Time.deltaTime * timer * 5f);

                if (shieldGroup.transform.position.y > -1.1f)
                {
                    controlTweeningY = false;
                    tweening.Bounce_Y = false;
                }
            }
            else
            {
                if (tweening.Rotation_Smooth)
                {
                    aliensGroup.transform.position = Vector3.Slerp(aliensGroup.transform.position, new Vector3(1.25f, 0.5f, 0f), Time.deltaTime * timer);
                }
                else
                {
                    aliensGroup.transform.position = Vector3.Lerp(aliensGroup.transform.position, new Vector3(1.25f, 0.5f, 0f), Time.deltaTime * timer);
                }

                shieldGroup.transform.position = Vector3.Lerp(shieldGroup.transform.position, new Vector3(0, -1.75f, 0f), Time.deltaTime * timer);
            }

            if (tweening.Enabled && tweening.Shake)
            {
                int total = aliensGroup.transform.childCount;
                if (shake_intensity > 0)
                {
                    int i = 0;
                    foreach (Transform item in aliensGroup.transform)
                    {
                        Vector3 originPosition = aliensOriginPosition[i++];
                        item.localPosition = originPosition + Random.insideUnitSphere * shake_intensity;
                        //transform.rotation = new Quaternion(
                        //    originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .2f,
                        //    originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .2f,
                        //    originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .2f,
                        //    originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .2f);  
                        if (i > total - 1)
                        {
                            i = 0;
                        }
                    }
                    shake_intensity -= shake_decay;
                }
                else
                {
                    //tweening.Shake = false;
                }
            }

            Invoke("TweeningDisable", 7f);
        }

        if (Input.GetKeyDown(KeyCode.Pause) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            showSideBar = !showSideBar;
            Time.timeScale = 1 - Time.timeScale;

            AudioListener.pause = !AudioListener.pause;
            Camera.main.GetComponent<AudioListener>().enabled = !Camera.main.GetComponent<AudioListener>().enabled;
        }
    }

    private float GetTimeTweeningScale()
    {
        if (tweening.Time_Scale && shieldGroup.transform.position.y < 5f)
        {
            Time.timeScale -= Time.deltaTime;
        }
        if (tweening.Time_Scale && shieldGroup.transform.position.y < 1f)
        {
            Time.timeScale += Time.deltaTime * 5f;
        }
        return timer;
    }

    void OnGUI()
    {
        if (showSideBar)
        {
            GUI.skin = skinSideBar;

            // BACKGROUND
            GUI.DrawTexture(new Rect(0, 0, Screen.width / 3, Screen.height), textureSideBar);
            // TÍTULO
            GUI.Box(new Rect(0, 0, Screen.width / 3, Screen.height), "JUICINESS");

            // COLORS
            if (GUI.Button(new Rect(12.5f, 25f, 200, 20), "Colors"))
            {
                itemMenu = ItemMenu.Colors;
            }
            // TWEENING
            if (GUI.Button(new Rect(12.5f, 50f, 200, 20), "Tweening"))
            {
                itemMenu = ItemMenu.Tweening;
            }
            // STRETCH AND SQUEEZE
            if (GUI.Button(new Rect(12.5f, 75f, 200, 20), "Stretch and Squeeze"))
            {
                itemMenu = ItemMenu.StretchAndSqueezem;
            }
            // SOUNDS
            if (GUI.Button(new Rect(12.5f, 100f, 200, 20), "Sounds"))
            {
                itemMenu = ItemMenu.Sounds;
            }
            // PARTICLES
            if (GUI.Button(new Rect(12.5f, 125f, 200, 20), "Particles"))
            {
                itemMenu = ItemMenu.Particles;
            }
            // SCREEN SHAKE
            if (GUI.Button(new Rect(12.5f, 150f, 200, 20), "Screen Shake"))
            {
                itemMenu = ItemMenu.ScreenShake;
            }
            // PERSONALITY
            if (GUI.Button(new Rect(12.5f, 175f, 200, 20), "Personality"))
            {
                itemMenu = ItemMenu.Personality;
            }
            // FINISH HIM
            //if (GUI.Button(new Rect(12.5f, 200f, 200, 20), "Finish Him"))
            //{
            //    itemMenu = ItemMenu.FinishHim;
            //}
            //// OTHER
            //if (GUI.Button(new Rect(12.5f, 225f, 200, 20), "Other"))
            //{
            //    itemMenu = ItemMenu.Other;
            //}

            if (itemMenu == ItemMenu.Colors)
            {
                GUI.Label(new Rect(12.5f, 250f, 200, 20), "Colors");
                colors.ScreenColors = GUI.Toggle(new Rect(12.5f, 275f, 200, 20), colors.ScreenColors, "SCREEN COLORS");
                colors.BackgroundColorEffect = GUI.Toggle(new Rect(12.5f, 300f, 200, 20), colors.BackgroundColorEffect, "BACKGROUND COLOR EFFECT");

                ChangeColors();
                PlayerPrefs.SetInt("SCREEN_COLORS", System.Convert.ToInt32(colors.ScreenColors));
            }
            else if (itemMenu == ItemMenu.Tweening)
            {
                GUI.Label(new Rect(12.5f, 250f, 200, 20), "Tweening");
                tweening.Enabled = GUI.Toggle(new Rect(12.5f, 275f, 200, 20), tweening.Enabled, "TWEENING ENABLED");
                tweening.Bounce_Y = GUI.Toggle(new Rect(12.5f, 300f, 200, 20), tweening.Bounce_Y, "TWEENING BOUNCE Y");
                tweening.Rotation_Smooth = GUI.Toggle(new Rect(12.5f, 325f, 200, 20), tweening.Rotation_Smooth, "TWEENING ROTATION SMOOTH");
                tweening.Time_Scale = GUI.Toggle(new Rect(12.5f, 350f, 200, 20), tweening.Time_Scale, "TWEENING TIME SCALE");
                tweening.Shake = GUI.Toggle(new Rect(12.5f, 380f, 200, 20), tweening.Shake, "TWEENING SHAKE");

                PlayerPrefs.SetInt("TWEENING ENABLE", System.Convert.ToInt32(tweening.Enabled));
                PlayerPrefs.SetInt("TWEENING ROTATION SMOOTH", System.Convert.ToInt32(tweening.Enabled && tweening.Rotation_Smooth));
                PlayerPrefs.SetInt("TWEENING TIME SCALE", System.Convert.ToInt32(tweening.Enabled && tweening.Time_Scale));
                PlayerPrefs.SetInt("TWEENING BOUNCE Y", System.Convert.ToInt32(tweening.Enabled && tweening.Bounce_Y));
                PlayerPrefs.SetInt("TWEENING SHAKE", System.Convert.ToInt32(tweening.Enabled && tweening.Shake));
            }
            else if (itemMenu == ItemMenu.StretchAndSqueezem)
            {
                GUI.Label(new Rect(12.5f, 250f, 200, 20), "Stretch and Squeezem");
                stretchAndSqueeze.ShipStretch = GUI.Toggle(new Rect(12.5f, 275f, 200, 20), stretchAndSqueeze.ShipStretch, "SHIP STRETCH");
                //stretchAndSqueeze.AmmoStretch = GUI.Toggle(new Rect(12.5f, 350f, 200, 20), stretchAndSqueeze.AmmoStretch, "AMMO STRETCH");
                stretchAndSqueeze.AlienStretch = GUI.Toggle(new Rect(12.5f, 300f, 200, 20), stretchAndSqueeze.AlienStretch, "ALIEN STRETCH");
                //stretchAndSqueeze.WallStretch = GUI.Toggle(new Rect(12.5f, 325f, 200, 20), stretchAndSqueeze.WallStretch, "WALL STRETCH");

                ChangeShipStretch();
                //ChangeAmmoStretch();
                ChangeAlienStretch();
                //ChangeWallStretch();
            }
            else if (itemMenu == ItemMenu.Sounds)
            {
                GUI.Label(new Rect(12.5f, 250f, 200, 20), "Sounds");
                sounds.Ammo = GUI.Toggle(new Rect(12.5f, 275f, 200, 20), sounds.Ammo, "SOUND AMMO");
                sounds.Kill = GUI.Toggle(new Rect(12.5f, 300f, 200, 20), sounds.Kill, "SOUND KILL");
                sounds.Effects = GUI.Toggle(new Rect(12.5f, 325f, 200, 20), sounds.Effects, "SOUND EFFECTS");
                sounds.Music = GUI.Toggle(new Rect(12.5f, 350f, 200, 20), sounds.Music, "SOUND MUSIC");
                
                ChangeMusic();
            }
            else if (itemMenu == ItemMenu.Particles)
            {
                GUI.Label(new Rect(12.5f, 250f, 200, 20), "Particles");
                particles.AmmoCollision = GUI.Toggle(new Rect(12.5f, 275f, 200, 20), particles.AmmoCollision, "PARTICLES AMMO COLLISION");
                //particles.AmmoSmoke = GUI.Toggle(new Rect(12.5f, 275f, 200, 20), particles.AmmoSmoke, "PARTICLES AMMO SMOKE");
                particles.FallOff = GUI.Toggle(new Rect(12.5f, 300f, 200, 20), particles.FallOff, "FALL OFF");
                particles.Dark = GUI.Toggle(new Rect(12.5f, 325f, 200, 20), particles.Dark, "DARK");
                //particles.BlockRotate = GUI.Toggle(new Rect(12.5f, 400f, 200, 20), particles.BlockRotate, "BLOCK ROTATE");
                //particles.BlockDarken = GUI.Toggle(new Rect(12.5f, 425f, 200, 20), particles.BlockDarken, "BLOCK DARKEN");
                //particles.BlockShatter = GUI.Toggle(new Rect(12.5f, 450f, 200, 20), particles.BlockDarken, "BLOCK SHATTER");
                //particles.ParticleBlockShatter = GUI.Toggle(new Rect(12.5f, 475f, 200, 20), particles.BlockDarken, "PARTICLE BLOCK SHATTER");
                //particles.ParticlePaddleCollision = GUI.Toggle(new Rect(12.5f, 500f, 200, 20), particles.ParticlePaddleCollision, "PARTICLE PADDLE COLLISION");
                //particles.BallTrail = GUI.Toggle(new Rect(12.5f, 525f, 200, 20), particles.BallTrail, "BALL TRAIL");
            }
            else if (itemMenu == ItemMenu.ScreenShake)
            {
                GUI.Label(new Rect(12.5f, 250f, 200, 20), "Screen Shake");
                screenShake.Shake = GUI.Toggle(new Rect(12.5f, 275f, 200, 20), screenShake.Shake, "SCREEN SHAKE");
                GUI.Label(new Rect(12.5f, 300f, 100, 20), "SHAKE POWER");
                screenShake.ShakePower = GUI.HorizontalSlider(new Rect(110f, 305f, 80, 20), screenShake.ShakePower, 0f, 0.2f); // SHAKE POWER
                GUI.Label(new Rect(197.5f, 300f, 100, 20), screenShake.ShakePower.ToString("f2"));

                Camera.main.GetComponent<CameraShake>().coef_shake_intensity = screenShake.ShakePower;
            }
            else if (itemMenu == ItemMenu.Personality)
            {
                GUI.Label(new Rect(12.5f, 250f, 200, 20), "Personality");
                personality.ShipFace = GUI.Toggle(new Rect(12.5f, 275f, 200, 20), personality.ShipFace, "SHIP FACE");
                //personality.PaddleLookAtBall = GUI.Toggle(new Rect(12.5f, 300f, 200, 20), personality.PaddleLookAtBall, "PADDLE LOOK AT BALL");
                //GUI.Label(new Rect(12.5f, 325f, 100, 20), "PADDLE SMILE");
                //personality.PaddleSmile = GUI.HorizontalSlider(new Rect(110f, 330f, 80, 20), personality.PaddleSmile, 0f, 100f); // PADDLE SMILE
                //GUI.Label(new Rect(197.5f, 325f, 100, 20), personality.PaddleSmile.ToString("f2"));
                //GUI.Label(new Rect(12.5f, 350f, 100, 20), "PADDLE EYE SIZE");
                //personality.PaddleEyeSize = GUI.HorizontalSlider(new Rect(110f, 355f, 80, 20), personality.PaddleEyeSize, 0f, 300f); // PADDLE EYE SIZE
                //GUI.Label(new Rect(197.5f, 350f, 100, 20), personality.PaddleEyeSize.ToString("f2"));
                //GUI.Label(new Rect(12.5f, 375f, 100, 20), "EYE SEPARATION");
                //personality.PaddleEyeSeparation = GUI.HorizontalSlider(new Rect(110f, 380f, 80, 20), personality.PaddleEyeSeparation, 0f, 60f); // PADDLE EYE SEPARATION
                //GUI.Label(new Rect(197.5f, 375f, 100, 20), personality.PaddleEyeSeparation.ToString("f2"));

                shipEyes.SetActive(personality.ShipFace);
            }
            else if (itemMenu == ItemMenu.FinishHim)
            {
                //GUI.Label(new Rect(12.5f, 250f, 200, 20), "Finish Him");
                //finishHim.ScreenColorGlitch = GUI.Toggle(new Rect(12.5f, 275f, 200, 20), finishHim.ScreenColorGlitch, "SCREEN COLOR GLITCH");
            }
            else if (itemMenu == ItemMenu.Other)
            {
                //GUI.Label(new Rect(12.5f, 250f, 200, 20), "Other");
                //GUI.Label(new Rect(12.5f, 275f, 100, 20), "NUM BALLS");
                //other.NumBalls = GUI.HorizontalSlider(new Rect(110f, 280f, 80, 20), other.NumBalls, 0f, 1f); // NUM BALLS
                //GUI.Label(new Rect(197.5f, 275f, 100, 20), other.NumBalls.ToString("f2"));
            }
        }
    }

    //private void ChangeWallStretch()
    //{
    //    foreach (GameObject item in listObjectsRock)
    //    {
    //        item.SetActive(stretchAndSqueeze.WallStretch);
    //    }
    //}

    private void ChangeMusic()
    {
        if (sounds.Music)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
        else
        {
            if (audio.isPlaying)
            {
                audio.Stop();
            }
        }

        aliensGroup.GetComponent<AliensGroup>().enabledMovementSound = sounds.Effects;

        ship.GetComponent<PlayerShooter>().enableAmmoSound = sounds.Ammo;
        ship.GetComponent<PlayerMovement>().enableKillSound = sounds.Kill;
    }

    private void ChangeColors()
    {
        colors.SetColors
        (
            kindOfColors,
            new GUIText[] 
                    {
                        GameObject.FindGameObjectWithTag("GUI Lifes").GetComponent<GUIText>(),
                        GameObject.FindGameObjectWithTag("GUI Points").GetComponent<GUIText>(),
                        GameObject.FindGameObjectWithTag("GUI Records").GetComponent<GUIText>()
                    },
            GameObject.FindGameObjectsWithTag("Shield"),
            GameObject.FindGameObjectsWithTag("Alien"),
            GameObject.Find("Ground"),
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().graphicsRenderer,
            listObjectsRock
        );

        colors.SetBackgroundEffects(groupOfBackgroundColors);
    }

    public void RandomBackgroundColor()
    {
        colors.RandomBackgroundColor();
        Invoke("BackToOriginalBackgroundColor", 0.1f);
    }

    public void BackToOriginalBackgroundColor()
    {
        colors.BackToOriginalBackgroundColor();
        CancelInvoke("BackToOriginalBackgroundColor");
    }
}
