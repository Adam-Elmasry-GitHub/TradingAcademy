using System.Text.Json;
using Microsoft.JSInterop;
using TradingAcademy.Learning;

namespace TradingAcademy.Web.Services;

/// <summary>
/// Holds one visitor's progress for the lifetime of their browser connection and saves it to the
/// browser's localStorage, so every learner keeps their own progress without an account.
/// </summary>
public sealed class ProgressService
{
    private const string StorageKey = "trading-academy:progress";
    private readonly IJSRuntime _js;

    public ProgressService(IJSRuntime js) => _js = js;

    public Progress Progress { get; private set; } = new();
    public bool IsLoaded { get; private set; }
    public event Action? Changed;

    /// <summary>Reads saved progress. Call from OnAfterRenderAsync, because JavaScript is not available while prerendering.</summary>
    public async Task EnsureLoadedAsync()
    {
        if (IsLoaded) return;
        try
        {
            var json = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(json))
                Progress = JsonSerializer.Deserialize<Progress>(json) ?? new Progress();
        }
        catch (JsonException)
        {
            Progress = new Progress();
        }
        IsLoaded = true;
        Changed?.Invoke();
    }

    public async Task SaveAsync()
    {
        Changed?.Invoke();
        try
        {
            await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, JsonSerializer.Serialize(Progress));
        }
        catch (JSException)
        {
            // Private browsing can block storage; progress then lasts for this visit only.
        }
    }

    public Task CompleteLessonAsync(string id) =>
        Progress.CompletedLessons.Add(id) ? SaveAsync() : Task.CompletedTask;

    public async Task ResetAsync()
    {
        Progress = new Progress();
        await SaveAsync();
    }
}
