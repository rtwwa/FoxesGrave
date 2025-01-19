using System;
using UnityEngine;

public class CoinRotation : MonoBehaviour, IInteractable
{
    [SerializeField] CoinType coinType;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject outlineModel;
    public void Interact()
    {
        CoinManager.Instance.UnlockAbility(coinType);
        this.gameObject.SetActive(false);
        Destroy(model);
        Destroy(outlineModel);
        Destroy(this);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0f, rotationSpeed * Time.deltaTime, 0f), Space.World);
    }

    public void ShowOutline()
    {
        outlineModel.SetActive(true);
    }

    public void HideOutline()
    {
        outlineModel.SetActive(false);
    }
}