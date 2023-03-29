using Meeter.Models;

namespace Meeter.Services.Exports;

public interface IMeetingExportService
{
    void Export(Stream stream, MeeterExportData exportModel);
}
