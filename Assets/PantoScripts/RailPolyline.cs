using DualPantoFramework;
namespace UnityEngine
{


    public class RailPolyline : ColliderPolyline
    {
        public string[] texts;
        public float displacement = 0.3f;
        public RailPolyline()
        {

            this.gizmoColor = Color.cyan;
        }
        protected override void CreateObstacle(int i)
        {
            Rail r = this.gameObject.AddComponent<Rail>();
            if (i < texts.Length)
            {
                r.text = texts[i];
            }

            r.CreateObstacle(points[i], points[i + 1], displacement);
            r.Enable();
            DrawLine(points[i], points[i+1]);

            // create new trigger for sound
            GameObject newObj = new GameObject("RailTrigger");
            newObj.transform.parent = transform;
            newObj.transform.position = new Vector3(
                (points[i].x + points[i + 1].x) / 2,
                0,
                (points[i].y + points[i + 1].y) / 2
            );
            BoxCollider newTrigger = newObj.AddComponent<BoxCollider>();
            newTrigger.isTrigger = true;
            newTrigger.size = new Vector3(
                1, // <--- width of trigger
                10,
                Vector2.Distance(points[i], points[i + 1])
            );
            newObj.transform.LookAt(new Vector3(points[i].x, 0, points[i].y), Vector3.up);
            newObj.AddComponent<RailTrigger>();
        }
        void DrawLine(Vector2 start, Vector2 end)
        {
            GameObject n = new GameObject();
            n.transform.parent = transform;
            n.layer = LayerMask.NameToLayer("Rails");
            LineRenderer lr = n.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(start.x, 5, start.y));
            lr.SetPosition(1, new Vector3(end.x, 5, end.y));
            lr.startWidth = 0.01f * GetPantoSync().gameObject.transform.localScale.magnitude;
            lr.material = Resources.Load("Materials/Rails") as Material;
        }
    }

    public class RailTrigger : MonoBehaviour
    {
        AudioSource audioSource;
        float fadeChangePerSecond = 5f;
        float fadeTarget;

        private void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = Resources.Load<AudioClip>("Sounds/railSound");
            audioSource.loop = true;
            audioSource.volume = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //audioSource.Play();
                fadeTarget = 1;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //audioSource.Stop();
                fadeTarget = 0;
            }
        }

        private void Update()
        {
            if (audioSource.volume != fadeTarget)
            {
                audioSource.volume = Mathf.Clamp01(audioSource.volume + Mathf.Sign(fadeTarget - audioSource.volume) * fadeChangePerSecond * Time.deltaTime);
            }
            if (audioSource.volume == 0 && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            if (audioSource.volume > 0 && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
