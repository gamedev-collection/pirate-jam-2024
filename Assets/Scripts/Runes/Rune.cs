using UnityEngine;

public abstract class Rune : ScriptableObject
{
    [SerializeField] private SO_Rune _runeData;
    public SO_Rune RuneData { get { return _runeData; } }
}
