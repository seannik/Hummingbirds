using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a Single Flower with nectar
/// </summary>

public class Flower : MonoBehaviour
{
    [Tooltip("Colour of a Full Flower")]
    public Color fullFlowerColor = new Color(1f, 0f, 0.3f);

    [Tooltip("Colour of an Empty Flower")]
    public Color emptyFlowerColor = new Color(0.5f, 0f, 1f);

    /// <summary>
    /// The trigger collider representing the nectar.
    /// </summary>
    [HideInInspector]
    public Collider nectarCollider;

    //Solid collider representing flower petals
    private Collider flowerCollider;

    //Flowers material
    private Material flowerMaterial;

    /// <summary>
    /// A Vector3 pointing up out of flower
    /// </summary>
    public Vector3 FlowerUpVector    {
        get
        {
            return nectarCollider.transform.up;
        }
    }

    /// <summary>
    /// The center of nectarCollider.
    /// </summary>
    public Vector3 FlowerCenterPosition
    {
        get
        {
            return nectarCollider.transform.position;
        }
    }

    /// <summary>
    /// The amount of remaining nectar
    /// </summary>
    public float NectarAmount { get; private set; }

    /// <summary>
    /// Checks if there is any Nectar left
    /// </summary>
    public bool HasNectar
    {
        get
        {
            return NectarAmount > 0f;
        }
    }

    /// <summary>
    /// Attempt to remove Nectar from the flower
    /// </summary>
    /// <param name="amount"> The amount of nectar to remove</param>
    /// <returns>The amount successfully removed</returns>
    public float Feed(float amount)
    {
        //track nectar consumption, limits to amount availiable
        float nectarTaken = Mathf.Clamp(amount, 0f, NectarAmount);

        //Subtract taken nectar
        NectarAmount -= amount;

        //could use HasNectar
        if (NectarAmount <= 0)
        {
            //No nectar left
            NectarAmount = 0;

            //Disable flower collider, makes it easier for ML-Agent
            flowerCollider.gameObject.SetActive(false);
            nectarCollider.gameObject.SetActive(false);

            // Change color to empty
            flowerMaterial.SetColor("_BaseColor", emptyFlowerColor);

        }

        // Return amount of consumed nectar
        return nectarTaken;

    }

    /// <summary>
    /// Resets the Flower
    /// </summary>

    public void ResetFlower()
    {
        //Refill the nectar
        NectarAmount = 1f;

        //Re-enable the flower colliders
        flowerCollider.gameObject.SetActive(true);
        nectarCollider.gameObject.SetActive(true);

        //Return flower to full color
        flowerMaterial.SetColor("_BaseColor", fullFlowerColor);
    }

    /// <summary>
    /// Called when the flower is woken
    /// </summary>
    private void Awake()
    {
        // Find flowers mesh renderer and material
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        flowerMaterial = meshRenderer.material;

        //Find coliders
        flowerCollider = transform.Find("FlowerCollider").GetComponent<Collider>();
        nectarCollider = transform.Find("FlowerNectarCollider").GetComponent<Collider>();

    }
}
