using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class PlatformController : RaycastController {

    [Header("Moving")]
    public PlatformWaypoint currentWaypoint;
    public float maxSpeed;
    public float accelerationDistance;
    public float decelerationDistance;
    public float waitTime;
    [SerializeField]
    public Vector2 speed = Vector2.zero;


    [Header("Crumbling")]
    public float crumbleTime;
    public float restoreTime;
    public bool onlyPlayerCrumble;
    public float currentWaitTime = 0;
    public float currentCrumbleTime = 0;
    public float currentRestoreTime = 0;
    public bool crumbled = false;
    public bool crumbling = false;

    public List<PassengerMovement> passengerMovement;
    public Dictionary<Transform, ObjectController2D> passengerDictionary = new Dictionary<Transform, ObjectController2D>();
    private Animator animator;
    private Collider2D myCollider;
    private PhysicsConfig pConfig;

    private static readonly string ANIMATION_CRUMBLING = "crumbling";
    private static readonly string ANIMATION_CRUMBLE = "crumble";
    private static readonly string ANIMATION_RESTORE = "restore";

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        pConfig = GameObject.FindObjectOfType<PhysicsConfig>();
        if (!pConfig) {
            pConfig = (PhysicsConfig)new GameObject().AddComponent(typeof(PhysicsConfig));
            pConfig.gameObject.name = "Physics Config";
            Debug.LogWarning("PhysicsConfig not found on the scene! Using default config.");
        }
    }

    private void Update()
    {
        UpdateRaycastOrigins();
        // Add passengers


        //If crumbled count to restore
        if (crumbleTime > 0) {
            if (crumbled) {
                //Count Restore Time

                // If restore time done restore
                if (currentRestoreTime > 0) {
                    currentRestoreTime -= Time.deltaTime;
                    if (currentRestoreTime <= 0) {
                        crumbled = false;
                        crumbling = false;

                        myCollider.enabled = true;
                        animator.SetTrigger(ANIMATION_RESTORE);
                    }
                }
                return;
            } else if (crumbling) {
                if (currentCrumbleTime > 0) {
                    currentCrumbleTime -= Time.deltaTime;
                } else if (currentCrumbleTime <= 0) {
                    crumbled = true;
                    crumbling = false;
                    animator.SetTrigger(ANIMATION_CRUMBLE);
                    myCollider.enabled = false;
                    currentRestoreTime = restoreTime;
                }
            }
        }
        Vector2 velocity = Vector2.zero;
        Vector2 distance = Vector2.zero;
        Vector3 newPos = transform.position;
        if (currentWaypoint) {
            if (currentWaitTime > 0) {
                currentWaitTime -= Time.deltaTime;
                return;
            }
            distance = currentWaypoint.transform.position - transform.position;
            if (distance.magnitude <= decelerationDistance) {
                if (distance.magnitude > 0) {
                    speed -= Time.deltaTime * distance.normalized * maxSpeed * maxSpeed /
                        (2 * decelerationDistance);
                } else {
                    speed = Vector2.zero;
                }
            } else if (speed.magnitude < maxSpeed) {
                if (accelerationDistance > 0) {
                    speed += Time.deltaTime * distance.normalized * maxSpeed * maxSpeed /
                        (2 * accelerationDistance);
                }
                if (speed.magnitude > maxSpeed || accelerationDistance <= 0) {
                    speed = distance.normalized * maxSpeed;
                }
            }
            newPos = Vector2.MoveTowards(transform.position, currentWaypoint.transform.position,
                speed.magnitude * Time.deltaTime);
            velocity = newPos - transform.position;
            //transform.position = newPos;
            if (distance.magnitude < 0.00001f) {
                speed = Vector2.zero;
                currentWaypoint = currentWaypoint.nextWaipoint;
                currentWaitTime = waitTime;
            }
        }

        // Move Passengers
        CalculatePassengerMovement(velocity);

        MovePassengers(true);
        transform.position = newPos;
        //transform.Translate(velocity);
        MovePassengers(false);
    }

    void MovePassengers(bool beforeMovePlatform)
    {
        foreach (PassengerMovement passenger in passengerMovement) {
            if (!passengerDictionary.ContainsKey(passenger.transform)) {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<ObjectController2D>());
            }

            if (passenger.moveBeforePlatform == beforeMovePlatform) {
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }

    void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // Non-Moving Platform
        // Skip if we are already crumbling
        if(velocity.magnitude == 0f && !crumbling && !crumbled) {
            //See if someone is on top
            float rayLength = skinWidth * 2;
            for (int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, pConfig.passengerMask);
                if (hit && hit.distance != 0) {
                    if (crumbleTime > 0 && currentCrumbleTime <= 0) {
                        if ((onlyPlayerCrumble && pConfig.characterMask == (pConfig.characterMask | (1 << hit.transform.gameObject.layer))) ||
                                pConfig.passengerMask == (pConfig.passengerMask | (1 << hit.transform.gameObject.layer))) {
                            currentCrumbleTime = crumbleTime;
                            crumbling = true;
                            animator.SetTrigger(ANIMATION_CRUMBLING);
                        }
                    }
                }
            }
        }

        // Vertically moving platform
        if (velocity.y != 0) {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, pConfig.passengerMask);

                if (hit && hit.distance != 0) {
                    if (!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        // Horizontally moving platform
        if (velocity.x != 0) {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++) {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, pConfig.passengerMask);

                if (hit && hit.distance != 0) {
                    if (!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }
            }
        }

        // Passenger on top of a horizontally or downward moving platform
        if (directionY == -1 || velocity.y == 0 && velocity.x != 0) {
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, pConfig.passengerMask);

                if (hit && hit.distance != 0) {
                    if (!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }
    }


    public struct PassengerMovement {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

    void OnDrawGizmos()
    {
        
    }
}