using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Colors
    {
        private List<Color> backgroundColors;
        private Color originalColor;
        // TRUE
        public bool ScreenColors { get; set; }
        // TRUE
        public bool BackgroundColorEffect { get; set; }

        public void SetColors(Color[] listKinfOfColors, GUIText[] listPointsGuiText, GameObject[] listObjectsShield, GameObject[] listObjectsAliens, GameObject ground, Renderer player, GameObject[] listRocks)
        {
            if (ScreenColors)
            {
                Camera.main.backgroundColor = listKinfOfColors[0];

                ground.renderer.material.color = listKinfOfColors[1];
                player.material.color = listKinfOfColors[2];

                listPointsGuiText[0].color = listKinfOfColors[1];
                listPointsGuiText[1].color = listKinfOfColors[1];
                listPointsGuiText[2].color = listKinfOfColors[2];

                foreach (GameObject item in listObjectsShield)
                {
                    item.renderer.material.color = listKinfOfColors[1];
                }
                foreach (GameObject item in listObjectsAliens)
                {
                    item.renderer.material.color = listKinfOfColors[2];
                }
                foreach (GameObject item in listRocks)
                {
                    item.renderer.material.color = listKinfOfColors[1];
                }
            }
            else
            {
                Camera.main.backgroundColor = Color.black;

                ground.renderer.material.color = Color.white;
                player.material.color = Color.white;

                foreach (GUIText item in listPointsGuiText)
                {
                    item.color = Color.white;
                }
                foreach (GameObject item in listObjectsShield)
                {
                    item.renderer.material.color = Color.white;
                }
                foreach (GameObject item in listObjectsAliens)
                {
                    item.renderer.material.color = Color.white;
                }
                foreach (GameObject item in listRocks)
                {
                    item.renderer.material.color = Color.white;
                }
            }
        }

        public void SetBackgroundEffects(Color[] groupOfBackgroundColors)
        {
            backgroundColors = groupOfBackgroundColors.ToList();
        }

        public void RandomBackgroundColor()
        {
            originalColor = Camera.main.backgroundColor;

            int random = UnityEngine.Random.Range(0, backgroundColors.Count);
            Camera.main.backgroundColor = backgroundColors[random];
        }

        public void BackToOriginalBackgroundColor()
        {
            Camera.main.backgroundColor = originalColor;
        }
    }
}
