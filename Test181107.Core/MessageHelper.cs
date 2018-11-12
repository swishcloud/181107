using System;
using System.Collections.Generic;
using System.Text;

namespace Test181107.Core
{
    public static class MessageHelper
    {
        public static Encoding LiteralEncoding => Encoding.UTF8;
        public static MessagePayload CreateLiteralMessage(string literal, string from, string to, string mark)
        {
            var bytes = LiteralEncoding.GetBytes(literal);
            return new BytesMessage(bytes, MessageKeys.Literal, from, to, mark);
        }
        public static MessagePayload CreateLoginMessage(string userName, string mark)
        {
            var bytes = LiteralEncoding.GetBytes("none");
            return new BytesMessage(bytes, MessageKeys.Login, userName, null, mark);
        }
        public static MessagePayload CreateRemoteMessage(string from, string to, string mark)
        {
            var header = new Header() { From = from, To = to, Key = MessageKeys.Remote, Mark = mark };
            return new InstructMessage(header);
        }
        public static MessagePayload CreateRemoteStopMessage(string from, string to, string mark)
        {
            var header = new Header() { From = from, To = to, Mark = mark, Key = MessageKeys.RemoteStop };
            return new InstructMessage(header);
        }
        public static MessagePayload CreateRemoteImageMessage(byte[] bytes, string from, string to, string mark)
        {
            return new BytesMessage(bytes, MessageKeys.RemoteImage, from, to, mark);
        }
        public static MessagePayload CreateCloseSessionMessage(string from, string mark)
        {
            return new InstructMessage(new Header() { Key = MessageKeys.CloseSession, From = from, Mark = mark });
        }
    }
}
