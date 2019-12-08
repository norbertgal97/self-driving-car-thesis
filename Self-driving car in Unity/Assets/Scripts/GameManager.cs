using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; private set; }

  [SerializeField]
  private List<Node> sourcesForCars;
  [SerializeField]
  private List<Node> destinationsForCars;
  [SerializeField]
  private GameObject carPrefab;
  [SerializeField]
  private GameObject pedestrianPrefab;
  [SerializeField]
  private List<Transform> sourcesForPedestrians;
  [SerializeField]
  [Range(1.0f, 180.0f)]
  private float spawnTimeForCars = 1f;
  [SerializeField]
  [Range(0.5f, 180.0f)]
  private float spawnTimeForPedestrians = 1f;
  [SerializeField]
  private int carCounter = 0;
  [SerializeField]
  private int pedestrianCounter = 0;
  [SerializeField]
  private int maxCarsOnScreen = 10;
  [SerializeField]
  private int maxPedestriansOnScreen = 10;
  private float currentSpawnTimeForCars = 0f;
  private float currentSpawnTimeForPedestrians = 0f;
  private List<Node> availableSourcesForCars = new List<Node>();
  private List<Transform> availableSourcesForPedestrians = new List<Transform>();

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
    }
    else
    {
      Instance = this;
    }
  }

  private void Update()
  {
    currentSpawnTimeForCars += Time.deltaTime;
    currentSpawnTimeForPedestrians += Time.deltaTime;

    if (currentSpawnTimeForCars > spawnTimeForCars && carCounter < maxCarsOnScreen)
    {
      SpawnCar();
      currentSpawnTimeForCars = 0f;
    }

    if (currentSpawnTimeForPedestrians > spawnTimeForPedestrians && pedestrianCounter < maxPedestriansOnScreen)
    {
      SpawnPedestrian();
      currentSpawnTimeForPedestrians = 0f;
    }
  }

  private void SpawnCar()
  {
    if (availableSourcesForCars.Count == 0)
    {
      foreach (Node source in sourcesForCars)
      {
        if (!source.GetComponent<SourceForCars>().busy)
        {
          availableSourcesForCars.Add(source);
        }
      }
    }

    if (availableSourcesForCars.Count == 0)
    {
      return;
    }

    carPrefab.SetActive(false);

    Node randomSource = availableSourcesForCars[Random.Range(0, availableSourcesForCars.Count)];
    Node randomDestination = destinationsForCars[Random.Range(0, destinationsForCars.Count)];

    availableSourcesForCars.Remove(randomSource);

    if (randomDestination.gameObject.tag == Strings.destinationForParking)
    {
      destinationsForCars.Remove(randomDestination);
    }

    GameObject car = Instantiate(carPrefab, randomSource.transform.position, Quaternion.identity);
    CarController carController = car.GetComponent<CarController>();

    carController.source = randomSource;
    carController.destination = randomDestination;
    carController.OnDestroy.AddListener(CarDestroyed);

    switch (randomSource.gameObject.tag)
    {
      case Strings.sourceSouth:
        break;
      case Strings.sourceWest:
        car.transform.Rotate(0f, 90f, 0f);
        break;
      case Strings.sourceEast:
        car.transform.Rotate(0f, -90f, 0f);
        break;
      case Strings.sourceNorth:
        car.transform.Rotate(0f, 180f, 0f);
        break;
      default:
        break;
    }

    carCounter++;
    car.SetActive(true);
  }

  private void SpawnPedestrian()
  {
    if (availableSourcesForPedestrians.Count == 0)
    {
      foreach (Transform source in sourcesForPedestrians)
      {
        if (!source.gameObject.GetComponent<SourceForPedestrians>().busy)
        {
          availableSourcesForPedestrians.Add(source);
        }
      }
    }

    if (availableSourcesForPedestrians.Count == 0)
    {
      return;
    }

    pedestrianPrefab.SetActive(false);

    Transform randomSource = availableSourcesForPedestrians[Random.Range(0, availableSourcesForPedestrians.Count)];
    availableSourcesForPedestrians.Remove(randomSource);

    GameObject pedestrian = Instantiate(pedestrianPrefab, randomSource.position, Quaternion.identity);

    PedestrianController pedestrianController = pedestrian.GetComponent<PedestrianController>();
    pedestrianController.OnDestroy.AddListener(PedestrianDestroyed);

    pedestrian.transform.position = new Vector3(randomSource.transform.position.x, 0.1f, randomSource.transform.position.z);

    switch (randomSource.gameObject.tag)
    {
      case Strings.sourceWest:
        pedestrian.transform.Rotate(0f, 90f, 0f);
        break;
      case Strings.sourceEast:
        pedestrian.transform.Rotate(0f, -90f, 0f);
        break;
      default:
        break;
    }

    pedestrianCounter++;
    pedestrian.SetActive(true);
  }

  public void CarDestroyed()
  {
    carCounter--;
  }

  public void PedestrianDestroyed()
  {
    pedestrianCounter--;
  }
}
