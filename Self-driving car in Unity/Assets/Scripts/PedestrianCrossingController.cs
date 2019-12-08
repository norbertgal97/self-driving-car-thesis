using UnityEngine;

public class PedestrianCrossingController : MonoBehaviour
{
  public int pedestrianCounter { get; private set; } = 0;
  public int carCounter { get; private set; } = 0;
  public bool dangerous { get; private set; } = false;

  private GameObject distanceSensorGameObject;

  public void SetDangerous(GameObject other)
  {
    float velocity = other.GetComponent<Rigidbody>().velocity.magnitude;

    if (velocity > 8f)
    {
      dangerous = true;
    }
    else
    {
      dangerous = false;
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    switch (other.tag)
    {
      case Strings.car:
        carCounter++;
        break;
      case Strings.pedestrian:
        pedestrianCounter++;
        break;
      default:
        break;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    switch (other.tag)
    {
      case Strings.car:
        carCounter--;
        dangerous = false;
        break;
      case Strings.pedestrian:
        pedestrianCounter--;
        break;
      default:
        break;
    }
  }
}
