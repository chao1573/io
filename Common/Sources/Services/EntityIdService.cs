namespace Common
{
    public class EntityIdService:IService
    {
        private int m_startIndex = 0;

        public int GetId()
        {
            return m_startIndex++;
        }
    }
}