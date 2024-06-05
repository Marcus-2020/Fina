using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Models;
using Fina.Core.Categories.Requests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Fina.Web.Pages.Categories;

public partial class GetAllCategoriesPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public List<Category> Categories { get; set; }
    public GetAllCategoriesRequest InputModel { get; set; } = new();

    #endregion

    #region Services

    [Inject] public ICategoryHandler Handler { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public IDialogService Dialog { get; set; } = null!;
    
    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsBusy = true;
            
            var request = new GetAllCategoriesRequest();
            var result = await Handler.GetAllAsync(request);

            if (!result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Error);
                return;
            }

            Categories = result.Data ?? [];
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

    #region Methods

    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        var result = await Dialog.ShowMessageBox("Atenção", 
            $"Ao prosseguir a categoria {title} será removida. Deseja continuar?",
            "Excluir",
            "Cancelar");

        if (result is true) await OnDeleteAsync(id, title);
    }

    public async Task OnDeleteAsync(long id, string title)
    {
        var response = await Handler.DeleteAsync(new DeleteCategoryRequest { Id = id });
        
        if (!response.IsSuccess)
        {
            Snackbar.Add(response.Message, Severity.Error);
            return;
        }

        Snackbar.Add($"Categoria {title} removida com sucesso.", Severity.Info);
        Categories.RemoveAll(c => c.Id == id);
        
        StateHasChanged();
    }

    #endregion
}