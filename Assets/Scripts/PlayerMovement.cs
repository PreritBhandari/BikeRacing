using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerMovement : MonoBehaviour
{
    private CharacterController bike;
    
    private Vector3 moveVector;

    public float speed;
    public float vert ;
    private float autospeed=4.0f;

    public AudioSource audio;

    public AudioSource gameOver;



    // float smooth = 1.0f;

    // private float verticalVelocity=0.0f;
    // private float gravity=12.0f;


    void Start() {

        bike=GetComponent<CharacterController>();

        audio=GetComponent<AudioSource>();
        audio.Play();
        
    }




    void Update() {
      

        bike.Move(Vector3.forward*autospeed*Time.deltaTime);

        

        moveVector.x=Input.GetAxis("Horizontal")*speed*3;
      

        // forward and backward 

        // moveVector.z=Input.GetAxisRaw("Vertical")*vert;



        bike.Move(moveVector*Time.deltaTime);


      
        
    }


    public void OnTriggerEnter(Collider other) {
       
        if (other.gameObject.tag == "Block") {

            StartCoroutine(Death());

      
        }
    }

    IEnumerator Death() {
        Destroy(gameObject);
        yield return new WaitForSeconds(1); //waits 3 seconds
        gameOver.Play();

        
    }

        
 
      
}
