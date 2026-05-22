using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startpos,length;
   [SerializeField] private GameObject cam;
   [SerializeField]private  float parallaxEffect;
    
    void Start()
    {
        startpos = this.transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = startpos * parallaxEffect;
        float movement = cam.transform.position.x  *(1-parallaxEffect);
        this.transform.position = new Vector2(startpos + distance, this.transform.position.y);

        if (movement > startpos + length) startpos += length;
        else if (movement < startpos - length) startpos -= length;
    }
}
