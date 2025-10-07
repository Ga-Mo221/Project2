using UnityEngine;

public class SelectBoxItem : MonoBehaviour
{
    [SerializeField] private PlayerOrEnemyTowerSelectBox _playerOrEnemyTower;
    [SerializeField] private PlayerOrEnemyStorageSelectBox _playerOrEnemyStorage;
    [SerializeField] private EnemyHouseSelectBox _enemyHouse;
    [SerializeField] private PlayerSelectBox _player;
    [SerializeField] private EnemyOrAnimalSelectBox _enemyOrAnimal;
    [SerializeField] private TreeOrRockSelectBox _treeOrRock;
    [SerializeField] private GoldMineSelectBox _gold;

    public void openPlayerTower(House house)
    {
        _playerOrEnemyTower.add(house);
        _playerOrEnemyTower.gameObject.SetActive(true);
        _playerOrEnemyStorage.gameObject.SetActive(false);
        _enemyHouse.gameObject.SetActive(false);
        _player.gameObject.SetActive(false);
        _enemyOrAnimal.gameObject.SetActive(false);
        _treeOrRock.gameObject.SetActive(false);
        _gold.gameObject.SetActive(false);
    }

    public void openEnemyTower(EnemyHuoseController house)
    {
        _playerOrEnemyTower.add(house);
        _playerOrEnemyTower.gameObject.SetActive(true);
        _playerOrEnemyStorage.gameObject.SetActive(false);
        _enemyHouse.gameObject.SetActive(false);
        _player.gameObject.SetActive(false);
        _enemyOrAnimal.gameObject.SetActive(false);
        _treeOrRock.gameObject.SetActive(false);
        _gold.gameObject.SetActive(false);
    }

    public void openPlayerStorage(House house)
    {
        _playerOrEnemyStorage.add(house);
        _playerOrEnemyTower.gameObject.SetActive(false);
        _playerOrEnemyStorage.gameObject.SetActive(true);
        _enemyHouse.gameObject.SetActive(false);
        _player.gameObject.SetActive(false);
        _enemyOrAnimal.gameObject.SetActive(false);
        _treeOrRock.gameObject.SetActive(false);
        _gold.gameObject.SetActive(false);
    }

    public void openEnemyStorage(EnemyHuoseController house)
    {
        _playerOrEnemyStorage.add(house);
        _playerOrEnemyTower.gameObject.SetActive(false);
        _playerOrEnemyStorage.gameObject.SetActive(true);
        _enemyHouse.gameObject.SetActive(false);
        _player.gameObject.SetActive(false);
        _enemyOrAnimal.gameObject.SetActive(false);
        _treeOrRock.gameObject.SetActive(false);
        _gold.gameObject.SetActive(false);
    }

    public void openEnemyHouse(EnemyHuoseController house)
    {
        _enemyHouse.add(house);
        _playerOrEnemyTower.gameObject.SetActive(false);
        _playerOrEnemyStorage.gameObject.SetActive(false);
        _enemyHouse.gameObject.SetActive(true);
        _player.gameObject.SetActive(false);
        _enemyOrAnimal.gameObject.SetActive(false);
        _treeOrRock.gameObject.SetActive(false);
        _gold.gameObject.SetActive(false);
    }

    public void openPlayer(PlayerAI player)
    {
        _player.add(player);
        _playerOrEnemyTower.gameObject.SetActive(false);
        _playerOrEnemyStorage.gameObject.SetActive(false);
        _enemyHouse.gameObject.SetActive(false);
        _player.gameObject.SetActive(true);
        _enemyOrAnimal.gameObject.SetActive(false);
        _treeOrRock.gameObject.SetActive(false);
        _gold.gameObject.SetActive(false);
    }

    public void openEnemy(EnemyAI enemy)
    {
        _enemyOrAnimal.add(enemy);
        _playerOrEnemyTower.gameObject.SetActive(false);
        _playerOrEnemyStorage.gameObject.SetActive(false);
        _enemyHouse.gameObject.SetActive(false);
        _player.gameObject.SetActive(false);
        _enemyOrAnimal.gameObject.SetActive(true);
        _treeOrRock.gameObject.SetActive(false);
        _gold.gameObject.SetActive(false);
    }

    public void openAnimal(AnimalAI animal)
    {
        _enemyOrAnimal.add(animal);
        _playerOrEnemyTower.gameObject.SetActive(false);
        _playerOrEnemyStorage.gameObject.SetActive(false);
        _enemyHouse.gameObject.SetActive(false);
        _player.gameObject.SetActive(false);
        _enemyOrAnimal.gameObject.SetActive(true);
        _treeOrRock.gameObject.SetActive(false);
        _gold.gameObject.SetActive(false);
    }

    public void openTreeOrRock(Item item)
    {
        _treeOrRock.add(item);
        _playerOrEnemyTower.gameObject.SetActive(false);
        _playerOrEnemyStorage.gameObject.SetActive(false);
        _enemyHouse.gameObject.SetActive(false);
        _player.gameObject.SetActive(false);
        _enemyOrAnimal.gameObject.SetActive(false);
        _treeOrRock.gameObject.SetActive(true);
        _gold.gameObject.SetActive(false);
    }

    public void openGold(Item item)
    {
        _gold.add(item);
        _playerOrEnemyTower.gameObject.SetActive(false);
        _playerOrEnemyStorage.gameObject.SetActive(false);
        _enemyHouse.gameObject.SetActive(false);
        _player.gameObject.SetActive(false);
        _enemyOrAnimal.gameObject.SetActive(false);
        _treeOrRock.gameObject.SetActive(false);
        _gold.gameObject.SetActive(true);
    }
}
