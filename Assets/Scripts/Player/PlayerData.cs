using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Player", order = 1)]
public class PlayerData : ScriptableObject
{
    public int hp;
    public int damage;
    public float speed;
}
