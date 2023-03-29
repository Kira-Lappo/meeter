using Meeter.Models.Exports;

namespace Meeter.Cli.Services.MeetingExports;

public interface IMeetingExportService
{
    void Export(Stream stream, MeeterExportData exportModel);
}
