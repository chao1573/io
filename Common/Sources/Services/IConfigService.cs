using Common.Data;

namespace Common
{
    public interface IConfigService:IService
    {
        PlayerData GetPlayer(int id);
    }
}