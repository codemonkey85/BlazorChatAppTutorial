using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorChatAppTutorial.Client
{
    public class JsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        #region Constructor / Destructor

        public JsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", $"./js/JsInterop.js").AsTask());
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                IJSObjectReference module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }

        #endregion

        #region Private methods

        private async Task<T> InvokeAsync<T>(string method, params object[] args)
        {
            IJSObjectReference module = await moduleTask.Value;
            return await module.InvokeAsync<T>(method, args);
        }

        private async Task InvokeVoidAsync(string method, params object[] args)
        {
            IJSObjectReference module = await moduleTask.Value;
            await module.InvokeVoidAsync(method, args);
        }

        #endregion
    }
}
