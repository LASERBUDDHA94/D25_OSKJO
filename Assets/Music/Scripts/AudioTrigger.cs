using UnityEngine;
using UnityEngine.Events;
using FMODUnity;
using FMOD.Studio;

public class AudioTrigger : MonoBehaviour
{
    //public UnityEvent onTriggered;
    public bool setParameter;
    public bool play;
    public bool stop;
    public string eventName;
    public string parameterName;
    public float value;
    //private GameObject aController = GameObject.Find("AudioController");
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //onTriggered.Invoke();
            Debug.Log(this.gameObject.name + " triggered");

            if (setParameter)
            {
                Debug.Log("Calling setParameter: "  + parameterName + " value: " + value);
                GameObject.Find("AudioController").GetComponent<AudioController>().SetParameter("event:/" + eventName, parameterName, value);
            }

            if (play)
            {
                Debug.Log("Calling play: " + eventName);
                GameObject.Find("AudioController").GetComponent<AudioController>().PlayEvent("event:/" + eventName);
            }

            if (stop)
            {
                Debug.Log("Calling stop: " + eventName);
                GameObject.Find("AudioController").GetComponent<AudioController>().StopEvent("event:/" + eventName);
            }

        }
    }
    
    
    
}
