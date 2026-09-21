using UnityEngine;

public class CarSwitcher : MonoBehaviour
{
    public GameObject[] cars;
    public FollowCamera followCamera;

    private int currentCar = 0;

    void Start()
    {
        SwitchCar(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCar = (currentCar + 1) % cars.Length;
            SwitchCar(currentCar);
        }
    }

    void SwitchCar(int index)
    {
        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].SetActive(i == index);
        }

        followCamera.target = cars[index].transform;
    }
}