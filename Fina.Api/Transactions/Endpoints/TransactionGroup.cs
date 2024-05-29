using Fina.Api.Transactions.Endpoints.Movements;
using Fina.Api.Transactions.Endpoints.Transactions;
using Fina.Core.Common.Responses;
using Fina.Core.Transactions.Models;
using Fina.Core.Transactions.Requests.Movement;
using Fina.Core.Transactions.Requests.Transaction;

namespace Fina.Api.Transactions.Endpoints;

public static class TransactionGroup
{
    public static void MapTransactionEndpoints(this RouteGroupBuilder builder)
    {
        #region Transaction

        builder.MapPost("", CreateTransactionEndpoint.HandleAsync)
            .WithName("CreateTransaction")
            .WithDescription("Cria uma nova transação.")
            .WithDisplayName("Criar Transação")
            .WithTags("Transactions")
            .Accepts<CreateTransactionRequest>("application/json")
            .Produces<Response<Transaction?>>()
            .WithOpenApi();

        builder.MapPut("", UpdateTransactionEndpoint.HandleAsync)
            .WithName("UpdateTransaction")
            .WithDescription("Atualiza uma transação existente.")
            .WithDisplayName("Atualizar Transação")
            .WithTags("Transactions")
            .Accepts<UpdateTransactionRequest>("application/json")
            .Produces<Response<Transaction?>>()
            .WithOpenApi();
        
        builder.MapDelete("{id}", DeleteTransactionEndpoint.HandleAsync)
            .WithName("DeleteTransaction")
            .WithDescription("Deleta uma transação existente.")
            .WithDisplayName("Deletar Transação")
            .WithTags("Transactions")
            .Produces<Response<Transaction?>>()
            .WithOpenApi();

        builder.MapGet("{id}", GetTransactionByIdEndpoint.HandleAsync)
            .WithName("GetTransactionById")
            .WithDescription("Obtém uma transação pelo ID.")
            .WithDisplayName("Obter Transação por ID")
            .WithTags("Transactions")
            .Produces<Response<Transaction?>>()
            .WithOpenApi();

        builder.MapGet("/by-period", GetTransactionsByPeriodEndpoint.HandleAsync)
            .WithName("GetTransactionsByPeriod")
            .WithDescription("Obtém todas as transações de um período.")
            .WithDisplayName("Obter Transações por Período")
            .WithTags("Transactions")
            .Produces<PagedResponse<List<Transaction>>>()
            .WithOpenApi();

        #endregion

        #region Movement

        builder.MapPost("movements", CreateMovementEndpoint.HandleAsync)
            .WithName("CreateMovement")
            .WithDescription("Cria um movimento vinculado a uma transação")
            .WithDisplayName("Criar Movimento")
            .WithTags("Transactions", "Movements")
            .Accepts<CreateMovementRequest>("application/json")
            .Produces<Response<Movement?>>()
            .WithOpenApi();

        builder.MapPut("movements", UpdateMovementEndpont.HandleAsync)
            .WithName("UpdateMovement")
            .WithDescription("Atualiza um movimento de uma transação.")
            .WithDisplayName("Atualizar Movimento")
            .WithTags("Transactions", "Movements")
            .Accepts<UpdateMovementRequest>("application/json")
            .Produces<Response<Movement?>>()
            .WithOpenApi();
        
        builder.MapDelete("movements/{id}", DeleteMovementEndpoint.HandleAsync)
            .WithName("DeleteMovement")
            .WithDescription("Deleta um movimento de uma transação por ID")
            .WithDisplayName("Deleta Movimento")
            .WithTags("Transactions", "Movements")
            .Produces<Response<Movement?>>()
            .WithOpenApi();

        builder.MapGet("movements/{id}", GetMovementByIdEndpoint.HandleAsync)
            .WithName("GetMovementById")
            .WithDescription("Obtém um movimento pelo ID.")
            .WithDisplayName("Obter Movimento por ID")
            .WithTags("Transactions", "Movements")
            .Produces<Response<Movement?>>()
            .WithOpenApi();

        builder.MapGet("{transactionId}/movements", GetMovementsByTransactionIdEndpoint.HandleAsync)
            .WithName("GetMovementsByTransactionId")
            .WithDescription("Obtém todos os movimentos de uma transação.")
            .WithDisplayName("Obter Movimentos por ID da Transação")
            .WithTags("Transactions", "Movements")
            .Produces<PagedResponse<List<Movement>>>()
            .WithOpenApi();

        #endregion
    }
}