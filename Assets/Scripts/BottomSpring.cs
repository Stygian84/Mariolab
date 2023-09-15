using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomSpring : MonoBehaviour
{
    public LayerMask player;
    private BoxCollider2D boxCollider2D;
    private SpringJoint2D springJoint2D;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        springJoint2D = GetComponent<SpringJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bottomDetection())
        {
            unkillSpringJoint2D();
        }
        else
        {
            killSpringJoint2D();
        }
    }

    void killSpringJoint2D()
    {
        springJoint2D.distance = 0;
        springJoint2D.dampingRatio = 1;
        springJoint2D.frequency = 0;
    }

    void unkillSpringJoint2D()
    {
        springJoint2D.distance = 0.1f;
        springJoint2D.dampingRatio = 5;
        springJoint2D.frequency = 5;
    }

    private bool bottomDetection()
    {
        Debug.Log("Player Detected");
        return Physics2D.BoxCast(
            boxCollider2D.bounds.center,
            boxCollider2D.bounds.size,
            0f,
            Vector2.down,
            .1f,
            player
        );
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider2D.bounds.center,
            new Vector2(
                boxCollider2D.bounds.size.x,
                boxCollider2D.bounds.size.y-0.1f
            )
        );
    }
    
}
