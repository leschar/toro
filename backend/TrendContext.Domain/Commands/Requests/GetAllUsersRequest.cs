﻿using MediatR;
using System;
using System.Collections.Generic;
using TrendContext.Domain.Commands.Responses;

namespace TrendContext.Domain.Commands.Requests
{
    public class GetAllUsersRequest : IRequest<CommandResponse<IEnumerable<GetAllUsersResponse>>>
    {
    }
}
