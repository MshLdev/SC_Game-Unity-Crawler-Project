using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Profiling;
using UnityEngine;

public class UI_telemetry : MonoBehaviour
{
    public float TeleRefreshRate = 0.6f;
    public string Game_Version = "[SimpCrawler v0.2a(13.08.2022)] --> Build";
    public int avgFrameRate;
    
    Text Tele_Text;
    GameObject Player_GO;
    GameObject Text_GO;
    float lastRefresh;



 
    
    public void Start()
    {
        Text_GO = GameObject.Find("UI_Telemetry");
        Tele_Text = Text_GO.GetComponent<Text>();
        Player_GO = GameObject.Find("Player");
        textUpdate();
    }

    public void Update ()
    {
        if(Input.GetKeyDown("f1"))
            textSwitch();

        if(lastRefresh >= TeleRefreshRate && Text_GO.activeSelf)
            textUpdate();
        else
            lastRefresh += Time.deltaTime;
    }



    public void textUpdate()
    {
            double usedVram = Profiler.GetAllocatedMemoryForGraphicsDriver()*0.000001;
            float current = 0;
            current = Time.frameCount / Time.time;
            avgFrameRate = (int)current;

            Tele_Text.text = 
                Game_Version + "\n\n"

                + $"[{SystemInfo.graphicsDeviceName}]\n"
                + $"[{Screen.width}x{Screen.height}] --> {SystemInfo.graphicsDeviceType}  ({(int)usedVram}mb/{SystemInfo.graphicsMemorySize}mb)\n"
                + $"[{SystemInfo.graphicsDeviceVersion}] --> smLv{SystemInfo.graphicsShaderLevel}\n\n"

                + $"[{Time.frameCount}] --> Frame\n"
                + $"[{ (int)(Time.deltaTime * 1000f) }ms] --> FrameTime\n"
                + $"[{avgFrameRate}] --> FramesPerSecond\n\n"

                + $"[x.{Player_GO.transform.position.x.ToString("F1")}, y.{Player_GO.transform.position.y.ToString("F1")}, z.{Player_GO.transform.position.z.ToString("F1")}]";
                

            lastRefresh = 0;
    }

    public void textSwitch()
    {
        Text_GO.SetActive(!Text_GO.activeSelf);
    }
}
