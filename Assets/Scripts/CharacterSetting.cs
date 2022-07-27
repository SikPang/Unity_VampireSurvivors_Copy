using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetting : MonoBehaviour
{
    [SerializeField] Character character;

    public Character GetCharacter()
    {
        return character;
    }
}
