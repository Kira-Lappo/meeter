using Meeter.Models;

namespace Meeter.Services.Stores;

public interface IMeetingStore
{
    IEnumerable<Meeting> GetAll();

    Meeting GetById(Guid id);

    Meeting Add(Meeting meeting);

    void Remove(Guid id);

    void Update(Meeting meeting);
}
