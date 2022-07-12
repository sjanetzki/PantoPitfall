using System.Threading.Tasks;
using UnityEngine;
using DualPantoFramework;
using System;

public class MovingObstacleManager : MonoBehaviour
{
    // moving a circular collider at 25Hz.
    // known issue: user can still get into the obstacle
    GameObject obstacle;
    Vector3 direction = new Vector3(1f, 0, 0);
    PantoCollider pantoCollider;
    async void Start()
    {
        obstacle = GameObject.Find("Obstacle");
        pantoCollider = obstacle.GetComponent<PantoCircularCollider>();

        pantoCollider.CreateObstacle();
        pantoCollider.Enable();
        while (true)
        {
            Vector3 newPos = obstacle.transform.position + direction;
            if (Math.Abs(obstacle.transform.position.x) <= 5 && Math.Abs(newPos.x) > 5)
            {
                direction = direction * -1;
            }
            await MoveObstacle(newPos);
        }
    }

    async Task MoveObstacle(Vector3 position)
    {
        PantoCollider oldCollider = obstacle.GetComponent<PantoCollider>();

        //clone obstacle to make sure we don't overwrite the reference to the old collider
        GameObject newObs = Instantiate(obstacle);
        Destroy(obstacle);
        obstacle = newObs;
        obstacle.transform.position = position;
        PantoCollider collider = obstacle.GetComponent<PantoCollider>();

        // first enable the new collider before removing the old one to make sure the user is not accidentally getting into the obstacle
        collider.CreateObstacle();
        collider.Enable();
        oldCollider.Remove();
        await Task.Delay(2000);
    }
}
