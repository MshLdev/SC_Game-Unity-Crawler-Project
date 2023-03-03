using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///
///--
/// Lets try to Control a flow of initialization 
/// to make it 100% safe and reliable
public class controller_Script : MonoBehaviour
{
    private DATABASE        scDatabase;
    private AudioMenager    scAudioMenager;
    private Navigation      scNavigation;
    private Hero            scHero;
    private UI_telemetry    scTelemetry;
    private UI_Interface    scInterface;
    private UI_tooltip      scTooltip;
    private DEBUGER         scDebuger;

    void Start()
    {
        scDatabase =        GetComponent<DATABASE>();
        scAudioMenager =    GetComponent<AudioMenager>();
        scNavigation =      GetComponent<Navigation>();
        scHero =            GetComponent<Hero>();
        scTelemetry =       GetComponent<UI_telemetry>();
        scInterface =       GetComponent<UI_Interface>();
        scTooltip =         GetComponent<UI_tooltip>();
        scDebuger =         GetComponent<DEBUGER>();

        scriptsinit();
    }

    void scriptsinit()
    {   
        scDatabase.LoadDB();
        scAudioMenager.LoadResources();
        scNavigation.CreateNavMap();
        scHero.Init();
        scTelemetry.Init();
        scTooltip.Init();
        scInterface.Create();
    }
}
