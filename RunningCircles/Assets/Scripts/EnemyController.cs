using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed = 3f;

    [Tooltip("How often (in seconds) the random direction changes")]
    [SerializeField] float _randomChangeInterval = 1.5f;

    [Tooltip("How strongly the enemy avoids the player (0 = ignores player, 1 = full flee)")]
    [SerializeField][Range(0f, 1f)] float fearStrength = 0.8f;

    private Vector2 _randomDir;
    private float _nextRandomTime;

    private Transform _player;
    private bool _stopped = false;
    private (float minX, float maxX, float minY, float maxY) _cameraBoundaries;

    public void Initialize(GameObject player, (float _minX, float _maxX, float _minY, float _maxY) cameraBoundaries)
    {
        if (player != null)
        {
            _player = player.transform;
        }
        _cameraBoundaries = cameraBoundaries;
        PickNewWanderDir();
    }

    public void UpdateEnemy()
    {
        if (_stopped || _player == null) return;

        if (Time.time >= _nextRandomTime)
        {
            PickNewWanderDir();
        }

        Vector2 dir = _randomDir;

        Vector2 awayFromPlayer = ((Vector2)transform.position - (Vector2)_player.position).normalized;

        dir = Vector2.Lerp(dir, awayFromPlayer, fearStrength).normalized;

        // Move
        Vector2 newPos = (Vector2)transform.position + dir * speed * Time.deltaTime;

        // Clamp within camera bounds and gently steer inward
        newPos.x = Mathf.Clamp(newPos.x, _cameraBoundaries.minX, _cameraBoundaries.maxX);
        newPos.y = Mathf.Clamp(newPos.y, _cameraBoundaries.minY, _cameraBoundaries.maxY);

        transform.position = newPos;
    }

    private void PickNewWanderDir()
    {
        _randomDir = Random.insideUnitCircle.normalized;
        _nextRandomTime = Time.time + _randomChangeInterval * Random.Range(0.5f, 1.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_stopped) return;
        if (other.CompareTag("Player"))
        {
            _stopped = true;
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = Color.gray;
        }
    }
}
