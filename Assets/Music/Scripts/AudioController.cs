using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioController : MonoBehaviour
{

    //Combotimer för EnemyHit-stinger-system (á la Windwaker)
    //Tanken är att varje gång man slår en fiende ska det hända "stingers" i form av noter
    //Noter som går uppåt i skalan som är relevant till themet som spelas
    //Mer info om det i FMOD-noteringarna
    //Något som också är viktigt är att det finns variation, så att det inte är samma serie varje gång
    //Sidechain vore också bra, behandlas kanske lite mer som SFX
    //Varje gång man slår en fiende så sätts "hit" till true
    //Timern tickar ner, vid 0 så sätts "hit" till false
    //Parametern "Combo" sätts också till 0 i det relevanta eventet
    //Borde det då vara en global parameter tro? Förmodligen!
    public float comboTimer = 1f;
    public int comboHit = 0;
    public bool hit = false;
    
    //public void SetGlobalParameter(string eventName, string parameterName, float value)
    
    public void ComboTrigger()
    {
        hit = true;
        comboTimer = 2f;
        comboHit++;
        if (comboHit >= 4)
        {
            comboHit = 1;
        }
        RuntimeManager.StudioSystem.setParameterByName("Combo", comboHit);
    }
    
    void Start()
    {
        //En for-loop körs och skapar en instans för alla AudioEvents som lagts till i editorn
        for (int i = 0; i < audioEvents.Length; i++)
        {
            audioEvents[i].eventInstance = RuntimeManager.CreateInstance(audioEvents[i].audioEvent);
            
            Debug.Log("Created instance: " + audioEvents[i].audioEvent.Path);
        }
    }

    //Strukt för FMOD parametrar, så att man kan skapa en array av parametrar
    //i varje Audio Event-strukt som går att utöka, då olika event har olika antal
    [System.Serializable]
    public struct AudioParameter
    {
        public string parameterName;
        public float startingValue;
    }

    //Strukt för Audio Events
    [System.Serializable]
    public struct AudioEventData
    {
        public EventReference audioEvent;
        
        [HideInInspector]
        public EventInstance eventInstance;

        public AudioParameter[] parameters;
    }

    //Array av AudioEventData, lista som man kan utöka i editorn och lägga till events
    [SerializeField] public AudioEventData[] audioEvents;
    
    //Metod som kallas från AudioTriggers för att sätta parametrar på ett givet event
    //Det finns tre parametrar: eventnamn, parameternamn och värde
    //Metoden loopar igenom arrayen av AudioEventData och hittar eventet med rätt namn
    //Sedan sätter den värdet på den önskade parametern till önskat värde
    //Rider varnade om att när man gör på det här sättet så kan det skapas en
    //kopia av structet och att structet aldrig påverkas
    //Därför skapas en lokal variabel innan parametern sätts
    public void SetParameter(string eventName, string parameterName, float value)
    {
        foreach (AudioEventData audioEventData in audioEvents)
        {
            if (audioEventData.audioEvent.Path == eventName)
            {
                FMOD.Studio.EventInstance instance = audioEventData.eventInstance;
                
                instance.setParameterByName(parameterName, value);
                
                break;
            }
        }
    }

    //Metod för att starta ett event, fungerar liknande till SetParameter
    //En skillnad är att metoden först kollar om eventet spelas eller sustainar
    //innan den startar eventet. Om det spelar eller sustainar gör den inget
    public void PlayEvent(string eventName)
    {
        Debug.Log("Checking for: " + eventName);
        
        for (int i = 0; i < audioEvents.Length; i++)
        {
            Debug.Log("Now at: " + audioEvents[i].audioEvent.Path);
            if (audioEvents[i].audioEvent.Path == eventName)
            {
                Debug.Log("Event name matched!");
                EventInstance instance = audioEvents[i].eventInstance;

                instance.getPlaybackState(out PLAYBACK_STATE state);

                if (state != PLAYBACK_STATE.PLAYING &&
                    state != PLAYBACK_STATE.SUSTAINING)
                {
                    FMOD.RESULT result = instance.start();
                    Debug.Log("Start result: " + result);
                }

                break;
            }
        }
    }
    
    //Metod för att stoppa ett event, fungerar som PlayEvent-metoden
    //men kollar istället om eventet spelas ELLER sustainar
    //om så, så stoppas eventet (tillåter fadeout)
    public void StopEvent(string eventName)
    {
        foreach (AudioEventData audioEventData in audioEvents)
        {
            if (audioEventData.audioEvent.Path == eventName)
            {
                EventInstance instance = audioEventData.eventInstance;
                
                instance.getPlaybackState(out PLAYBACK_STATE state);

                if (state == PLAYBACK_STATE.PLAYING || state == PLAYBACK_STATE.SUSTAINING)
                {
                    instance.stop(STOP_MODE.ALLOWFADEOUT);
                }
                
                break;
            }
        }
    }

    
    void Update()
    {
        //Här dealas det med Combo-timern
        //Om vi registrerat en hit och timern är mer än noll så tickar den ner
        if (hit && comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
        }
        
        if (comboTimer <= 0f)
        {
            hit = false;
            comboHit = 0;
            comboTimer = 2f;
        }
        
    }
}
