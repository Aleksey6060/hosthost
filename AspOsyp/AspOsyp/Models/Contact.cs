using System;
using System.Collections.Generic;

namespace AspOsyp.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public string ContactName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ContactMessage { get; set; } = null!;
}
