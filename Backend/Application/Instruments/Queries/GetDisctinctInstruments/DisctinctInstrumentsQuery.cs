using Application.Abstractions.Messaging;
using SharedLib.Models.Common;
using System.Collections.Generic;

namespace Application.Instruments.Queries.GetDisctinctInstruments;

public sealed record DisctinctInstrumentsQuery(): IQuery<IEnumerable<SelectItem>>;