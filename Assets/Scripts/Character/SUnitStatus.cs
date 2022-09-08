using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SUnitStatus
{
    public string name;
    public int damage;
    public int attack_range;
    public int attack_speed;
    public int hp;

    public SUnitStatus(string _name, int _damage, int _attack_range, int _attack_speed, int _hp)
    {
        name = _name;
        damage = _damage;
        attack_range = _attack_range;
        attack_speed = _attack_speed;
        hp = _hp;
    }
}
