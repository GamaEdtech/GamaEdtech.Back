namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using EntityFramework.Exceptions.Common;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Core.Extensions.Linq;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Repositories;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Data.Dto.Transaction;
    using GamaEdtech.Domain.Entity;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public class TransactionService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<TransactionService>> localizer
        , Lazy<ILogger<TransactionService>> logger)
        : LocalizableServiceBase<TransactionService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), ITransactionService
    {
        public async Task<ResultData<ListDataSource<TransactionDto>>> GetTransactionsAsync(ListRequestDto<Transaction>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<Transaction>().GetManyQueryable(requestDto?.Specification).FilterListAsync(requestDto?.PagingDto);
                var users = await result.List.Select(t => new TransactionDto
                {
                    Id = t.Id,
                    CreationDate = t.CreationDate,
                    CurrentBalance = t.CurrentBalance,
                    Description = t.Description,
                    IsDebit = t.IsDebit,
                    Points = t.Points,
                }).ToListAsync();
                return new(OperationResult.Succeeded) { Data = new() { List = users, TotalRecordsCount = result.TotalRecordsCount } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<TransactionDto>> GetTransactionAsync([NotNull] ISpecification<Transaction> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<Transaction>().GetManyQueryable(specification).Select(t => new TransactionDto
                {
                    Id = t.Id,
                    CreationDate = t.CreationDate,
                    UserId = t.UserId,
                    CurrentBalance = t.CurrentBalance,
                    Description = t.Description,
                    IsDebit = t.IsDebit,
                    Points = t.Points,
                }).FirstOrDefaultAsync();

                return new(OperationResult.Succeeded) { Data = result };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<long>> IncreaseBalanceAsync([NotNull] CreateTransactionRequestDto requestDto) => await CreateTransactionInternalAsync(requestDto, false);

        public async Task<ResultData<long>> DecreaseBalanceAsync([NotNull] CreateTransactionRequestDto requestDto) => await CreateTransactionInternalAsync(requestDto, true);

        public async Task<ResultData<int>> GetCurrentBalanceAsync([NotNull] GetCurrentBalanceRequestDto requestDto)
        {
            try
            {
                var repository = UnitOfWorkProvider.Value.CreateUnitOfWork().GetRepository<Transaction>();
                var data = await GetCurrentBalanceInternalAsync(requestDto, repository);

                return new(OperationResult.Succeeded) { Data = data.Data.Balance };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        private async Task<ResultData<(int Balance, long? TransactionId)>> GetCurrentBalanceInternalAsync([NotNull] GetCurrentBalanceRequestDto requestDto, IRepository<Transaction, long> repository)
        {
            try
            {
                var balance = await repository.GetManyQueryable(t => t.UserId == requestDto.UserId)
                    .OrderByDescending(t => t.Id).Select(t => new { t.CurrentBalance, t.Id }).FirstOrDefaultAsync();

                return new(OperationResult.Succeeded) { Data = (balance?.CurrentBalance ?? 0, balance?.Id) };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        private async Task<ResultData<long>> CreateTransactionInternalAsync([NotNull] CreateTransactionRequestDto requestDto, bool isDebit)
        {
            IUnitOfWork? uow = null;
            IRepository<Transaction, long>? repository = null;

            var result = await SaveInternalAsync();
            return result.OperationResult switch
            {
                OperationResult.Succeeded => result,
                OperationResult.Duplicate => await SaveInternalAsync(),
                _ => result,
            };

            async Task<ResultData<long>> SaveInternalAsync()
            {
                try
                {
                    uow ??= UnitOfWorkProvider.Value.CreateUnitOfWork();
                    repository ??= uow.GetRepository<Transaction>();

                    var previousTransaction = await GetCurrentBalanceInternalAsync(new() { UserId = requestDto.UserId }, repository);
                    var factor = isDebit ? (requestDto.Points * -1) : requestDto.Points;
                    var transaction = new Transaction
                    {
                        PreviousTransactionId = previousTransaction.Data.TransactionId,
                        IdentifierId = requestDto.IdentifierId,
                        CreationDate = DateTimeOffset.UtcNow,
                        CurrentBalance = factor + previousTransaction.Data.Balance,
                        Description = $"{(isDebit ? "Decrease" : "Increase")} Balance by {requestDto.Description}",
                        IsDebit = isDebit,
                        Points = requestDto.Points,
                        UserId = requestDto.UserId,
                    };
                    repository.Add(transaction);
                    _ = await uow.SaveChangesAsync();

                    return new(OperationResult.Succeeded) { Data = transaction.Id };
                }
                catch (UniqueConstraintException)
                {
                    return new(OperationResult.Duplicate) { Errors = [new() { Message = "Duplicate Data, please try again", }] };
                }
                catch (Exception exc)
                {
                    Logger.Value.LogException(exc);
                    return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
                }
            }
        }
    }
}
