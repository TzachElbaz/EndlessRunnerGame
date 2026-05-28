using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] public bool _IsObsteclCourse;
    [SerializeField] public float _GenerateDistance;
    Player player;
    [SerializeField] private float depth = 1;
    public PASS_POINT _passPoint;
    public GameObject[] _purseList;


    public enum PASS_POINT
    {
        UP,
        MIDDLE,
        DOWN,
        UP_MIDDLE,
        UP_DOWN,
        MIDDLE_DOWN,

    }
    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        RunGameManeger.ClearAllObstacles += ClearAllObstacle;
        RunGameManeger.ClearOffScreenObstacles += ClearOffScreenObstacles;
        RunGameManeger.ClearOnScreenObstacles += ClearOnScreenObstacles;
    }
    private void OnDestroy()
    {
        RunGameManeger.ClearAllObstacles -= ClearAllObstacle;
        RunGameManeger.ClearOffScreenObstacles -= ClearOffScreenObstacles;
        RunGameManeger.ClearOnScreenObstacles -= ClearOnScreenObstacles;
    }
    private void FixedUpdate()
    {
        float realVelocity = player.velocity.x / depth;
        Vector2 position = transform.position;

        position.x -= realVelocity * Time.fixedDeltaTime;

        if (position.x < -30 && !_IsObsteclCourse)
        {
            
            if (!_IsObsteclCourse) Destroy(gameObject);
            else if(position.x < -60) Destroy(gameObject);
        } 
            

        transform.position = position;
    }

    private void ClearAllObstacle()
    {
        Destroy(gameObject);
    }
    private void ClearOffScreenObstacles()
    {
        if (transform.position.x < 62) return;
        Destroy(gameObject);
        
    }
    private void ClearOnScreenObstacles()
    {
        if (transform.position.x > 66) return;
        Destroy(gameObject);
        
    }

    public void ActivatePurse(int purseNum)
    {
        _purseList[purseNum].SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 endPosition = new Vector2 (transform.position.x + _GenerateDistance, transform.position.y);
        Gizmos.DrawLine(transform.position, endPosition);
        Gizmos.DrawLine(endPosition, new Vector2(endPosition.x, endPosition.y + 5));
    }
}
