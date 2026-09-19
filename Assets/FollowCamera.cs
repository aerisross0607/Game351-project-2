using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Terrain terrain;

    public float distance = 8f;
    public float height = 4f;

    public float positionSmooth = 6f;
    public float rotationSmooth = 6f;

    public float terrainOffset = 2f;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition =
            target.position
            - target.forward * distance
            + Vector3.up * height;

        if (terrain != null)
        {
            float terrainHeight =
                terrain.SampleHeight(desiredPosition)
                + terrain.transform.position.y;

            float minimumHeight =
                terrainHeight + terrainOffset;

            if (desiredPosition.y < minimumHeight)
            {
                desiredPosition.y = minimumHeight;
            }
        }

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            positionSmooth * Time.deltaTime
        );

        Vector3 lookDirection =
            target.position - transform.position;

        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSmooth * Time.deltaTime
            );
        }
    }
}