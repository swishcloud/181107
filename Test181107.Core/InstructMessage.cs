using System;
using System.Collections.Generic;
using System.Text;

namespace Test181107.Core
{
    public class InstructMessage : MessagePayload
    {
        public InstructMessage(Header header)
        {
            this.Header = header;
        }
    }
}
