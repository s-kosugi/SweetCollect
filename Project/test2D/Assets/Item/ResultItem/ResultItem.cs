using UnityEngine;

public class ResultItem : MonoBehaviour
{
    [SerializeField] float rotateSpeedMin = 120.0f;
    [SerializeField] float rotateSpeedMax = 360.0f;
    float rotationSpeed = 0f;
    float rotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = Random.Range(rotateSpeedMin,rotateSpeedMax);
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f,360.0f));
    }

    // Update is called once per frame
    void Update()
    {
        // Z軸回転
        rotation += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, rotation);

    }
}
