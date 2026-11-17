using UnityEngine;
using UnityEngine.Events;

public class AudioTriggerZ : MonoBehaviour
{
    //public UnityEvent onTriggerEnter;
    
    public enum ElementalPower
    {
        None,
        Fire,
        Psychic,
        Dark,
        Earth
    }

    [System.Serializable]
    public struct CharacterInformation
    {
        public string name;
        public float speed;
        public int health;
        public bool canFly;
        public bool isCool;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("OnTriggerEnter");
            //onTriggerEnter.Invoke();
        }
    }
    */
    
    //public ElementalPower elementKraft;
    
    //private void Start()
    //{
    //    Debug.Log(elementKraft);
    //}
}
