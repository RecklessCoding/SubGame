using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour
{
    public Texture2D fadeOutTexture;

    public float fadeSpeed = 0.5f;

    public float alpha = 0.3f;

    private int drawDepth = -1000;

    void OnGUI()
    {
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);      
    }
}