using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.BAL.Common
{

    public enum CustomHttpStatusCode 
    {
        // Summary:
        //     Equivalent to HTTP status 100. System.Net.HttpStatusCode.Continue indicates
        //     that the client can continue with its request.
        Success = 1,

        DataNotFound = 2,

        Duplicate=3,
        //
        // Summary:
        //     Equivalent to HTTP status 101. System.Net.HttpStatusCode.SwitchingProtocols
        //     indicates that the protocol version or protocol is being changed.
        Error = 4,
        InvalidInput=5
        //
        // Summary:
        //     Equivalent to HTTP status 200. System.Net.HttpStatusCode.OK indicates that
        //     the request succeeded and that the requested information is in the response.
        //     This is the most common status code to receive.

    }
    /// <summary>
    /// Enum For Store Procedure Status
    /// </summary>
    public enum StoreProcedureStatus
    {
        Success=1,
        Duplicate=-2,
        Error=-1,
    }

  
}
