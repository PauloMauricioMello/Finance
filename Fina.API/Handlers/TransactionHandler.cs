using Fina.API.Data;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Fina.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Fina.Core.Common;

namespace Fina.API.Handlers;

public class TransactionHandler(AppDbContext context) :ITransactionHandler
{
      public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
      {
            if(request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
                  request.Amount *= -1;

            var transaction = new Transaction
            {
                  UserId = request.UserId,
                  CategoryId = request.CategoryId,
                  CreatedAt = DateTime.Now,
                  Amount = request.Amount,
                  PaidOrReceivedAt = request.PaidReceivedAt,
                  Title = request.Title,
                  Type = request.Type,
            };

            try
            {
                  await context.Transactions.AddAsync(transaction);
                  await context.SaveChangesAsync();

                  return new Response<Transaction?>(transaction, 201, "Transação craiada com sucesso!");

            }
            catch
            {
                  return new Response<Transaction?>(null, 500, "Não foi possível criar essa transação!");
            }
      }

      public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
      {
            try
            {
                  var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                  if(transaction == null) return new Response<Transaction?>(null, 404, "Transação não encontrada!");

                  context.Transactions.Remove(transaction);
                  await context.SaveChangesAsync();

                  return new Response<Transaction?>(transaction, message: "Transação excluída com sucesso!");
            }
            catch
            {
                  return new Response<Transaction?>(null, 500, "Não foi possível excluir essa transação!");
            }
      }

      public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
      {
            try
            {
                  var transaction = await context.Transactions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                  return transaction is null
                        ? new Response<Transaction?>(null, 404, "Transação não encontrada")
                        : new Response<Transaction?>(transaction);
            }
            catch
            {
                  return new Response<Transaction?>(null, 500, "Não foi possível recuperar a transação!");
            }
      }

      public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionByPeriodRequest request)
      {
            try
            {
                  request.StartDate ??= DateTime.Now.GetFirstDay();
                  request.EndDate ??= DateTime.Now.GetLastDay();
            }
            catch
            {
                  return new PagedResponse<List<Transaction>?>(null, 500, "Não foi possível determinar a data de início e fim!");
            }

            try
            {
                  var query = context.Transactions
                                    .AsNoTracking()
                                    .Where(x => 
                                    x.PaidOrReceivedAt >= request.StartDate &&
                                    x.PaidOrReceivedAt <= request.EndDate &&
                                    x.UserId == request.UserId)
                                    .OrderBy(x => x.PaidOrReceivedAt);

                  var transation = await query
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync();

                  var count = await query.CountAsync();

                  return new PagedResponse<List<Transaction>?>(
                        transation,
                        count,
                        request.PageNumber,
                        request.PageSize);
            }
            catch
            {
                  return new PagedResponse<List<Transaction>?>(null, 500, "Não foi possível encontrar os registros");
            }
      }

      public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
      {
            if(request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
                  request.Amount *= -1;

            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if(transaction == null) return new Response<Transaction?>(null, 404, "Transação não encontrada");

            try
            {
                  transaction.CategoryId = request.CategoryId;
                  transaction.Amount = request.Amount;
                  transaction.Title = request.Title;
                  transaction.Type = request.Type;
                  transaction.PaidOrReceivedAt = request.PaidReceivedAt;

                  context.Transactions.Update(transaction);
                  await context.SaveChangesAsync();

                  return new Response<Transaction?>(transaction, message: "Transação atualizada com sucesso!");
            }
            catch
            {
                  return new Response<Transaction?>(null, 500, "Não foi possível atualizar essa transação!");
            }
      }
}
