using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public Animator marioAnimator;
    public Transform marioTransform;
    public Transform bottomOfFlag;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(){
        marioAnimator.SetTrigger("flag");
        marioTransform.position= Vector2.MoveTowards(transform.position, bottomOfFlag.position, 1);
    }
}
