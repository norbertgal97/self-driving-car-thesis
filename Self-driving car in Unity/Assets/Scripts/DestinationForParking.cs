using UnityEngine;

public class DestinationForParking : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == Strings.car)
    {
      CarController carController = other.
        gameObject.GetComponent<CarController>();
      carController.isParking = true;

      DistanceSensor distanceSensor = other.
        gameObject.GetComponentInChildren<DistanceSensor>();
      distanceSensor.gameObject.SetActive(false);
    }
  }
}
