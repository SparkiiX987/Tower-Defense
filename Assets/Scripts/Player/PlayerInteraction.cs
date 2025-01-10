using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactible;
    [SerializeField] private GameObject interactionMenu;
    [SerializeField] private StatDisplayer statDisplayer;

    [Header("Inputs"), HideInInspector]
    private InputSystem_Actions inputs;
    private InputAction mouse;

    private void Awake()
    {
        inputs = new();
        mouse = inputs.Player.Interact;
    }

    private void OnEnable()
    {
        mouse.Enable();
    }

    private void OnDisable()
    {
        mouse.Disable();
    }

    private void Start()
    {
        mouse.started += Interact;
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        RaycastHit2D hit = Physics2D.Raycast(GetMouseWolrdPos(), Vector2.zero, 10, interactible);
        if (hit.collider)
        {
            if (hit.collider.CompareTag("NoStats")) { return; }

            if (hit.collider.CompareTag("Tower"))
            {
                interactionMenu.SetActive(true);
                interactionMenu.transform.position = Camera.main.WorldToScreenPoint(hit.transform.position);
                interactionMenu.GetComponent<TowerConstructer>().Initialize(hit.transform.GetComponent<TowerBehaviour>());
                Tower towerData = hit.collider.GetComponent<TowerBehaviour>().GetTowerData();

                if (towerData.Name == "Base") { return; }

                statDisplayer.gameObject.SetActive(true);
                statDisplayer.ResetData();
                statDisplayer.SetData(towerData);
            }
            else
            {
                AIStats enemie = hit.collider.transform.GetComponent<AIStats>();
                statDisplayer.gameObject.SetActive(true);
                statDisplayer.ResetData();
                statDisplayer.SetData(enemie);
                return;
            }

            return;
        }

        if (statDisplayer.gameObject.activeInHierarchy)
        {
            statDisplayer.ResetData();
            statDisplayer.gameObject.SetActive(false);
        }
        interactionMenu.SetActive(false);
    }

    public static Vector2 GetMouseWolrdPos()
    {
        return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
