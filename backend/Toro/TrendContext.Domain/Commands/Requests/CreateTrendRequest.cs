﻿using Flunt.Notifications;
using Flunt.Validations;
using MediatR;
using Newtonsoft.Json;
using TrendContext.Domain.Commands.Responses;
using TrendContext.Shared.Commands;

namespace TrendContext.Domain.Commands.Requests
{
    public class CreateTrendRequest : Notifiable<Notification>, IRequest<CreateTrendResponse>, ICommand
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("currentPrice")]
        public decimal CurrentPrice { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Symbol, "CreateTrendRequest.Symbol", "Symbol is required.")
                .IsGreaterThan(CurrentPrice, decimal.Zero, "CreateTrendRequest.CurrentPrice", "CurrentPrice is invalid")
                .IsFalse(Symbol.Length < 5 && Symbol.Length > 8, Symbol, "Symbol is invalid.")
            );
        }
    }
}
