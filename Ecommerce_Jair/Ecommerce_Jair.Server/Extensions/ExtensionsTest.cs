using Microsoft
    
    
    .CodeAnalysis.CSharp.Syntax;
using System.Runtime.CompilerServices;

namespace Ecommerce_Jair.Server.Extensions
{
    public static class ExtensionsTest
    {
        public static void Saludar (this string mesagge){

            Console.WriteLine($"Hola string con contenido {mesagge}");

        }
    }
}
