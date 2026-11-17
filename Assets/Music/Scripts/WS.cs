using UnityEngine;

public class WS : MonoBehaviour
{
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

    public int[] highScores;
    
    public CharacterInformation characterInformation;
    public ElementalPower elementalPower;
    
}
