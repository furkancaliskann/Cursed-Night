using System.Collections.Generic;
using UnityEngine;

public class SaveCars : MonoBehaviour
{
    private ItemList itemList;
    private SaveManager saveManager;

    [SerializeField] private GameObject carPrefab;

    void Awake()
    {
        itemList = GetComponent<ItemList>();
        saveManager = GetComponent<SaveManager>();
    }
    public void SaveCar()
    {
        SaveCar saveCar = new SaveCar();

        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        for (int i = 0; i < cars.Length; i++)
        {
            CarController carController = cars[i].GetComponent<CarController>();
            Chest chest = cars[i].GetComponent<Chest>();

            saveCar.position.Add(cars[i].transform.position);
            saveCar.rotation.Add(cars[i].transform.rotation);
            saveCar.fuel.Add(carController.fuel);
            saveCar.durability.Add(carController.durability);
            saveCar.carStarted.Add(carController.carStarted);
            saveCar.carStoped.Add(carController.carStoped);
            saveCar.selectedRadioChannel.Add(carController.selectedRadioChannel);

            SaveInventory saveChest = new SaveInventory();

            for (int j = 0; j < chest.items.Count; j++)
            {
                if (chest.items[j] != null)
                {
                    saveChest.nickName.Add(chest.items[j].nickName);
                    saveChest.amount.Add(chest.items[j].amount);
                    saveChest.durability.Add(chest.items[j].durability);
                    saveChest.ammoInside.Add(chest.items[j].ammoInside);
                }
                else
                {
                    saveChest.nickName.Add(null);
                    saveChest.amount.Add(0);
                    saveChest.durability.Add(0);
                    saveChest.ammoInside.Add(0);
                }
            }

            saveCar.carChest.Add(saveChest);
        }

        StartCoroutine(saveManager.Save(saveCar, SaveType.Car));
    }
    public void LoadCar()
    {
        SaveCar saveCar = JsonUtility.FromJson<SaveCar>(saveManager.Load(SaveType.Car));

        if (saveCar == null) return;

        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        for (int i = 0; i < cars.Length; i++)
        {
            Destroy(cars[i]);
        }

        for (int i = 0; i < saveCar.position.Count; i++)
        {
            GameObject car = Instantiate(carPrefab, saveCar.position[i], saveCar.rotation[i]);

            car.GetComponent<CarController>().LoadCar(saveCar.fuel[i], saveCar.durability[i], saveCar.carStarted[i],
                saveCar.carStoped[i], saveCar.selectedRadioChannel[i]);

            Chest chest = car.GetComponent<Chest>();

            for (int j = 0; j < saveCar.carChest[i].nickName.Count; j++)
            {
                chest.items[j] = itemList.CreateNewItem(saveCar.carChest[i].nickName[j], saveCar.carChest[i].amount[j],
                    saveCar.carChest[i].durability[j], saveCar.carChest[i].ammoInside[j]);
            }
        }
    }
}

[System.Serializable]
public class SaveCar
{
    public List<Vector3> position = new List<Vector3>();
    public List<Quaternion> rotation = new List<Quaternion>();
    public List<float> fuel = new List<float>();
    public List<int> durability = new List<int>();
    public List<bool> carStarted = new List<bool>();
    public List<bool> carStoped = new List<bool>();
    public List<int> selectedRadioChannel = new List<int>();

    public List<SaveInventory> carChest = new List<SaveInventory>();
}
