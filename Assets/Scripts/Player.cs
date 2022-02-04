using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField] float      speed = 4.0f;
    [SerializeField] float      jumpForce = 7.5f;

    private Animator            darkAnimator;
    [SerializeField] private Animator            lightAnimator;
    [SerializeField] private SpriteRenderer      darkSprite;
    [SerializeField] private SpriteRenderer      lightSprite;
    private Rigidbody2D         body2d;
    private Sensor_Player       groundSensor;
    private bool                grounded = false;
    private bool                combatIdle = false;
    private bool                isDead = false;
    private int                 health;
    private int                 startingHealth;
    private int                 happiness;
    private int                 totalHappiness;

	private void Awake() {
        Game.Instance.Player = this;
	}

	// Use this for initialization
	void Start () {
        darkAnimator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Player>();

        health = startingHealth;
        totalHappiness = FindObjectsOfType<Happiness>().Length;

        SpriteRendererAlpha(lightSprite, happiness / totalHappiness);
        SpriteRendererAlpha(darkSprite, 1 - (happiness / totalHappiness));
    }
	
	// Update is called once per frame
	void Update () {
        //Check if character just landed on the ground
        if (!grounded && groundSensor.State()) {
            grounded = true;
            darkAnimator.SetBool("Grounded", grounded);
            lightAnimator.SetBool("Grounded", grounded);

        }

        //Check if character just started falling
        if(grounded && !groundSensor.State()) {
            grounded = false;
            darkAnimator.SetBool("Grounded", grounded);
            lightAnimator.SetBool("Grounded", grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

        //Set AirSpeed in animator
        darkAnimator.SetFloat("AirSpeed", body2d.velocity.y);
        lightAnimator.SetFloat("AirSpeed", body2d.velocity.y);

        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("e")) {
            if (!isDead) {
                darkAnimator.SetTrigger("Death");
                lightAnimator.SetTrigger("Death");
            } else {
                darkAnimator.SetTrigger("Recover");
                lightAnimator.SetTrigger("Recover");
            }

            isDead = !isDead;
        }

        /*//Hurt
        else if (Input.GetKeyDown("q"))
            m_animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0)) {
            m_animator.SetTrigger("Attack");
        }

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            m_combatIdle = !m_combatIdle;*/

        //Jump
        else if (Input.GetKeyDown("space") && grounded) {
            darkAnimator.SetTrigger("Jump");
            lightAnimator.SetTrigger("Jump");
            grounded = false;
            darkAnimator.SetBool("Grounded", grounded);
            lightAnimator.SetBool("Grounded", grounded);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon) {
            darkAnimator.SetInteger("AnimState", 2);
            lightAnimator.SetInteger("AnimState", 2);
        }

        //Combat Idle
        else if (combatIdle) {
            darkAnimator.SetInteger("AnimState", 1);
            lightAnimator.SetInteger("AnimState", 1);
        }

        //Idle
        else {
            darkAnimator.SetInteger("AnimState", 0);
            lightAnimator.SetInteger("AnimState", 0);
        }
    }

    public void UpdateHealth(int _delta) {
        health += _delta;        
	}

    public void IncrementHappiness() {
        happiness++;

        SpriteRendererAlpha(lightSprite, happiness / (float)totalHappiness);
        SpriteRendererAlpha(darkSprite, 1 - (happiness / (float)totalHappiness));

	}

    private void SpriteRendererAlpha(SpriteRenderer _renderer, float _alpha) {
        Color tmp = _renderer.color;
        tmp.a = _alpha;
        _renderer.color = tmp;
    }
}
