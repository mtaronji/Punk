using System;
using System.Collections.Generic;

namespace Punk.SP500OptionModels;

public partial class SequenceComplete
{
    public string Code { get; set; } = null!;

    public int IsComplete { get; set; }

    public virtual OptionCode CodeNavigation { get; set; } = null!;
}
