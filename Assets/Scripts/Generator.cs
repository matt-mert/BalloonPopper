using System.Collections;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject balloon1;
    [SerializeField] private GameObject balloon2;
    [SerializeField] private GameObject balloon3;
    [SerializeField] private GameObject balloon4;
    [SerializeField] private GameObject balloon5;

    [Header("DifficultyByRate")]
    [SerializeField] private float initialRate;
    [SerializeField] private float minimumRate;
    [SerializeField] private float changeRateDelay;
    [SerializeField] private float changeRateAmount;

    [Header("DifficultyByAccel")]
    [SerializeField] private float initialAccel;
    [SerializeField] private float maximumAccel;
    [SerializeField] private float changeAccelDelay;
    [SerializeField] private float changeAccelAmount;

    private WaitForSeconds waitGenerateInterval;
    private WaitForSeconds waitChangeRateInterval;
    private WaitForSeconds waitChangeAccelInterval;
    private float currentRate;
    private float currentAccel;
    private float generateHeight = 50f;

    private void Awake()
    {
        currentRate = initialRate;
        currentAccel = initialAccel;
        waitGenerateInterval = new WaitForSeconds(currentRate);
        waitChangeRateInterval = new WaitForSeconds(changeRateDelay);
        waitChangeAccelInterval = new WaitForSeconds(changeAccelDelay);
    }

    // Randomly determining the instantiation position and rotation of the prefab.
    // Instantiating a random balloon prefab and assigning the particles prefab that
    // we have to the public variable particle in the Balloon class of the newly
    // instantiated balloon object.
    private void Generate()
    {
        float x = Random.Range(-36, +36);
        float y = generateHeight;
        float z = Random.Range(-4, -36);
        float p = Random.Range(0, 360);
        float q = Random.Range(0, 360);
        float r = Random.Range(0, 360);
        Vector3 point = new Vector3(x, y, z);
        Quaternion rotation = Quaternion.Euler(new Vector3(p, q, r));

        int id = Random.Range(1, 6);
        GameObject instantiatedObject;

        switch (id)
        {
            case 1:
                instantiatedObject = Instantiate(balloon1, point, rotation);
                instantiatedObject.GetComponent<Balloon>().particle = particles;
                break;
            case 2:
                instantiatedObject = Instantiate(balloon2, point, rotation);
                instantiatedObject.GetComponent<Balloon>().particle = particles;
                break;
            case 3:
                instantiatedObject = Instantiate(balloon3, point, rotation);
                instantiatedObject.GetComponent<Balloon>().particle = particles;
                break;
            case 4:
                instantiatedObject = Instantiate(balloon4, point, rotation);
                instantiatedObject.GetComponent<Balloon>().particle = particles;
                break;
            case 5:
                instantiatedObject = Instantiate(balloon5, point, rotation);
                instantiatedObject.GetComponent<Balloon>().particle = particles;
                break;
        }
    }

    // Calling the Generate method at certain intervals.
    public IEnumerator BalloonGenerator()
    {
        while (true)
        {
            Generate();
            yield return waitGenerateInterval;
        }
    }

    // Decreasing the generation interval value at certain intervals.
    // Once the minimum value is reached, we stop this coroutine.
    public IEnumerator ChangeDifficultyByRate()
    {
        while (true)
        {
            if (currentRate > minimumRate)
            {
                currentRate -= changeRateAmount;
                waitGenerateInterval = new WaitForSeconds(currentRate);
            }
            else
            {
                StopCoroutine(ChangeDifficultyByRate());
            }

            yield return waitChangeRateInterval;
        }
    }

    // Increasing the falling speed value at certain intervals.
    // Once the maximum value is reached, we stop this coroutine.
    public IEnumerator ChangeDifficultyByAccel()
    {
        while (true)
        {
            if (currentAccel < maximumAccel)
            {
                currentAccel += changeAccelAmount;
                Physics.gravity = new Vector3(0, -1 * currentAccel, 0);
            }
            else
            {
                StopCoroutine(ChangeDifficultyByAccel());
            }
            
            yield return waitChangeAccelInterval;
        }
    }
}
