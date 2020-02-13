using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    
        private bool set = false;
        private Vector3 firePos;
        private Vector3 direction;
        private float speed;
        private float timeElapsed;
 


    void Update()
    {
        if (!set)
            return;
        timeElapsed += Time.deltaTime;
        transform.position = firePos +direction * speed  * timeElapsed;
        transform.position += Physics.gravity * (timeElapsed *timeElapsed) / 2.0f;
        // extra validation for cleaning the scene
        if (transform.position.y < -1.0f)
            Destroy(this.gameObject);// or set = false; and hide it
    }
    public void Set(Vector3 firePos, Vector3 direction, float speed)
    {
        this.firePos = firePos;
        this.direction = direction.normalized;
        this.speed = speed;
        //transform.position = firePos;
        set = true;
    }
}
