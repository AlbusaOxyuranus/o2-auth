using System;

namespace O2.Auth.Web.Utilities
{
    public interface IBase64QrCodeGenerator 
    { 
        string Generate(Uri target); 
    }
}