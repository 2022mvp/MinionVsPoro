using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnit : MonoBehaviour
{
    public SUnitStatus sStatus;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitStatus(string _name, int _damage, int _attack_range, int _attack_speed, int _hp)
    {
        sStatus.name = _name;
        sStatus.damage = _damage;
        sStatus.attack_range = _attack_range;
        sStatus.attack_speed = _attack_speed;
        sStatus.hp = _hp;
    }

    public void moveUnit(Vector3 location)
    {

    }
}
