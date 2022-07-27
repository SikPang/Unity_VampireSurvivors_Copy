using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetting : MonoBehaviour
{
    [SerializeField] CharacterData character;

    public CharacterData GetCharacter()
    {
        return character;
    }
}
