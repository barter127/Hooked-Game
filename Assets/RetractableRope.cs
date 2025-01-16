using UnityEngine;

public class RetractableRope : MonoBehaviour
{
    public GameObject ropeSegmentPrefab;
    public int segmentCount = 10;
    public float segmentLength = 1.0f;
    public float retractionSpeed = 5.0f;

    private GameObject[] ropeSegments;
    private bool isRetracting = false;

    void Start()
    {
        CreateRope();
        EnableRope(false);  // Start with the rope disabled
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRetracting = !isRetracting;
            EnableRope(!isRetracting);
        }

        if (isRetracting)
        {
            RetractRope();
        }
    }

    void CreateRope()
    {
        ropeSegments = new GameObject[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            ropeSegments[i] = Instantiate(ropeSegmentPrefab, transform);
            ropeSegments[i].transform.position = transform.position + Vector3.down * segmentLength * i;
            if (i > 0)
            {
                HingeJoint2D joint = ropeSegments[i].AddComponent<HingeJoint2D>();
                joint.connectedBody = ropeSegments[i - 1].GetComponent<Rigidbody2D>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = new Vector2(0, 0.5f);
                joint.anchor = new Vector2(0, -0.5f);
            }
        }
    }

    void EnableRope(bool enable)
    {
        foreach (GameObject segment in ropeSegments)
        {
            segment.SetActive(enable);
        }
    }

    void RetractRope()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            ropeSegments[i].transform.position = Vector3.MoveTowards(
                ropeSegments[i].transform.position,
                transform.position,
                retractionSpeed * Time.deltaTime
            );
        }
    }
}
