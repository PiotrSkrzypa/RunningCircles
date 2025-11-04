using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyController enemyPrefab;

    [SerializeField] int count = 1000;

    [SerializeField] int maxSpawnAttemptsPerEnemy = 10;

    [SerializeField] float minSpacing = 0.05f;

    [SerializeField] float _marginPercent = 0.01f;

    private GameObject _player;
    private Camera _camera;
    private (float minX, float maxX, float minY, float maxY) _cameraBoundaries;
    private List<EnemyController> _enemies = new List<EnemyController>();

    void Awake()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner: assign enemyPrefab in inspector.");
            enabled = false;
            return;
        }

        _camera = Camera.main;
        CalculateCameraBounds();
    }

    void Start()
    {
        FindPlayer();
        SpawnEnemies();
    }
    private void FindPlayer()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void SpawnEnemies()
    {
        List<Vector2> positions = new List<Vector2>(count);

        for (int i = 0; i < count; i++)
        {
            Vector2 pos = Vector2.zero;
            bool placed = false;
            for (int attempt = 0; attempt < maxSpawnAttemptsPerEnemy; attempt++)
            {
                float x = Random.Range(_cameraBoundaries.minX, _cameraBoundaries.maxX);
                float y = Random.Range(_cameraBoundaries.minY, _cameraBoundaries.maxY);
                Vector2 tryPos = new Vector2(x, y);

                bool ok = true;
                
                if (_player != null && Vector2.Distance(tryPos, _player.transform.position) < 0.5f) ok = false;

                if (ok)
                {
                    for (int j = 0; j < positions.Count; j++)
                    {
                        if (Vector2.Distance(tryPos, positions[j]) < minSpacing)
                        {
                            ok = false;
                            break;
                        }
                    }
                }

                if (ok)
                {
                    pos = tryPos;
                    placed = true;
                    break;
                }
            }

            // fallback: place anywhere if we couldn't find spaced position
            if (!placed)
            {
                pos = new Vector2(Random.Range(_cameraBoundaries.minX, _cameraBoundaries.maxX), Random.Range(_cameraBoundaries.minY, _cameraBoundaries.maxY));
            }

            positions.Add(pos);
            var enemy = Instantiate(enemyPrefab, pos, Quaternion.identity, transform);
            enemy.Initialize(_player, _cameraBoundaries);
            _enemies.Add(enemy);
            enemy.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
    }

    private void Update()
    {
        foreach (var enemy in _enemies)
        {
            enemy.UpdateEnemy();
        }
    }
    private void CalculateCameraBounds()
    {
        Vector3 bl = _camera.ViewportToWorldPoint(new Vector3(_marginPercent, _marginPercent, 0));
        Vector3 tr = _camera.ViewportToWorldPoint(new Vector3(1f - _marginPercent, 1f - _marginPercent, 0));
        _cameraBoundaries.minX = bl.x;
        _cameraBoundaries.minY = bl.y;
        _cameraBoundaries.maxX = tr.x;
        _cameraBoundaries.maxY = tr.y;
    }
}
