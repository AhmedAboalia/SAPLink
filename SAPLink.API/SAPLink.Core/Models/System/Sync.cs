﻿
namespace SAPLink.Core.Models.System;

public class Sync
{
    [Key]
    public UpdateType UpdateType { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
}