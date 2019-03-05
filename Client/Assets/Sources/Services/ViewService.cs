using Common;
using UnityEngine;
namespace IO
{
	public class ViewService:IService, IAssetListener
	{
		private readonly Contexts m_contexts;
		public ViewService(Contexts contexts)
		{
			m_contexts = contexts;
			contexts.game.CreateEntity().AddAssetListener(this);
		}
		
		public void OnAsset(GameEntity entity, string value)
		{
			var prefab = Resources.Load<GameObject>(value);
			var view = Object.Instantiate(prefab).GetComponent<IView>();
			view.Link(entity, m_contexts.game);
		}
	}
}