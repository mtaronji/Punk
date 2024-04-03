using System;
using System.Collections.Generic;

namespace Punk.SP500OptionModels;

public partial class OptionCode
{
    public string Code { get; set; } = null!;

    public virtual SequenceComplete? SequenceComplete { get; set; }
}
