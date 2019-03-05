using Entitas;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Xna = Microsoft.Xna.Framework;

namespace IO
{
    public class InputController : MonoBehaviour
    {
        private Contexts m_contexts;
        private string m_inputId;
        // Use this for initialization
        void Start()
        {
            m_inputId = "11111";
            m_contexts = Contexts.sharedInstance;
            //            var inputEntity =  m_contexts.input.CreateEntity();
            //            inputEntity.AddInputId(m_inputId);

        }

        // Update is called once per frame
        void Update()
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            var inputEntity = m_contexts.input.CreateEntity();
            inputEntity.AddInputOwner(m_inputId);
            inputEntity.AddMoveInput(Xna.Vector2.Normalize(new Xna.Vector2(h, v)));
//            if (Mathf.Abs(h) > 0f || Mathf.Abs(v) > 0f)
//            {
//
//            }
            //                m_contexts.input.GetEntitiesWithInputId(m_inputId).SingleEntity().ReplaceMoveInput(new Vector2(h,v).normalized);
            //}
            //                m_contexts.input.CreateEntity().AddMoveInput(new Vector2(h, v).normalized);

            //            if(Input.GetButtonDown("Fire"))
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                var shootEntity = m_contexts.input.CreateEntity();
                shootEntity.AddInputOwner(m_inputId);
                shootEntity.isShootInput = true;
            }
        }

        public void OnCreatePlayerClicked()
        {
            var inputEntity = m_contexts.input.CreateEntity();
            inputEntity.AddInputOwner(m_inputId);
            inputEntity.isJoinGameInput = true;
        }
    }
}