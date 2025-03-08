using UnityEngine;

public enum SpellType
    {
        Fireball,
        Zap,
        Frost_whirlwind,
        Spike
    }

public class SpellTypeData : MonoBehaviour
{
    [SerializeField] private Sprite basic;
    [SerializeField] private Sprite fireball;
    [SerializeField] private Sprite zap;
    [SerializeField] private Sprite frost_whirlwind;
    [SerializeField] private Sprite spike;
    
    public Sprite GetDataByID(SpellType id)
    {
        switch (id)
        {
            case SpellType.Fireball:
                return fireball;
                break;
            case SpellType.Zap:
                return zap;
                break;
            case SpellType.Frost_whirlwind:
                return frost_whirlwind;
                break;
            case SpellType.Spike:
                return spike;
                break;
        }
        return basic;
    }
}