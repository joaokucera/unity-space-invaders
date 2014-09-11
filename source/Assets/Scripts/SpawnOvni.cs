using UnityEngine;
using System.Collections;

public class SpawnOvni : MonoBehaviour
{
    #region Fields

    public SideBar scriptSideBar;
    public PlayerShooter scriptPlayerShooter;
    public GameObject prefabOvni;
    
    private bool[] spawnMarks = { false, false, false, false, false };

    #endregion

    #region Methods

    void Update()
    {
        if (!spawnMarks[0] && scriptPlayerShooter.points >= 10)
        {
            Spawn(0);
        }
        else if (!spawnMarks[1] && scriptPlayerShooter.points >= 200)
        {
            Spawn(1);
        }
        else if (!spawnMarks[2] && scriptPlayerShooter.points >= 300)
        {
            Spawn(2);
        }
        else if (!spawnMarks[3] && scriptPlayerShooter.points >= 400)
        {
            Spawn(3);
        }
        else if (!spawnMarks[4] && scriptPlayerShooter.points >= 500)
        {
            Spawn(4);
        }
    }

    private void Spawn(int index)
    {
        GameObject ovni = GameObject.Find("Ovni");
        if (ovni == null)
        {
            ovni = GameObject.Find("Ovni(Clone)");
            if (ovni == null)
            {
                GameObject go = (GameObject)Instantiate(prefabOvni, transform.position, transform.rotation);
                
                scriptSideBar.colors.ScreenColors = System.Convert.ToBoolean(PlayerPrefs.GetInt("SCREEN_COLORS", 0));

                if (scriptSideBar.colors.ScreenColors)
                {
                    go.renderer.material.color = scriptSideBar.kindOfColors[2];
                }
                else
                {
                    go.renderer.material.color = Color.white;
                }
            }
            spawnMarks[index] = true;
        }
    }

    #endregion
}
