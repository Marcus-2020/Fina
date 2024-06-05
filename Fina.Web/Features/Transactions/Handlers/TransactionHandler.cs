using System.Net.Http.Json;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Handlers;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests;

namespace Fina.Web.Features.Transactions.Handlers;

public class TransactionHandler : ITransactionHandler
{
    private readonly HttpClient _client;

    public TransactionHandler(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);
    }

    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {
            HttpResponseMessage result = await _client.PostAsJsonAsync("v1/transactions", request);
            return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ??
                   new(null, 400, "Falha ao criar transação");
        }
        catch (Exception ex)
        {
            return new(null,
                500, "Ocorreu um erro interno ao tentar criar a transação");
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {
            HttpResponseMessage result = await _client.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
            return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ??
                   new(null, 400, "Falha ao atualizar transação");
        }
        catch (Exception ex)
        {
            return new(null, 500,
                "Ocorreu um erro interno ao tentar atualizar a transação");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            HttpResponseMessage result = await _client.DeleteAsync($"v1/transactions/{request.Id}");
            return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ??
                   new(null, 400, "Falha ao excluir transação");
        }
        catch (Exception ex)
        {
            return new(null, 500,
                "Ocorreu um erro interno ao tentar excluir a transação");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            return await _client.GetFromJsonAsync<Response<Transaction?>>($"v1/transactions/{request.Id}") ??
                   new(null, 400, "Falha ao obter transação por ID");
        }
        catch (Exception ex)
        {
            return new(null, 500,
                "Ocorreu um erro interno ao tentar obter a transação por ID");
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetAllAsync(GetTransactionByPeriodRequest request)
    {
        try
        {
            string url = $"v1/transactions?pageSize={request.PageSize}&pageNumber={request.PageNumber}";
            
            if (request.StartDate.HasValue) url += $"&startDate={request.StartDate.Value.ToString("yyyy-MM-dd")}";
            if (request.EndDate.HasValue) url += $"&endDate={request.EndDate.Value.ToString("yyyy-MM-dd")}";

            return await _client.GetFromJsonAsync<PagedResponse<List<Transaction>?>>(url) ??
                   new(null, 400, "Falha ao obter transações");
        }
        catch (Exception ex)
        {
            return new(null, 500, 
                "Ocorreu um erro interno ao tentar obter as transações");
        }
    }
}