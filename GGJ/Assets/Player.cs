using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour {

    public float maxJumpHeight = 4;
    public float minJumpHeight = 2;
    public float timeToJumpApex = 0.4f;
    public float moveSpeed = 6;

    float accelerationTimeAirborn = 0.2f;
    float acclerationTimeGrounded = 0.1f;

    Controller2D controller;

    float gravity = -20;
    float maxJump = 8;
    float minJump = 4;
    Vector3 velocity;
    float velocityXSmoothing;

    public GameObject landingEffect;
    public GameObject dashEffectHoriz;
    public GameObject dashEffectDown;
    public GameObject dashSide;

    bool lungeOn;
    bool dig;
    [HideInInspector]public bool canJump;

    Animator anim;

    public static Player instance;

    void Start() {
        instance = this;
        anim = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJump = Mathf.Abs(gravity) * timeToJumpApex;
        minJump = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    // spawns the landing particle at the foot of the character
    public void spawnLanding() {
        spawnParticle(landingEffect, new Vector3(0, -0.25f), 0.36f);
    }

    // spawns the landing particle at the foot of the character
    public void spawnDashingUp()
    {
        spawnParticle(dashEffectHoriz, new Vector3(0, -0), 0.36f);
    }

    // spawns the landing particle at the foot of the character
    public void spawnDashingDown()
    {
        spawnParticle(dashEffectDown, new Vector3(0, -0), 0.36f);
    }

    // spawns the landing particle at the foot of the character
    public void DashHoriz() {
        GameObject effect = spawnParticle(dashSide, new Vector3(0, -0.25f), 0.30f);
        effect.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
    }

    // spawns a particle effect at the given position and then destroys it
    GameObject spawnParticle(GameObject spawnParticle, Vector3 offset, float destroy) {
        GameObject effect = Instantiate(spawnParticle, transform.position + offset, transform.rotation) as GameObject;
        Destroy(effect, destroy);
        return effect;
    }

    void Update() {
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }

        if (controller.collisions.below) {
            dig = true;
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.Z) && canJump) {
            velocity.y = maxJump;
            canJump = false;
        }
        if (Input.GetKeyUp(KeyCode.Z)) {
            if (velocity.y > minJump) {
                velocity.y = minJump;
            }
        }
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //anim.SetBool("jumping", !controller.collisions.below);

        if (input.x != 0) {
            GetComponent<SpriteRenderer>().flipX = input.x < 0.1f;
        }

        if (!lungeOn)  {
            movement(input);
            //anim.SetFloat("velocity", Mathf.Abs(input.x));
        }

        if (Input.GetKeyDown(KeyCode.C) && dig && input.SqrMagnitude() > 0.1f) {
            StopAllCoroutines();
        }

        //anim.SetFloat("velocityY", input.y);
        //anim.SetBool("lunge", lungeOn);
        controller.Move(velocity * Time.deltaTime);
    }

    void movement(Vector2 input) {
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? acclerationTimeGrounded : accelerationTimeAirborn);
        velocity.y += gravity * Time.deltaTime;
    }


    // sets instance of this to null when player is destroyed. probably a better way to do this
    void OnDestroy() {
        instance = null;
    }
}
