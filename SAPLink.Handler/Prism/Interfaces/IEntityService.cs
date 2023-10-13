using SAPLink.Core;

namespace SAPLink.Handler.Prism.Interfaces;
public interface IEntityService<T, TU> where T : class where TU : class
{
    Task<T> GetByCodeAsync(string code);
    Task<T> Sync(string body, string sid = "", Enums.UpdateType updateType = Enums.UpdateType.InitialDepartment);
    Task<string> CreateEntityPayload(TU entity);
    Task<string> CreateUpdatePayload(TU entity, long rowVersion);
}

public interface IEntitiesService<T, TU> where T : class where TU : class
{
    Task<T> GetAll();
    Task<T> AddAll(List<TU> list);
    string CreateEntitiesPayload(List<TU> entitiesList);
}