﻿using Flunt.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrendContext.Domain.Commands.Requests;
using TrendContext.Domain.Commands.Responses;
using TrendContext.Domain.Data.Interfaces;
using TrendContext.Domain.Entities;
using TrendContext.Domain.Repositories.Interfaces;
using TrendContext.Domain.Services;

namespace TrendContext.Domain.Handlers
{
    public class CreateSessionHandler : Notifiable<Notification>, IRequestHandler<CreateSessionRequest, CommandResponse<CreateSessionResponse>>
    {
        private readonly IUserRepository repository;
        private readonly ITokenService tokenService;
        private readonly ILogger<CreateSessionHandler> logger;

        public CreateSessionHandler(IUserRepository repository,
            ITokenService tokenService,
            ILogger<CreateSessionHandler> logger)
        {
            this.repository = repository;
            this.tokenService = tokenService;
            this.logger = logger;
        }

        public async Task<CommandResponse<CreateSessionResponse>> Handle(CreateSessionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                request.Validate();

                if(!request.IsValid)
                {
                    return new CommandResponse<CreateSessionResponse>(false, 400, string.Join(Environment.NewLine, request.Notifications.Select(x => x.Message)), null);
                }

                var existsUser = await repository.GetByCPFAsync(request.CPF);

                if (existsUser == null)
                {
                    return new CommandResponse<CreateSessionResponse>(false, 400, "User not found.", null);
                }

                var token = tokenService.GenerateToken(existsUser);

                return new CommandResponse<CreateSessionResponse>(true, 201, string.Empty,
                    new CreateSessionResponse
                    {
                        Id = existsUser.Id,
                        Name = existsUser.Name,
                        CPF = existsUser.CPF,
                        Token = token,
                    });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new CommandResponse<CreateSessionResponse>(false, 500, "Internal Server Error", null);
            }
        }
    }
}
