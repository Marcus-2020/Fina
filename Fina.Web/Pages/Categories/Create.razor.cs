using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Requests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Fina.Web.Pages.Categories;

public partial class CreateCategoryPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public CreateCategoryRequest InputModel { get; set; } = new();

    #endregion

    #region Services

    [Inject] public ICategoryHandler Handler { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    
    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        try
        {
            IsBusy = true;
            
            var response = await Handler.CreateAsync(InputModel);
            
            if (!response.IsSuccess)
            {
                Snackbar.Add(response.Message, Severity.Error);   
                return;
            }
            
            Snackbar.Add(response.Message, Severity.Success);
            NavigationManager.NavigateTo("/categorias");
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
}