using System;
using System.Collections;
using UnityEngine;

public enum PlayerState
{
    Hero,
    Spirit,
    Both
}


public class PlayerMovement : MonoBehaviour
{
    // Синглтон
    public static PlayerMovement Instance { get; private set; }

    // Все для обработки ивентов от Interactable объектов
    private Vector3 lastInteractDirection;
    private IInteractable lastInteractableObject;
    private Vector3 interactionBoxSize = new Vector3(1.5f, 1.75f, 2f); // Размер области взаимодействия
    private Vector3 interactionBoxOffset = new Vector3(0f, 0.75f, 1.25f); // Смещение области относительно игрока

    // Параметры для перемещения игрока
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameInput gameInput;
    private bool isWalking = false;

    // Для isGrounded, настроить потом под готовую модельку
    [SerializeField] private CapsuleCollider capsuleCollider;
    public CapsuleCollider GetCapsuleCollider() { return capsuleCollider; } // Этого тут нет
    private float playerRadius;
    private float playerHeight;
    private float groundCheckDistance = 0.1f;
    private float verticalVelocity = 0f;
    private bool isGrounded = true;

    // Модельки для необходимые для работы SpiritFlip
    [SerializeField] private GameObject pirateModel;
    [SerializeField] private GameObject spiritModel;
    [SerializeField] private GameObject fakePirateModel;
    private Vector3 coordinatesBeforeFlip;
    public bool isSpirit { get; private set; }
    public PlayerState playerState { get; private set; }

    // Слои для игнорирования призраком 
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask spiritIgnoreLayerMask;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one PlayerMovement instance");
        }

        Instance = this;
    }

    private void Start()
    {
        playerRadius = capsuleCollider.radius;
        playerHeight = capsuleCollider.height;

        gameInput.OnJumpAction += GameInput_OnJumpAction;
        gameInput.OnFlipAction += GameInput_OnFlipAction;
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnFlipAngelsState += GameInput_OnFlipAngelsState;
        gameInput.OnFlipGunslingerCurse += GameInput_OnFlipGunslingerCurse;
        gameInput.OnFlipLoversBlessing += GameInput_OnFlipLoversBlessing;
        gameInput.OnFlipSwordsMan += GameInput_OnFlipSwordsMan;
    }
    private void GameInput_OnFlipAngelsState(object sender, System.EventArgs e)
    {
        CoinManager.Instance.UseAbility(CoinType.AngelsState);
    }
    private void GameInput_OnFlipGunslingerCurse(object sender, System.EventArgs e)
    {
        CoinManager.Instance.UseAbility(CoinType.GunslingerCurse);
    }
    private void GameInput_OnFlipLoversBlessing(object sender, System.EventArgs e)
    {
        CoinManager.Instance.UseAbility(CoinType.LoversBlessing);
    }
    private void GameInput_OnFlipSwordsMan(object sender, System.EventArgs e)
    {
        CoinManager.Instance.UseAbility(CoinType.SwordsMan);
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        HandleMovement();
        HandleInteraction();
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (lastInteractableObject != null)
        {
            lastInteractableObject.Interact();
        }
    }

    private void HandleInteraction()
    {
        // Позиция и направление проверки
        Vector3 boxCenter = transform.position + transform.forward * interactionBoxOffset.z + transform.up * interactionBoxOffset.y;

        // Проверка коллизий с объектами
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, interactionBoxSize * 0.5f, transform.rotation);
        bool foundInteractable = false;

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent<IInteractable>(out IInteractable interactableComponent))
            {
                // Если найден объект для взаимодействия
                if (lastInteractableObject != interactableComponent)
                {
                    if (lastInteractableObject != null)
                    {
                        lastInteractableObject.HideOutline();
                    }

                    lastInteractableObject = interactableComponent;
                    interactableComponent.ShowOutline();
                }

                foundInteractable = true;
                break;
            }
        }

        if (!foundInteractable && lastInteractableObject != null)
        {
            lastInteractableObject.HideOutline();
            lastInteractableObject = null;
        }
    }

    // Визуализация Capsule для isGround в Scene (уже не работает нихуя, хуйню рисует, надо другой метод вызывать, но в целом я протестил все работает так что пох)
    //private void OnDrawGizmos()
    //{
    //    if (capsuleCollider == null) return;

    //    Vector3 colliderCenter = capsuleCollider.center;

    //    // Вычисляем верх и низ капсулы
    //    Vector3 top = transform.position + colliderCenter + Vector3.up * (playerHeight / 2f);  // Верхний конец капсулы
    //    Vector3 bottom = transform.position + colliderCenter + Vector3.down * (playerHeight / 2f);  // Нижний конец капсулы

    //    Gizmos.color = Color.red;

    //    // Отображаем CapsuleCast, который используется для проверки на землю
    //    Gizmos.DrawWireSphere(top, playerRadius);
    //    Gizmos.DrawWireSphere(bottom, playerRadius);

    //    // Отображаем линию между концами капсулы
    //    Gizmos.DrawLine(top, bottom);
    //}

    // Визуализация территории для Interactable в Scene
    // Хз, мб будет багать, но raycast вообще нахуй мажет, их надо сто штук
    //private void OnDrawGizmos()
    //{
    //    // Визуализация области взаимодействия
    //    Gizmos.color = Color.red;

    //    Vector3 boxCenter = transform.position + transform.forward * interactionBoxOffset.z + transform.up * interactionBoxOffset.y;
    //    Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
    //    Gizmos.DrawWireCube(Vector3.zero, interactionBoxSize);
    //}
    private bool IsGrounded()
    {
        RaycastHit hit;
        float checkDistance = 0.1f;  // Расстояние для проверки земли

        // Выстреливаем луч вниз от позиции игрока, чтобы проверить, есть ли земля под ним
        if (Physics.Raycast(transform.position, Vector3.down, out hit, checkDistance))
        {
            return true;
        }

        return false;
    }


    private void GameInput_OnJumpAction(object sender, EventArgs e)
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), rb.velocity.z);
        }
    }

    private void GameInput_OnFlipAction(object sender, EventArgs e)
    {
        if (!CoinManager.Instance.GetCoin(CoinType.SpiritFlip).IsUnlocked)
            return;

        if (!isGrounded && playerState == PlayerState.Hero)
            return;

        if (!spiritModel.activeSelf)
        {
            isSpirit = true;
            playerState = PlayerState.Spirit;
            coordinatesBeforeFlip = transform.position;
        }

        fakePirateModel.gameObject.transform.rotation = Quaternion.identity;
        fakePirateModel.gameObject.transform.position = coordinatesBeforeFlip;

        if (spiritModel.activeSelf)
        {
            StartCoroutine(SmoothReturnToPosition(coordinatesBeforeFlip, 0.4f));
            isSpirit = false;
            playerState = PlayerState.Hero;
        }
        else
        {
            fakePirateModel.SetActive(!fakePirateModel.activeSelf);
            pirateModel.SetActive(!pirateModel.gameObject.activeSelf);
            spiritModel.SetActive(!spiritModel.gameObject.activeSelf);
            isSpirit = true;
            playerState = PlayerState.Spirit;
        }

        Debug.Log($"You are {playerState}");
    }

    private IEnumerator SmoothReturnToPosition(Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = t * t * t * (5 - 2 * t);
            transform.position = Vector3.Lerp(startingPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;

        fakePirateModel.SetActive(!fakePirateModel.activeSelf);
        pirateModel.SetActive(!pirateModel.gameObject.activeSelf);
        spiritModel.SetActive(!spiritModel.gameObject.activeSelf);
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;

        Vector3 colliderCenter = transform.position + capsuleCollider.center;

        Vector3 top = colliderCenter + Vector3.up * (capsuleCollider.height / 2f - capsuleCollider.radius);
        Vector3 bottom = colliderCenter - Vector3.up * (capsuleCollider.height / 2f - capsuleCollider.radius);

        bool canMove = false;
        bool isSpiritModeActive = spiritModel.activeSelf;

        if (isSpiritModeActive)
        {
            // Объединяем playerLayerMask и SpiritIgnoreThis в одну маску
            LayerMask combinedMask = playerLayerMask | spiritIgnoreLayerMask;
            canMove = !Physics.CapsuleCast(top, bottom, capsuleCollider.radius, moveDirection, moveDistance, ~combinedMask);
            Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player") | LayerMask.NameToLayer("SpiritIgnoreThis"), true);
        }
        else
        {
            // Если isSpiritModeActive не активен, используем только playerLayerMask
            canMove = !Physics.CapsuleCast(top, bottom, capsuleCollider.radius, moveDirection, moveDistance, ~playerLayerMask);
            Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), true);
        }


        if (!canMove)
        {
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(top, bottom, capsuleCollider.radius, moveDirectionX, moveDistance, ~playerLayerMask);

            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(top, bottom, capsuleCollider.radius, moveDirectionZ, moveDistance, ~playerLayerMask);

                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }

        isWalking = moveDirection != Vector3.zero;

        transform.position = new Vector3(transform.position.x, transform.position.y + verticalVelocity * Time.deltaTime, transform.position.z);
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
}
