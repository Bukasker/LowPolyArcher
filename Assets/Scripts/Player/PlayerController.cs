using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference movmentControl;
    [SerializeField]
    private InputActionReference jumpControl;
    [SerializeField]
    private InputActionReference Interaction;
    [SerializeField]
    private InputActionReference Crouch;
    [SerializeField]
    private InputActionReference EqipedBow;
    [SerializeField]
    private InputActionReference EqipedSword;
    [SerializeField]
    private InputActionReference Attack;
    [SerializeField]
    private InputActionReference InventoryOn;
    [SerializeField]
    public float playerSpeed;
    [SerializeField]
    private float jumpHeight = 2.0f;
    [SerializeField]
    private float gravityValue = -20.81f;
    [SerializeField]
    private float rotationSpeed = 200f;


    [SerializeField]
    public Slider slider;
    [SerializeField]
    private float Stamina = 0f;
    private float maxStamina = 30f;
    private CharacterController controller;
    [SerializeField]
    private Vector3 playerVelocity;

    private Transform cameraMainTransform;
    private Transform playerShotTranform;
    private Animator anim;
    bool isJumping;

    [SerializeField]
    bool isGrounded;
    [SerializeField] 
    private float groundCheckDistance;
    [SerializeField] 
    private LayerMask groundMask;
    public Transform GroundCheck;

    [SerializeField]
    public static bool isEpressed;
    [SerializeField]
    public bool isExhausted = false;
    [SerializeField]
    public bool InventoryIsOpen;

    [SerializeField]
    public bool isCrouched;
    public event EventHandler OnCrouchPressed;
    public event EventHandler OnInventoryPressed;
    public event EventHandler BowEquipped;
    public event EventHandler SwordEquipped;

    public GameObject bow;
    public GameObject arrow;
    public GameObject arrowBag;
    public GameObject EmptyArrowBag;
    public GameObject Sword;
    public GameObject Player;
    public GameObject test;
    private Transform bowTransform;
    private bool haveBow = true;
    private bool haveArrow = true;
    private bool haveSword = true;
    [SerializeField]
    private bool BowIsEquipped = false;
    [SerializeField]
    private bool SwordIsEquipped = false;
    [SerializeField]
    public float BowDistancePower;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject CanvasObject;
    [SerializeField] private GameObject CanvasObjectInventory;
    private void OnDisable()
    {
        InventoryOn.action.Disable();
        EqipedSword.action.Disable();
        EqipedBow.action.Disable();
        Crouch.action.Disable();
        Attack.action.Disable();
        Interaction.action.Disable();
        movmentControl.action.Disable();
        jumpControl.action.Disable();
    }
    private void OnEnable()
    {
        InventoryOn.action.Enable();
        EqipedSword.action.Enable();
        EqipedBow.action.Enable();
        Crouch.action.Enable();
        Attack.action.Enable();
        Interaction.action.Enable();
        movmentControl.action.Enable();
        jumpControl.action.Enable();
    }
    private void Start()
    {
        OnCrouchPressed += OnCrouch;
        BowEquipped += OnEquippedBow;
        SwordEquipped += OnEquippedSword;
        OnInventoryPressed += OnInventory;
        Stamina = maxStamina;
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cameraMainTransform = Camera.main.transform;
        playerShotTranform = Camera.main.transform;
    }

    void Update()
    {

        if (Crouch.action.triggered)
        {
            OnCrouchPressed?.Invoke(this, EventArgs.Empty);
        }
        if (EqipedBow.action.triggered)
        {
            BowEquipped?.Invoke(this, EventArgs.Empty);
            if (SwordIsEquipped)
            {
                SwordIsEquipped = false;
                BowIsEquipped = true;
            }
        }
        if (EqipedSword.action.triggered)
        {
            SwordEquipped?.Invoke(this, EventArgs.Empty);
            if (BowIsEquipped)
            {
                BowIsEquipped = false;
                SwordIsEquipped = true;
            }
        }
        if (InventoryOn.action.triggered)
        {
            OnInventoryPressed?.Invoke(this, EventArgs.Empty);
        }

        isGrounded = Physics.CheckSphere(GroundCheck.position, groundCheckDistance, groundMask);
        if (isGrounded && jumpControl.action.triggered)
        {
            if (isCrouched)
            {
                anim.SetBool("isMovingCrouched", false);
                anim.SetBool("isCrouching", false);
                isCrouched = false;
            }
            anim.SetBool("isFalling", false);
            anim.SetTrigger("isJumping");
            isJumping = true;
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);
            anim.SetBool("isFalling", false);
            if (BowIsEquipped)
            {
                anim.SetLayerWeight(anim.GetLayerIndex("BowArmDisArm"), 1);
            }
            if (SwordIsEquipped)
            {
                anim.SetLayerWeight(anim.GetLayerIndex("SwordLayer"), 1f);
            }
        }
        else
        {
            if (BowIsEquipped)
            {
                anim.SetLayerWeight(anim.GetLayerIndex("BowArmDisArm"), 0);
            }
            if (SwordIsEquipped)
            {
                anim.SetLayerWeight(anim.GetLayerIndex("SwordLayer"), 0);
            }
            anim.SetBool("isGrounded", false);
            playerVelocity.y += gravityValue * Time.deltaTime;
            if (((isJumping && playerVelocity.y < 0) || playerVelocity.y < -2)&& !isGrounded)
            {
                anim.SetBool("isFalling", true);
                isJumping = false;
            }
            else
            {
                anim.SetBool("isFalling", false);
            }
        }
        controller.Move(playerVelocity * Time.deltaTime);
        Vector2 movment = movmentControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movment.x, 0, movment.y);

        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);
        //PLAYER ROTATION
        if (movment != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movment.x, movment.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
        //MOVMENT
        if (Interaction.action.triggered)
        {
            isEpressed = true;
        }
        else
        {
            isEpressed = false;
        }

        if (Stamina > 30)
        {

            Stamina = 30;
        }
        else if (Stamina <-1)
        {
            Stamina =-1;
        }
        if (movment == Vector2.zero)
        {
            anim.SetBool("isMovingCrouched", false);
            if (!isCrouched)
            {
                Stamina += 10f * Time.deltaTime;
                slider.value = Stamina;
                rotationSpeed = 20f;
                anim.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
            }
            if (isCrouched)
            {
                anim.SetBool("isCrouching", true);
                Stamina += 10f * Time.deltaTime;
                slider.value = Stamina;
            }
            else
            {
                anim.SetBool("isCrouching", false);
            }
        }
        else if (movment != Vector2.zero)
        {

            if (isCrouched)
            {
                anim.SetBool("isMovingCrouched", true);
                playerSpeed = 3.0f;
                Stamina += 10f * Time.deltaTime;
                slider.value = Stamina;
            }
            if (!Keyboard.current.leftShiftKey.isPressed && !isCrouched)
            {
                if (Stamina >= 29)
                {
                    isExhausted = false;
                    Stamina += 10f * Time.deltaTime;
                    slider.value = Stamina;
                    playerSpeed = 5.0f;
                    anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
                    anim.SetBool("isMoving", true);
                }
                if (Stamina < 29 && !isExhausted)
                {
                    Stamina += 10f * Time.deltaTime;
                    slider.value = Stamina;
                    playerSpeed = 5.0f;
                    anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
                    anim.SetBool("isMoving", true);
                }
                else if(Stamina < 29 && isExhausted)
                {
                    Stamina += 5f * Time.deltaTime;
                    slider.value = Stamina;
                    playerSpeed = 3.0f;
                    anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
                    anim.SetBool("isMoving", true);
                }
            }
            else if (Keyboard.current.leftShiftKey.isPressed)
            {
                anim.SetBool("isShotPressed",false );
                arrow.SetActive(false);
                if (BowIsEquipped)
                {
                    anim.SetLayerWeight(anim.GetLayerIndex("BowArmDisArm"), 0);
                }
                if (isCrouched)
                {
                    anim.SetBool("isMovingCrouched", false);
                    anim.SetBool("isCrouching", false);
                    isCrouched = false;
                }
                if (Stamina > 0)
                {
                    Stamina -= 15f * Time.deltaTime;
                    slider.value = Stamina;
                    playerSpeed = 10.0f;
                    anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
                    anim.SetBool("isMoving", true);
                }
                else if(Stamina < 0)
                {
                    isExhausted = true;
                    Stamina -= 15f * Time.deltaTime;
                    slider.value = Stamina;
                    playerSpeed = 3.0f;
                    anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
                    anim.SetBool("isMoving", true);
                }

            }
        }
        
        if (!haveBow)
        {
            bow.SetActive(false);
            EmptyArrowBag.SetActive(false);
            arrowBag.SetActive(false);
            arrowBag.SetActive(false);
            EmptyArrowBag.SetActive(false);
            
        }
        if (haveBow)
        {
            if (haveArrow)
            {
                arrowBag.SetActive(true);
                EmptyArrowBag.SetActive(false);
                if (Mouse.current.leftButton.isPressed && BowIsEquipped)
                {
                    anim.SetBool("isShotPressed", true);
                    arrow.SetActive(true);
                    float targetAngle = Mathf.Atan2(movment.x, movment.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
                    Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
                    BowDistancePower += Time.deltaTime ;
                }
                else if (!Mouse.current.leftButton.isPressed)
                {
                    if (BowDistancePower >= 1f)
                    {
                        anim.SetBool("isShotPressed", false);
                        arrow.SetActive(false);
                        if(BowDistancePower >= 1.5f)
                        {
                            BowDistancePower = 5f;
                            arrow.SetActive(true);

                            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 2f));

                            var spawnedArrow = Instantiate(arrowPrefab, arrow.transform.position, arrow.transform.rotation);
                            var spawnedArrowRigidbody = spawnedArrow.GetComponent<Rigidbody>();

                            spawnedArrow.transform.right -= ray.direction; 
                            spawnedArrowRigidbody.AddForce(ray.direction * BowDistancePower, ForceMode.Impulse);
                            BowDistancePower = 0;
                        }
                    }
                    else
                    {
                        BowDistancePower = 0;
                        anim.SetBool("isShotPressed", false);
                        arrow.SetActive(false);
                    }
                }
            }
            if (!haveArrow)
            {
                arrowBag.SetActive(false);
                EmptyArrowBag.SetActive(true);
            }
            if (BowIsEquipped)
            {
                anim.SetBool("isEqupped",true);
                bow.SetActive(true);
            }
            if (!BowIsEquipped)
            {
                anim.SetBool("isEqupped", false);
                bow.SetActive(false);
            }
        }

        if (SwordIsEquipped)
        {
            anim.SetBool("isSwordEquiped", true);
            Sword.SetActive(true);
            if (Attack.action.triggered && !Keyboard.current.leftShiftKey.isPressed)
            {
                anim.SetTrigger("isAttackNormal");
                anim.SetLayerWeight(anim.GetLayerIndex("SwordLayer"), 1);
            }
            if (Attack.action.triggered && Keyboard.current.leftShiftKey.isPressed)
            {
                anim.SetTrigger("isAttackStrong");
                anim.SetLayerWeight(anim.GetLayerIndex("SwordLayer"), 1);
            }
        }
        else
        {
            anim.SetBool("isSwordEquiped", false);
            Sword.SetActive(false);
        }
        if (InventoryIsOpen)
        {
            DisableCanvas();
        }
        else
        {
            EnableCanvas();
        }
       
    }
    private void OnCrouch(object sender,EventArgs e)
    {
        isCrouched = !isCrouched;
    }
    private void OnEquippedBow(object sender, EventArgs e)
    {
        BowIsEquipped = !BowIsEquipped;
    }
    private void OnEquippedSword(object sender, EventArgs e)
    {
        SwordIsEquipped = !SwordIsEquipped;
    }
    private void OnInventory(object sender, EventArgs e)
    {
        InventoryIsOpen = !InventoryIsOpen;
    }
    void DisableCanvas()
    {

        CanvasObject.GetComponent<Canvas>().enabled = false;
        CanvasObjectInventory.GetComponent<Canvas>().enabled = true;
    }
    void EnableCanvas()
    {
        CanvasObject.GetComponent<Canvas>().enabled = true;
        CanvasObjectInventory.GetComponent<Canvas>().enabled =false;
    }
}