using UnityEngine;

public class PedestrianSensor : MonoBehaviour
{
  [SerializeField]
  private GameObject pedestrian = null;
  private GameObject pedestrianCrossing = null;

  private void Update()
  {
    if (pedestrianCrossing != null)
    {
      PedestrianCrossingController pedestrianCrossingController = pedestrianCrossing.GetComponent<PedestrianCrossingController>();
      PedestrianController pedestrianController = pedestrian.GetComponent<PedestrianController>();
      bool stopped = false;
      bool reversed = false;

      if (pedestrianCrossingController.carCounter > 0 || pedestrianCrossingController.dangerous)
      {
        stopped = true;

        if (pedestrianCrossingController.pedestrianCounter > 0)
        {
          reversed = true;
          stopped = false;
        }
      }
      else
      {
        stopped = false;
        reversed = false;
      }
      pedestrianController.reversed = reversed;
      pedestrianController.stopped = stopped;
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == Strings.pedestrianCrossing)
    {
      pedestrianCrossing = other.gameObject;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.tag == Strings.pedestrianCrossing)
    {
      PedestrianController pedestrianController = pedestrian.GetComponent<PedestrianController>();
      pedestrianCrossing = null;
      pedestrianController.reversed = false;
      pedestrianController.stopped = false;
    }
  }
}
