using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Profiling;
using UnityEngine;

public class UI_telemetry : MonoBehaviour
{
    public float     TeleRefreshRate     = 0.6f;
    public string    Game_Version        = "[SimpCrawler v0.5a(28.08.2022--14:31)] --> Alpha Build";
    private float    lastRefresh         = 0f;
    private Vector3  PlayerLocation      = Vector3.zero;
    
    [SerializeField]private GameObject          telemetryTarget;
    [SerializeField]private List<GameObject>    debugToSwitch;

   
    public void Init()
    {
        textUpdate();
        teleSwitch();
    }


    void Update ()
    {
        if(Input.GetKeyDown("f1"))
        {
            teleSwitch();
        }

        if(lastRefresh >= TeleRefreshRate && telemetryTarget.activeSelf)
            textUpdate();
        else
            lastRefresh += Time.deltaTime;
    }


    void textUpdate()
    {
            double usedVram = Profiler.GetAllocatedMemoryForGraphicsDriver()*0.000001;
            float current = 0;
            current = Time.frameCount / Time.time;

            telemetryTarget.GetComponent<Text>().text = 
                Game_Version + "\n\n"

                + $"[{SystemInfo.graphicsDeviceName}]\n"
                + $"[{Screen.width}x{Screen.height}] --> {SystemInfo.graphicsDeviceType}  ({(int)usedVram}mb/{SystemInfo.graphicsMemorySize}mb)\n"
                + $"[{SystemInfo.graphicsDeviceVersion}] --> smLv{SystemInfo.graphicsShaderLevel}\n\n"

                + $"[{Time.frameCount}] --> Frame\n"
                + $"[{ (int)(Time.deltaTime * 1000f) }ms] --> FrameTime\n"
                + $"[{(int)current}] --> FramesPerSecond\n\n"

                + $"[x.{PlayerLocation.x.ToString("F1")}, y.{PlayerLocation.y.ToString("F1")}, z.{PlayerLocation.z.ToString("F1")}]";
                
            lastRefresh = 0f;
    }

    void teleSwitch()
    {
        telemetryTarget.SetActive(!telemetryTarget.activeSelf);
        foreach (GameObject toSwitch in debugToSwitch)
            toSwitch.SetActive(!toSwitch.activeSelf);
        
    }

        void updateLocation(Vector3 position)
    {
        PlayerLocation = position;
    }


    ////////////////////////////
    ////Events
    private void OnEnable()
    {
        // Subscribe to the OnSomethingFound event
        controller_player.PlayerLocation += updateLocation;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnSomethingFound event
        controller_player.PlayerLocation -= updateLocation;
    }
}
