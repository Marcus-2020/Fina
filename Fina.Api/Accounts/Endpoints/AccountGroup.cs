using Fina.Core.Accounts.Models;
using Fina.Core.Common.Responses;

namespace Fina.Api.Accounts.Endpoints;

public static class AccountGroup
{
    public static void MapAccountEndpoints(this RouteGroupBuilder builder)
    {
        builder.MapPost("", CreateAccountEndpoint.HandleAsync)
            .WithName("CreateAccount")
            .WithDescription("Cria uma nova conta que contém um saldo inicial e pertence a um usuário.")
            .WithDisplayName("Criar Conta")
            .WithTags("Accounts")
            .Produces<Response<Account?>>()
            .WithOpenApi();
        
        builder.MapPut("", UpdateAccountEndpoint.HandleAsync)
            .WithName("UpdateAccount")
            .WithDescription("Atualiza uma conta existente.")
            .WithDisplayName("Atualizar Conta")
            .WithTags("Accounts")
            .Produces<Response<Account?>>()
            .WithOpenApi();
        
        builder.MapDelete("{id}", DeleteAccountEndpoint.HandleAsync)
            .WithName("DeleteAccount")
            .WithDescription("Deleta uma conta existente.")
            .WithDisplayName("Deletar Conta")
            .WithTags("Accounts")
            .Produces<Response<Account?>>()
            .WithOpenApi();
        
        builder.MapGet("{id}", GetAccountByIdEndpoint.HandleAsync)
            .WithName("GetAccountById")
            .WithDescription("Obtém uma conta pelo ID.")
            .WithDisplayName("Obter Conta por ID")
            .WithTags("Accounts")
            .Produces<Response<Account?>>()
            .WithOpenApi();
        
        builder.MapGet("", GetAllAccountsEndpoint.HandleAsync)
            .WithName("GetAllAccounts")
            .WithDescription("Obtém todas as contas.")
            .WithDisplayName("Obter Todas as Contas")
            .WithTags("Accounts")
            .Produces<PagedResponse<List<Account>>>()
            .WithOpenApi();
    }
}