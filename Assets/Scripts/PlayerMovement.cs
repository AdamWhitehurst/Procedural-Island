using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed;

    private Rigidbody2D rb;
    // private Animator anmtr;

    // Start is called before the first frame update
    void Start() {
        // anmtr = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update() {
        Vector3 newVelocity = GetInputs();
        // Move in space
        if (newVelocity != Vector3.zero) {
            MoveCharacter(newVelocity, speed);
        }
        // Update Animation State
        // UpdateAnimatorMovement(newVelocity);
    }

    Vector3 GetInputs() {
        Vector3 vel = Vector3.zero;
        vel.x = Input.GetAxisRaw("Horizontal");
        vel.y = Input.GetAxisRaw("Vertical");
        return vel;
    }

    // Movement callable from other Components
    void MoveCharacter(Vector3 velocity, float speed) {
        rb.MovePosition(
            transform.position + (velocity * speed * Time.deltaTime)
          );
    }

    // void UpdateAnimatorMovement(Vector3 movement) {
    //     if (movement != Vector3.zero) {
    //         anmtr.SetBool("moving", true);
    //         if (movement.x != 0) {
    //             anmtr.SetFloat("moveX", movement.x);
    //         }
    //     } else {
    //         anmtr.SetBool("moving", false);
    //     }
    // }

}
