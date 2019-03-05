using System.Text;
using Entitas;
using UnityEngine;
using Common;
using Entitas.Serialization;
using Entitas.Serialization.Json;

namespace IO
{
    public class GameController : MonoBehaviour
    {
//		public GameObject Obj1;
//		public GameObject Obj2;
        private Systems m_systems;

        void Awake()
        {
            var contexts = Contexts.sharedInstance;
            InitServices();
            m_systems = new Systems(contexts);
        }

        // Use this for initialization
        void Start()
        {
			var position = new Vector3(4f, 6f, 0f);
			var quaternion =  Quaternion.AngleAxis(-135f, Vector3.forward);
			var matrix = Matrix4x4.TRS(position, quaternion, Vector3.one);
			var newQuaternion = matrix.rotation;
			Debug.LogFormat("{0},{1}",quaternion, newQuaternion);
			var v = Quaternion.Dot(quaternion, newQuaternion);
			Debug.Log(v);

//			Obj1.transform.rotation = quaternion;
//			Obj2.transform.rotation = newQuaternion;

            m_systems.Initialize(); 
        }

        void InitServices()
        {
            Services.Add<ILogService>(new LogService());

            var viewService = new ViewService(Contexts.sharedInstance);
            Services.Add(viewService);
            Services.Add<ITimeService>(new TimeService());
            Services.Add(new EntityIdService());
            
            // blueprint service
            var blueprintService = new BlueprintService(new JsonBlueprints());
            var text = Resources.Load<TextAsset>("blueprints");
            blueprintService.AddBlueprints(text.bytes);
            Resources.UnloadAsset(text);
            Services.Add(blueprintService);
        }

        // Update is called once per frame
        void Update()
        {
            m_systems.Execute();
            m_systems.Cleanup();
        }

        private void OnDestroy()
        {
            m_systems.TearDown();
        }
    }
}