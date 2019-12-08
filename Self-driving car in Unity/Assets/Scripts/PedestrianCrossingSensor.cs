using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianCrossingSensor : MonoBehaviour
{
  public PedestrianCrossingController pedestrianCrossingController = null;

  private void OnTriggerEnter(Collider other)
  {
    if(other.tag == Strings.car)
    {
      pedestrianCrossingController.SetDangerous(other.gameObject);
    }
  }
}
