using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour
{
    public Texture2D fadeOutTexture;

    public float fadeSpeed = 0.5f;

    public float alpha = 0.3f;

    private int drawDepth = 10;

    private bool isFading = true;

    void start()
    {
      
    }

    void OnGUI()
    {
        if (isFading)
        {
            GUI.depth = drawDepth;

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
            GUI.depth = drawDepth;


        }


        //        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
        //GUI.color = new Color(1, 1, 1, alpha);
        //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
        //GUI.color = Color.white;
        //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }
}