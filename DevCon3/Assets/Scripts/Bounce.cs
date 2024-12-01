using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    //Attached to platform prefab and adds velocity to bullet after collision -cc
    private Rigidbody2D rb;
    private bool canBounce;
    
    //Controls up/right movement after collision with platform
    private float bounceForce = 2000;
    private float moveRight = 1500;
    void Start()
    {
        rb = GameObject.Find("GreyboxProjectile(Clone)").GetComponent<Rigidbody2D>();       
    }
    void FixedUpdate()
    {
        if(canBounce){
            //removes bullets original velocity and 
            rb.velocity = new Vector2(0,0);
            rb.AddForce (new Vector2 (moveRight, bounceForce));
            canBounce = false;
        }
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Bullet")){
            canBounce = true;
        }
        Destroy(gameObject);
    }
}
