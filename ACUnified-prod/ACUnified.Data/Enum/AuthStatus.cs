using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.Enum
{
    public enum AuthStatus
    {
        DefaultPW=1,
        ConfirmationCodeSent,
        ConfirmationCodeUsed,
        PWChangeCompleted
    }
}
