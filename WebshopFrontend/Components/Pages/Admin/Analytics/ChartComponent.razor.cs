using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WebshopFrontend.Constants;
using WebshopFrontend.DTOs;

namespace WebshopFrontend.Components.Pages.Admin.Analytics;

public partial class ChartComponent : ComponentBase, IDisposable
{
    [Parameter]
    public List<DataPoint> Data { get; set; } = [];

    [Parameter]
    public string ChartTitle { get; set; } = "Data";

    [Parameter]
    public ChartType ChartType { get; set; } = ChartType.Bar;

    [Parameter]
    public string Color { get; set; } = "#007bff";

    private readonly string _chartId = $"chart-{Guid.NewGuid():N}";
    private bool _isRendered;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && Data.Count != 0)
        {
            _isRendered = true;
            await DrawChartAsync();
        }
        else if (_isRendered && Data.Count != 0)
        {
            await DrawChartAsync();
        }
    }

    private async Task DrawChartAsync()
    {
        var displayData = Data.TakeLast(30).ToList(); 

        var labels = displayData.Select(d => d.Label).ToArray();
        var values = displayData.Select(d => d.Value).ToArray();

        var typeString = ChartType == ChartType.Bar ? "bar" : "line";
        var title = ChartTitle;
        var elementId = _chartId;
        var primaryColor = Color;

        await JsRuntime.InvokeVoidAsync("drawCanvasChart", elementId, typeString, labels, values, title, primaryColor);
    }

    public void Dispose()
    {
        // Cleanup, indien nodig
        // In dit geval is een simpele Canvas cleanup voldoende
    }
}