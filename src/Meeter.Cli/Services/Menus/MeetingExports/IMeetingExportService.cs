using Meeter.Models;

namespace Meeter.Cli.Services.Menus.MeetingExports;

public interface IMeetingExportService
{
    void Export(Stream stream, MeeterExportData exportModel);
}
