using System;
using System.Collections.Generic;
using Meeter.Models;

namespace Meeter.Services.Stores;

public interface IMeetingStore
{
    IEnumerable<Meeting> GetAll();

    Meeting GetById(Guid id);

    void Add(Meeting meeting);

    void Remove(Guid id);

    void Update(Meeting meeting);
}
