using UnityEngine;

public enum RandomSpawnerShape
{
    Box,
    Sphere,
}

// Used mostly for testing to provide stuff to fly around and into.
public class RandomAreaSpawner : MonoBehaviour
{
    [Header("General settings:")]

    [Tooltip("Prefab to spawn.")]
    public Transform prefab;

    [Tooltip("Shape to spawn the prefabs in.")]
    public RandomSpawnerShape spawnShape = RandomSpawnerShape.Sphere;

    [Tooltip("Multiplier for the spawn shape in each axis.")]
    public Vector3 shapeModifiers = Vector3.one;

    [Tooltip("How many prefab to spawn.")]
    public int asteroidCount = 50;

    [Tooltip("Distance from the center of the gameobject that prefabs will spawn")]
    public float range = 1000.0f;

    [Tooltip("Should prefab have a random rotation applied to it.")]
    public bool randomRotation = true;

    [Tooltip("Random min/max scale to apply.")]
    public Vector2 scaleRange = new Vector2(1.0f, 3.0f);

    [Header("Rigidbody settings:")]

    [Tooltip("Apply a velocity from 0 to this value in a random direction.")]
    public float velocity = 0.0f;

    [Tooltip("Apply an angular velocity (deg/s) from 0 to this value in a random direction.")]
    public float angularVelocity = 0.0f;

    [Tooltip("If true, raise the mass of the object based on its scale.")]
    public bool scaleMass = true;

    // Use this for initialization
    void Start()
    {
        if (prefab != null)
        {
            for (int i = 0; i < asteroidCount; i++)
                CreateAsteroid();
        }
    }

    private void CreateAsteroid()
    {
        Vector3 spawnPos = Vector3.zero;
         
        // Create random position based on specified shape and range.
        if (spawnShape == RandomSpawnerShape.Box)
        {
            spawnPos.x = Random.Range(-range, range) * shapeModifiers.x;
            spawnPos.y = Random.Range(-range, range) * shapeModifiers.y;
            spawnPos.z = Random.Range(-range, range) * shapeModifiers.z;
        }
        else if (spawnShape == RandomSpawnerShape.Sphere)
        {
            spawnPos = Random.insideUnitSphere * range;
            spawnPos.x *= shapeModifiers.x;
            spawnPos.y *= shapeModifiers.y;
            spawnPos.z *= shapeModifiers.z;
        }

        // Offset position to match position of the parent gameobject.
        spawnPos += transform.position;

        // Apply a random rotation if necessary.
        Quaternion spawnRot = (randomRotation) ? Random.rotation : Quaternion.identity;

        // Create the object and set the parent to this gameobject for scene organization.
        Transform t = Instantiate(prefab, spawnPos, spawnRot) as Transform;
        t.SetParent(transform);

        // Apply scaling.
        float scale = Random.Range(scaleRange.x, scaleRange.y);
        t.localScale = Vector3.one * scale;

        // Apply rigidbody values.
        Rigidbody r = t.GetComponent<Rigidbody>();
        if (r)
        {
            if (scaleMass)
                r.mass *= scale * scale * scale;

            r.AddRelativeForce(Random.insideUnitSphere * velocity, ForceMode.VelocityChange);
            r.AddRelativeTorque(Random.insideUnitSphere * angularVelocity * Mathf.Deg2Rad, ForceMode.VelocityChange);
        }
    }

    public void CreateNewAstroid()
    {
        CreateAsteroid();
    }
}
