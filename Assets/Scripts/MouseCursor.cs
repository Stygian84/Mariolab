using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Canvas canvas;
    private Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        startPos = Input.mousePosition/canvas.scaleFactor;
        
    }
}
