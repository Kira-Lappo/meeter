using System.Text.Json;
using Meeter.Models.Exports;
using Meeter.Services;

namespace Meeter.Cli.Services.MeetingExports;

public class MeetingJsonExportService : IMeetingExportService
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
    };

    public void Export(Stream stream, MeeterExportData exportModel)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(exportModel);

        JsonSerializer.Serialize(stream, exportModel, SerializerOptions);
    }
}
