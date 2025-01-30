namespace GamaEdtech.Backend.DomainService
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using EntityFramework.Exceptions.Common;

    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions.Linq;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Specification;
    using Farsica.Framework.DataAccess.UnitOfWork;
    using Farsica.Framework.Service;

    using GamaEdtech.Backend.Data.Dto.Board;
    using GamaEdtech.Backend.Data.Entity;
    using GamaEdtech.Backend.Shared.Service;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static Farsica.Framework.Core.Constants;

    public class BoardService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<FileService>> localizer
        , Lazy<ILogger<FileService>> logger)
        : LocalizableServiceBase<FileService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IBoardService
    {
        public async Task<ResultData<ListDataSource<BoardsDto>>> GetBoardsAsync(ListRequestDto<Board>? requestDto = null)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var result = await uow.GetRepository<Board, int>().GetManyQueryable(requestDto?.Specification).FilterListAsync(requestDto?.PagingDto);
                var users = await result.List.Select(t => new BoardsDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Icon = t.Icon,
                }).ToListAsync();
                return new(OperationResult.Succeeded) { Data = new() { List = users, TotalRecordsCount = result.TotalRecordsCount } };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<BoardDto>> GetBoardAsync([NotNull] ISpecification<Board> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var board = await uow.GetRepository<Board, int>().GetManyQueryable(specification).Select(t => new BoardDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Icon = t.Icon,
                }).FirstOrDefaultAsync();

                return board is null
                    ? new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["BoardNotFound"] },],
                    }
                    : new(OperationResult.Succeeded) { Data = board };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<int>> ManageBoardAsync([NotNull] ManageBoardRequestDto requestDto)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<Board, int>();
                Board? board = null;

                if (requestDto.Id.HasValue)
                {
                    board = await repository.GetAsync(requestDto.Id.Value);
                    if (board is null)
                    {
                        return new(OperationResult.NotFound)
                        {
                            Errors = [new() { Message = Localizer.Value["BoardNotFound"] },],
                        };
                    }

                    board.Title = requestDto.Title;
                    board.Description = requestDto.Description;
                    board.Icon = requestDto.Icon;

                    _ = repository.Update(board);
                }
                else
                {
                    board = new Board
                    {
                        Title = requestDto.Title,
                        Description = requestDto.Description,
                        Icon = requestDto.Icon,
                    };
                    repository.Add(board);
                }

                _ = await uow.SaveChangesAsync();

                return new(OperationResult.Succeeded) { Data = board.Id };
            }
            catch (ReferenceConstraintException)
            {
                return new(OperationResult.NotValid) { Errors = [new() { Message = Localizer.Value["InvalidStateId"], }] };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> RemoveBoardAsync([NotNull] ISpecification<Board> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var board = await uow.GetRepository<Board, int>().GetAsync(specification);
                if (board is null)
                {
                    return new(OperationResult.NotFound)
                    {
                        Data = false,
                        Errors = [new() { Message = Localizer.Value["BoardNotFound"] },],
                    };
                }

                uow.GetRepository<Board, int>().Remove(board);
                _ = await uow.SaveChangesAsync();
                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (ReferenceConstraintException)
            {
                return new(OperationResult.NotValid) { Errors = [new() { Message = Localizer.Value["BoardCantBeRemoved"], },] };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, },] };
            }
        }
    }
}
