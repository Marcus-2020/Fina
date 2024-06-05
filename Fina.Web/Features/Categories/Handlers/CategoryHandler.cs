using System.Net.Http.Json;
using System.Text.Json;
using Fina.Core.Categories.Handlers;
using Fina.Core.Categories.Models;
using Fina.Core.Categories.Requests;
using Fina.Core.Common.Responses;
using Newtonsoft.Json;

namespace Fina.Web.Features.Categories.Handlers;

public class CategoryHandler : ICategoryHandler
{
    private readonly HttpClient _client;

    public CategoryHandler(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);
    }

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
            HttpResponseMessage result = await _client.PostAsJsonAsync("v1/categories", request);
            
            var response =  await result.Content.ReadFromJsonAsync<Response<Category?>>() ?? 
                   new(null, 400, "Falha ao criar categoria");
            
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.IsSuccess);
            return response;
        }
        catch (Exception ex)
        {
            return new(null,
                500, "Ocorreu um erro interno ao tentar criar a categoria");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            HttpResponseMessage result = await _client.PutAsJsonAsync($"v1/categories/{request.Id}", request);
            return await result.Content.ReadFromJsonAsync<Response<Category?>>() ?? 
                   new(null, 400, "Falha ao atualizar categoria");
        }
        catch (Exception ex)
        {
            return new(null,
                500, "Ocorreu um erro interno ao tentar atualizar a categoria");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            HttpResponseMessage result = await _client.DeleteAsync($"v1/categories/{request.Id}");
            return await result.Content.ReadFromJsonAsync<Response<Category?>>() ??
                   new(null, 400, "Falha ao excluir a categoria");
        }
        catch (Exception ex)
        {
            return new(null,
                500, "Ocorreu um erro interno ao tentar excluir a categoria");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            return await _client.GetFromJsonAsync<Response<Category?>>($"v1/categories/{request.Id}") ??
                   new(null, 400, "Falha ao recuperar a categoria");;
        }
        catch (Exception ex)
        {
            return new(null,
                500, "Ocorreu um erro interno ao tentar recuperar a categoria");
        }
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            return await _client.GetFromJsonAsync<PagedResponse<List<Category>?>>(
                $"v1/categories?pageNumber={request.PageNumber}&pageSize={request.PageSize}") ??
                   new(null, 400, "Falha ao recuperar as categorias");
        }
        catch (Exception ex)
        {
            return new(null,
                500, "Ocorreu um erro interno ao tentar recuperar as categorias");
        }
    }
}