using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //Member variables can be referred to as fields.
    private int _healthPoints;

    //healthPoints is a basic property
    public int healthPoints
    {
        get
        {
            //Some other code
            // ...
            return _healthPoints;
        }
        set
        {
            // Some other code, check etc
            // ...
            _healthPoints = value; // value is the amount passed by the setter
        }
    }

    // usage
    // Debug.Log(player.healthPoints); // this will call instructions under get{}
    // player.healthPoints += 20; // this will call instructions under set{}, where value is 20
}
