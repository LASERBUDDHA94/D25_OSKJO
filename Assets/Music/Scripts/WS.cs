using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class WS : MonoBehaviour
{
    public float timer = 5f;
    
    public EventInstance instans;
    public EventReference referens;
    void Start()
    {
        instans = RuntimeManager.CreateInstance(referens);
        instans.start();
    }
    
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0f)
        {
            instans.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
