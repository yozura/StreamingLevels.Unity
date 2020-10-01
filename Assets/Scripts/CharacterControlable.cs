using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlable : Controlable
{
    public Transform characterBody;

    Animator animator;
    Rigidbody rigidbody;

    public float jumpForce = 5f;

    public override void Interact()
    {
        RaycastHit hit;
        if(Physics.Raycast(new Ray(cameraArm.position + cameraArm.forward, cameraArm.forward), out hit, 1f))
        {
            if(hit.collider.CompareTag("Vehicle"))
            {
                var vehicle = hit.collider.gameObject.GetComponent<VehicleControlable>();
                StartCoroutine(Ride(vehicle));
            }
        }
    }

    private IEnumerator Ride(VehicleControlable vehicle)
    {
        var controller = FindObjectOfType<Controller>();
        var charBody = GetComponent<Rigidbody>();
        var charCollider = GetComponent<CapsuleCollider>();
        charBody.isKinematic = true;
        charCollider.isTrigger = true;

        while (Vector3.Distance(transform.position, vehicle.ridePosition.position) > 0.1f)
        {
            yield return null;
            transform.position += (vehicle.ridePosition.position - transform.position).normalized * Time.deltaTime * 3f;
        }

        vehicle.InteractDoor(true);
        yield return new WaitForSeconds(0.5f);

        while(Vector3.Distance(transform.position, vehicle.characterSeat.position) > 0.1f)
        {
            yield return null;
            transform.position += (vehicle.characterSeat.position - transform.position).normalized * Time.deltaTime * 3f;
        }

        vehicle.driveCharacter = this;
        transform.SetParent(vehicle.characterSeat);

        controller.ChangeControlTarget(this, vehicle);
        vehicle.InteractDoor(false);
    }


    public override void Jump()
    {
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public override void Move(Vector2 input)
    {
        animator.SetFloat("MoveSpeed", input.magnitude);
        Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
        Vector3 moveDir = lookForward * input.y + lookRight * input.x;

        if (input.magnitude != 0)
        {
            characterBody.forward = lookForward;
            transform.position += moveDir * Time.deltaTime * 5f;
        }
    }

    public override void Rotate(Vector2 input)
    {
        if (cameraArm != null)
        {
            Vector3 camAngle = cameraArm.rotation.eulerAngles;
            float x = camAngle.x - input.y;

            if (x < 180f)
            {
                x = Mathf.Clamp(x, -1f, 70f);
            }
            else
            {
                x = Mathf.Clamp(x, 335f, 361f);
            }

            // 카메라 암 회전 시키기
            cameraArm.rotation = Quaternion.Euler(x, camAngle.y + input.x, camAngle.z);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = characterBody.GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
