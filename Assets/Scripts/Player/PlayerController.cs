using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Animator animator;
    [SerializeField] private float turnSmoothVelocity;
    [SerializeField] private float turnSmoothTime;
    [SerializeField] private float speed= 6f;
    private Vector3 offset;
    [SerializeField] public float cameraSmoothSpeed=0.5f;

    //TODO remove attack from player
    public float maxHealth =100f;
    public float maxDarkness = 100f;
    public float encroachAmount = 1f;
    public float encroachRate = 1f;
    [SerializeField] float currentDarkness;
    [SerializeField] float currentHealth;
    [SerializeField] bool isStill=false;
    public ThirdPersonCharacter character;
    public Transform characterTransform;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        offset = cam.position - characterTransform.position;
        currentDarkness = 0f;
        currentHealth = maxHealth;
        StartCoroutine(EncroachWithDelay());
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

        if (dir.magnitude >= 0.1f)
        {
            
            
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 newPos = transform.position + moveDir.normalized * speed * Time.deltaTime;

            NavMeshHit hit;
            bool isValid = NavMesh.SamplePosition(newPos, out hit, .3f, NavMesh.AllAreas);
            if (isValid)
            {
                //controller.Move(moveDir.normalized * speed * Time.deltaTime);
                character.Move(moveDir.normalized,false,false);
                //transform.position = hit.position;
                //animator.SetTrigger("Run");
                //animator.ResetTrigger("Idle");

            }
        }
        else
        {
            //animator.SetTrigger("Idle");
            //animator.ResetTrigger("Run");
            character.Move(Vector3.zero, false, false);
        }

    }

    public void updateEncroach(bool still,float rate)
    {
        isStill = still;
        encroachRate = rate;
    }

    IEnumerator EncroachWithDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(encroachRate);
             currentDarkness += encroachAmount;
        }
        
    } 



    public void Damage(float dmg)
    {
        currentHealth -= dmg;
        //Play dmg animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Die animation
        Debug.Log("Dead");
    }



    void LateUpdate()
    {
        Vector3 desiredPosition = characterTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(cam.position, desiredPosition, cameraSmoothSpeed);
        cam.position = smoothedPosition;
       
    }
}
