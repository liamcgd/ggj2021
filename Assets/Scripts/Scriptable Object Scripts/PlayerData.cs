using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Player Data")]
public class PlayerData : ScriptableObject
{
    public SimpleFloat health;
    public Sprite playerCharacter;
    public Sprite partnerCharacter;
}
