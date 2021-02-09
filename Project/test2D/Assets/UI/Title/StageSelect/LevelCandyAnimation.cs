using UnityEngine;
using UnityEngine.UI;

public class LevelCandyAnimation : MonoBehaviour
{
    [SerializeField] float AnimeSpeed = 2.0f;
    [SerializeField] float ScaleRate = 1.0f;
    private float Angle = 0.0f;
    private Image image = default;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        // サインカーブでα値を変える(0.0～1.0)
        Angle += AnimeSpeed;
        float scale = Mathf.Sin(Angle * Mathf.Deg2Rad);

        if (scale >= 0.5f)
        {
            Vector3 newScale = new Vector3((scale - 0.5f)*ScaleRate + 1.0f, (scale - 0.5f)*ScaleRate + 1.0f);
            transform.localScale = newScale;
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f);
        }
    }
}
