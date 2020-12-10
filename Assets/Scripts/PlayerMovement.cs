using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public class DashInputNode{
        public string key;
        public float timeSinceLastInput;
        public bool singlePress;
        public DashInputNode(string _key, float _timeSinceLastInput, bool _singlePress){
           key = _key;
           timeSinceLastInput = _timeSinceLastInput;
           singlePress = _singlePress;
       }
    }
    public float movementSpeed = 500f;
    public float dash = 5f;
    public float coolDown = 2f;
    public float coolDownTimer;
    private float dashTimer = 0;
    public float dashTimeLimit = 0.07f;
    public bool canDash = true;
    private bool isDashing = false;
    private float doubleTapTime = 0.2f;

    string[] inputs = {"up", "down", "left", "right", "w", "a", "s", "d"};
    DashInputNode[] Dash = new DashInputNode[8];
       
    private Vector2 velocity;
    private Vector2 movement;
    public Rigidbody2D myBody;

    //Animation variables
    public bool isMoving;
    public float dashingNow;
    public float horizontalMoveDirection;
    public float lastDirectionLeft = 1;
    public bool movingVertically;
    public Animator animator;

    void Start() {
        coolDownTimer = coolDown;
        dashTimer = dashTimeLimit;
        isMoving = false;
        for (int i = 0; i<inputs.Length; i++){
            Dash[i] = new DashInputNode(inputs[i], 0f, false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Stores the vertical and horizontal inputs into a vector
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Velocity will be the direction times the speed
        //Movement is normalized so diagonal distance will not be long than cardinal directions.
        if(isDashing) {
            velocity = movement.normalized * movementSpeed * dash;
            canDash = false;
            dashTimer -= Time.deltaTime;
            if(dashTimer <= 0f){
                isDashing = false;
                dashingNow = -1;
                dashTimer = dashTimeLimit;
            }
        } else {
            velocity = movement.normalized * movementSpeed;
        }

        //Determines if the player stopped dashing
        //Starts cooldown
        if (!canDash && !isDashing) {
            coolDownTimer = coolDownTimer - Time.deltaTime;
            if (coolDownTimer <= 0) {
                coolDownTimer = coolDown;
                canDash = true;
            }
        }
         for (int i = 0; i<Dash.Length; i++){
             DashOnDoubleTap(ref Dash[i]);
         }
        //---------------------------------------------------------------------------------------------------
        //Animation material-

        //Determine horizontal direction of movement based on player input
        horizontalMoveDirection = movement.x;

        //Debug.Log("lastDirectionLeft = " + lastDirectionLeft);
        //Debug.Log("dashingNow =" + dashingNow);

        //animator.SetFloat("horizontalDirection", horizontalMoveDirection);

        //Check whether or not the player is currently moving, and determine what direction
        if (velocity.magnitude > 0) {

            isMoving = true;

            //Save last horizontal direction traveled
            if (horizontalMoveDirection < 0) {
            
                lastDirectionLeft = -1;

            }

            
            if (horizontalMoveDirection > 0) {

                lastDirectionLeft = 1;

            }

            //Account for case when player is moving vertically up or down, exclusively
            if (horizontalMoveDirection == 0) {

                movingVertically = true;

            }

            else {

                movingVertically = false;

            }

        }

        else {
            
            isMoving = false;

            movingVertically = false;

        }

        animator.SetBool("isMoving", isMoving);

        animator.SetFloat("horizontalDirection", horizontalMoveDirection);

        animator.SetFloat("lastDirectionLeft", lastDirectionLeft);

        animator.SetBool("movingVertically", movingVertically);

        animator.SetFloat("dashingNow", dashingNow);

        //---------------------------------------------------------------------------------------------------

    }
    //scuffed function so I don't have to copy/paste a lot 
    //maybe refactor later
    void DashOnDoubleTap(ref DashInputNode DashInput){
          if(DashInput.singlePress){
            DashInput.timeSinceLastInput += Time.deltaTime;
            if (DashInput.timeSinceLastInput>doubleTapTime){
                 DashInput.timeSinceLastInput = 0f;
                 DashInput.singlePress = false;
            }
        }
        
        if(Input.GetKeyDown(DashInput.key)) {
                DashInput.singlePress = true;
            if(DashInput.timeSinceLastInput<doubleTapTime && DashInput.timeSinceLastInput>0f){
                if(canDash){
                    isDashing = true;
                    dashingNow = 1;
                    if(!FindObjectOfType<GameManager>().Over && PlayerPrefs.GetInt("Mute") == 0)
                    FindObjectOfType<AudioManager>().Play("DashSound");
                }
            }
        }
          
    }

    void FixedUpdate()
    {
        //Moves the position of the player
        myBody.MovePosition(myBody.position + velocity * Time.fixedDeltaTime);

    }
}