﻿using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.AccountsModule.User;
using API.Modules.PaymentsModule.Model;
using API.Modules.TariffsModule.Models;

namespace API.Modules.InvoiceModule.Model;

public class InvoiceEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public AccountEntity Account { get; set; }
    public Guid TariffId { get; set; }
    public TariffEntity Tariff { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? PaymentId { get; set; }
    public PaymentEntity? Payment { get; set; }
}