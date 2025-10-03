using Bikehub.Client.Models;
using CommunityToolkit.Mvvm.Input;

namespace Bikehub.Client.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}