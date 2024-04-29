using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerStateContext : MonoBehaviour
{
    public event Action OnHittingObstableWidSheild;

    public static PlayerStateContext Instance { get; private set; }

    // State
    private P_BaseState _currentState = null;

    // Instantiating the Concrete States
    public P_RunState runState = new();
    public P_JumpState jumpState = new();
    public P_SlideState slideState = new();
    public P_DeadState deadState = new();

    #region Private Variables

    // References
    [SerializeField] private Animator Anim;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private PlayerEffects playerEffects;
    [SerializeField] private LayerMask groundCheckLayer;

    private float gravity = Physics.gravity.y * 2; // Setting Custom Gravity
    private float maxSpeed = 22f;
    private int choosenLane = 1;
    private Vector3 targetPosition;
    private float laneDistance = 2.5f;
    public bool isDead = false;

    [SerializeField] private string activeState; // For Viewing Only

    #endregion

    #region Public Variables

    //References
   

    public readonly int Jump = Animator.StringToHash("Jump");
    public readonly int Slide = Animator.StringToHash("Slide");
    public readonly int Dead = Animator.StringToHash("Dead");
    [HideInInspector] public bool isSliding;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float forwardSpeed = 14;
    [HideInInspector] public float JumpForce = 11;

    #endregion

    #region Properties

    public Animator Animator { get { return Anim; } }
    public CharacterController CharacterController { get { return characterController; } }
    public float Gravity { get { return gravity; } }
    public Transform GroundCheckPos { get { return groundCheckPos; } }
    public LayerMask GroundCheckLayer { get { return groundCheckLayer; } }
    public PlayerEffects PlayerEffects { get { return playerEffects; } }

    #endregion

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (!characterController.isGrounded)
               direction.y += gravity * 1.3f * Time.deltaTime;  // Make The Charac Grounded When Enters This State to CheckSlide cuz its not Grounded

        direction.z = forwardSpeed;

        // Subscribing Left & Right Switch Input Events
        Input_Manager.OnLeftSwitch += Event_OnLeftSwitch;
        Input_Manager.OnRightSwitch += Event_OnRightSwitch;


        _currentState = runState;
        _currentState.Enter(this);
    }
    private void Update()
    {
        #region Only Editor
#if UNITY_EDITOR

        activeState = _currentState.ToString();
#endif
        #endregion

        if (PlayerManager.Instance.gameState != PlayerManager.GameState.Playing) return; // Starting After a Tap On the Screen
        _currentState.Update(this);

        ChoosingLane(); // Chooses which Lane the Player is going
        SwitchingPlayerAlongPaths(); // Moves the player Towars the Choosen Lane
    }
    private void FixedUpdate()
    {
        if (PlayerManager.Instance.gameState != PlayerManager.GameState.Playing) return; // Starting After a Tap On the Screen
        MovingByCharController(); // Moving in Fixed Update cuz it has Collision Issues
    }
    #region Public Methods

    public void SwitchState(P_BaseState switchingState) // It Switches Between States
    {
        if(isDead) return;
        _currentState.Exit(this);
        _currentState = switchingState;// Exiting Already Existing State
        _currentState.Enter(this); // Enterning the Another State
    }   

    public void SpeedOverTime()
    {
        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.05f * Time.deltaTime;
        }
    }  // Increases Forward Speed Over Time

    #endregion

    #region Private Methods
    private void MovingByCharController() // Moving Character Forward Using Character Controller Move Func
    {
        if (isDead) return;
        characterController.Move(direction * Time.fixedDeltaTime);

    }

    private void SwitchingPlayerAlongPaths() // Only Teleports the character to the Target poss
    {
        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDire = diff.normalized * 50 * Time.deltaTime;

            if (!isDead)
            {
                if (moveDire.sqrMagnitude < diff.sqrMagnitude)
                {
                    characterController.Move(moveDire);
                }
                else
                {
                    characterController.Move(diff);
                }
            }
        }
    }
    private void ChoosingLane()
    {
        targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (choosenLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (choosenLane == 2)
        {
            
            targetPosition += Vector3.right * laneDistance;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
       CheckHit(hit);
    }

    private void SettingGameOverTrue()
    {
        PlayerManager.gameOver = true;
    }

    #endregion

    #region Event Methods
    private void Event_OnRightSwitch()  // Adding Lane Value
    {
        choosenLane++;
        if (choosenLane == 3)
        {
            choosenLane = 2;
        }
    }
    private void Event_OnLeftSwitch() // Substracting Lane Value
    {
        choosenLane--;
        if (choosenLane == -1)
        {
            choosenLane = 0;
        }
    }

    private void CheckHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Obstacle"))
        {
            hit.collider.isTrigger = true;
            if(PowerUpManager.instance.HasPowerUp && PowerUpManager.instance.PowerUpInt == 1)
            {
                OnHittingObstableWidSheild?.Invoke();
                return;
            }
            SwitchState(deadState);
            Invoke(nameof(SettingGameOverTrue), 1.2f);
        }
        else if (hit.transform.CompareTag("Satellite"))
        {
            hit.collider.isTrigger = true;
            if (_currentState == slideState) return;
            if (PowerUpManager.instance.HasPowerUp && PowerUpManager.instance.PowerUpInt == 1)
            {
                OnHittingObstableWidSheild?.Invoke();
                // Break Sheild
                return;
            }
            SwitchState(deadState);
            Invoke(nameof(SettingGameOverTrue), 1.2f);
        }
    }

    #endregion

    #region Animation Events
    public void StopSlide() 
    {
        isSliding = false;
    }
    public void EndJump()
    {
        isJumping = false;
    }

    #endregion

}
