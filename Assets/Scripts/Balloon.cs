using UnityEngine;

public class Balloon : MonoBehaviour
{
    // Public variable for the particle prefab which is going to be assigned by the Generator.
    [HideInInspector] public GameObject particle;

    // Boom method is going to be called when the user presses on the corresponding balloon.
    // Changing the material of the particles to the material of the corresponding balloon.
    // Instantiating the particle effect and immediately destroying the corresponding balloon.
    public void Boom()
    {
        ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer>();
        Material material = GetComponent<MeshRenderer>().sharedMaterial;
        renderer.sharedMaterial = material;

        Gameplay.currentScore++;

        Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Detecting when the balloon hits the ground by utilizing tags.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Debug.Log("Touched To Ground");
        }
    }
}
