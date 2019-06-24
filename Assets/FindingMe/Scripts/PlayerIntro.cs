using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerIntro : MonoBehaviour
{
    public float maxJumpHeight = 4f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = .4f;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;
    private float moveSpeed = 6f;
    private float slowSpeed = 1.5f;
    private Rigidbody2D body;

    [SerializeField]
    private int pUp;

    [HideInInspector]
    public bool antiInput = false;

    private float gravity;
    private float baseGravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float walkSpeed;
    [HideInInspector]
    public Vector3 velocity;
    private float velocityXSmoothing;
    private bool justBounced = false;
    private bool justDoubleBounced = false;

    private Controller2D controller;

    private Vector2 directionalInput;

    [HideInInspector]
    public bool isOnGround = false;

    public Transform checkpoint;

    [HideInInspector]
    public bool dead = false;

    [SerializeField]
    private float deathWaitTime = 1.5f;

    [SerializeField]
    private SoundManager soundManager;

    [SerializeField]
    private AudioClip jumpAudioClip;
    private float jumpAudioVolume = 0.35f;

    [SerializeField]
    private AudioClip bumpAudioClip;
    private float bumpAudioVolume = 1.0f;

    [SerializeField]
    private AudioClip deathAudioClip;
    private float deathAudioVolume = 1.0f;

    [SerializeField]
    private AudioClip powerupAudioClip;
    private float powerupAudioVolume = 1.0f;

    [SerializeField]
    private DragonBones.UnityArmatureComponent dbArmatureComponent;

    [SerializeField]
    private string idleState;

    private float idleOffsetVelocity = 0.1f;

    [SerializeField]
    private string jumpState;

    [SerializeField]
    private string fallState;

    [SerializeField]
    private string glideState;

    [SerializeField]
    private string walkState;

    [SerializeField]
    private string bounceState;

    [SerializeField]
    private string bouncebackState;

    [SerializeField]
    private string deathState;

    private void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        baseGravity = gravity;
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        walkSpeed = moveSpeed;
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (dead)
        {
			return;
        }
        
        if (velocity.y == 0)
        {
            antiInput = false;
        }


        CalculateVelocity();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0f;
        }

        if (controller.collisions.below)
        {
            justBounced = false;
            justDoubleBounced = false;
        }
        
        if (velocity.y < 0f && pUp == 3)
        {
            gravity = baseGravity / 2;
        }
        else
        {
            gravity = baseGravity;
        }

		if (justBounced)
        {
            return;
        }

		else if (velocity.y > 0)
		{
            if (dbArmatureComponent.animation.lastAnimationName != jumpState)
            {
                dbArmatureComponent.animation.Play(jumpState);
                dbArmatureComponent.animation.timeScale = 1.0f;
            }
		}
		else if (velocity.y < 0 && pUp == 3)
		{
            if (dbArmatureComponent.animation.lastAnimationName != glideState)
            {
                dbArmatureComponent.animation.Play(glideState);
                dbArmatureComponent.animation.timeScale = 1.0f;
            }
		}
		else if (velocity.y < 0)
		{
            if (dbArmatureComponent.animation.lastAnimationName != fallState)
            {
                dbArmatureComponent.animation.Play(fallState);
                dbArmatureComponent.animation.timeScale = 1.0f;
            }
		}
        else if (velocity.x <= idleOffsetVelocity && velocity.x >= -idleOffsetVelocity)
        {
            if (dbArmatureComponent.animation.lastAnimationName != idleState)
            {
                dbArmatureComponent.animation.Play(idleState);
				dbArmatureComponent.animation.timeScale = 1.0f;
            }
        }
        else
        {
            if (dbArmatureComponent.animation.lastAnimationName != walkState)
            {
                dbArmatureComponent.animation.Play(walkState);
                if (pUp == 3)
                {
                    dbArmatureComponent.animation.timeScale = 0.2f;
                }
            }

            dbArmatureComponent.animation.timeScale = Mathf.Clamp(velocity.x, -2.0f, 2.0f);
        }
		
		if (velocity.x != 0 && !justBounced)
		{
			dbArmatureComponent.transform.localScale = new Vector3(Mathf.Sign(velocity.x), dbArmatureComponent.transform.localScale.y, dbArmatureComponent.transform.localScale.z);
		}
		
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;

            soundManager.PlayAudioClip(jumpAudioClip, volume: jumpAudioVolume);
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }


    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));
        velocity.y += gravity * Time.deltaTime;
    }

    public void Death()
    {
        soundManager.PlayAudioClip(deathAudioClip, volume: deathAudioVolume);

        StartCoroutine(DeathCoroutine(deathWaitTime));
    }

    public void UpgradePower(int powerUp, GameObject prefab)
    {
        soundManager.PlayAudioClip(powerupAudioClip, volume: powerupAudioVolume);

        pUp = powerUp;

        if (pUp == 3)
        {
            moveSpeed = slowSpeed;
        }

        Destroy(GameObject.FindGameObjectWithTag("Armor"));
        GameObject armor = Instantiate(prefab, transform);

        dbArmatureComponent = armor.GetComponent<DragonBones.UnityArmatureComponent>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && pUp > 0)
        {
            if (pUp > 0 && !justBounced)
            {
                antiInput = true;
                var sign = Mathf.Sign(velocity.x);
                directionalInput = Vector2.zero;
                justBounced = true;
                if (collision.gameObject.transform.position.x > transform.position.x && controller.collisions.right)
                {
                    dbArmatureComponent.animation.Play(bounceState);
                }
                else if (collision.gameObject.transform.position.x < transform.position.x) //&& controller.collisions.left)
                {
                    dbArmatureComponent.animation.Play(bounceState);
                }
                dbArmatureComponent.animation.timeScale = 0.5f;
                soundManager.PlayAudioClip(bumpAudioClip, volume: bumpAudioVolume);

                velocity.x = -20f * sign;

                if (velocity.y != 0)
                {
                    velocity.y = +15f;
                }
            }
            else if (pUp > 1 && !justDoubleBounced)
            {
                if (justBounced)
                {
                    justDoubleBounced = true;
                }
                antiInput = true;
                var sign = Mathf.Sign(velocity.x);
                directionalInput = Vector2.zero;
                justBounced = true;
                if (collision.gameObject.transform.position.x > transform.position.x && controller.collisions.right)
                {
                    dbArmatureComponent.animation.Play(bounceState);
                }
                else if (collision.gameObject.transform.position.x < transform.position.x && controller.collisions.left)
                {
                    dbArmatureComponent.animation.Play(bouncebackState);
                }
                dbArmatureComponent.animation.timeScale = 0.5f;
                soundManager.PlayAudioClip(bumpAudioClip, volume: bumpAudioVolume);

                velocity.x = -20f * sign;

                if (velocity.y != 0)
                {
                    velocity.y = +15f;
                }
            }
        }

        if (collision.gameObject.tag == "Checkpoint")
        {
            checkpoint = collision.transform;
        }
    }

    
    private IEnumerator DeathCoroutine(float delay)
    {
        dead = true;
        dbArmatureComponent.animation.Play(deathState);
		dbArmatureComponent.animation.timeScale = 0.5f;

        yield return new WaitForSeconds(delay);

        transform.position = checkpoint.position;

        dead = false;
    }

}
