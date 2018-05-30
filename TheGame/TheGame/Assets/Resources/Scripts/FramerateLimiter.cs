using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

public class FramerateLimiter : MonoBehaviour
{
	float dt = 0f;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        if (Application.targetFrameRate != ApplicationModel.targetFrameRate)
        {
            Application.targetFrameRate = ApplicationModel.targetFrameRate;
        }
        dt += (Time.unscaledDeltaTime - dt);
    }

    void OnGUI()
    {
        if (ApplicationModel.fpsOn)
        {
            int w = Screen.width, h = Screen.height;
            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h / 30;
            style.normal.textColor = new Color(1, 0, 0, 1f);
            float msec = dt * 1000f;
            float fps = 1f / dt;
            string fpsText = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, fpsText, style);
        }
    }
}

