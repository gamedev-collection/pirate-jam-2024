using UnityEngine;

public class Rune : MonoBehaviour
{
    [SerializeField] private SO_Rune _runeData;
    public SO_Rune RuneData { get { return _runeData; } }
}
