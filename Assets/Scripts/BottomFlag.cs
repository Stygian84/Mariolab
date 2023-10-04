using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomFlag : MonoBehaviour 
{
    public Rigidbody2D marioBody;
    public float propelforce=10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col){

         if (col.gameObject.CompareTag("Player"))
            {
                
                marioBody.AddForce(marioBody.transform.forward, ForceMode2D.Impulse);
            }
    }
}
