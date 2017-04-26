using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugTracer : MonoBehaviour
{
    public static DebugTracer instance;

    /************************************************翻译完成********************************************/
    public List<string> ListDebug = new List<string>();
    Vector2 DebugScrollPosition;
    GUISkin myGUISkin;
    Rect myDebugWindow;
    string DebugLabel;

    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval
    private float fps;
    // Use this for initialization

    void Start()
    {
        instance = this;
        timeleft = updateInterval;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    timeleft -= Time.deltaTime;
    //    accum += Time.timeScale / Time.deltaTime;
    //    ++frames;

    //    // Interval ended - update GUI text and start new interval
    //    if (timeleft <= 0.0)
    //    {
    //        // display two fractional digits (f2 format)
    //        fps = accum / frames;

    //        timeleft = updateInterval;
    //        accum = 0.0F;
    //        frames = 0;
    //    }
    //}

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(Screen.width / 2, 100f, 100f, 100f), fps.ToString());
    //}



    public void CreateDebug(string DebugSource, string DebugString)
    {
        Debug.Log(DebugSource + "：" + DebugString);
        ListDebug.Add("=" + DebugSource + "=" + DebugString);
        if (ListDebug.Count > 100)
        {
            ListDebug.RemoveAt(0);
        }
    }
}
