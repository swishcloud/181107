using System;
using System.Collections.Generic;
using System.Text;

namespace Test181107.Core
{
    public static class Env
    {
        public static Action<string> Print { get; private set; }
        static bool initialized;
        public static void Initialize(Action<string> print)
        {
            if(initialized)
            {
                throw new Exception("you don't have a second chance");
            }
            initialized = true;
            Env.Print = print;
        }

    }
}
