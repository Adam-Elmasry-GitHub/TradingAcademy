using System.Text;
using TradingAcademy;
using TradingAcademy.Learning;
using TradingAcademy.UI;

Console.OutputEncoding = Encoding.UTF8;

try
{
    var levels = Curriculum.Load();
    new App(levels, new ProgressStore()).Run();
}
catch (Exception e) when (e is IOException or InvalidDataException or System.Text.Json.JsonException)
{
    ConsoleUi.Error($"Could not start Trading Academy: {e.Message}");
    return 1;
}

return 0;
