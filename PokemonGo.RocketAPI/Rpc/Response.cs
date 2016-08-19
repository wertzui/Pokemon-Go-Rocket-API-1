using Google.Protobuf;
using Google.Protobuf.Collections;
using POGOProtos.Networking.Envelopes;
using System;

namespace PokemonGo.RocketAPI.Rpc
{
    public class Response
    {
        protected IMessage[] returnMessages;

        public Response(ResponseEnvelope response, Type[] responseTypes)
        {
            ApiUrl = response.ApiUrl;
            AuthTicket = response.AuthTicket;
            ParseMessages(response.Returns, responseTypes);
        }
        public Response(string apiUrl, AuthTicket authTicket, IMessage[] returnMessages)
        {
            ApiUrl = apiUrl;
            AuthTicket = authTicket;
            this.returnMessages = returnMessages;
        }

        public string ApiUrl { get; set; }
        public AuthTicket AuthTicket { get; set; }

        void ParseMessages(RepeatedField<ByteString> returns, Type[] returnTypes)
        {
            if (returnMessages == null)
            {
                if (returns == null)
                    throw new ArgumentNullException(nameof(returns));
                if (returnTypes == null)
                    throw new ArgumentNullException(nameof(returnTypes));
                if (returns.Count != returnTypes.Length)
                    throw new ArgumentOutOfRangeException(nameof(returnTypes), $"{nameof(returnTypes)} must have the same length as {nameof(returns)}.");

                var result = new IMessage[returnTypes.Length];
                for (var i = 0; i < returnTypes.Length; i++)
                {
                    result[i] = Activator.CreateInstance(returnTypes[i]) as IMessage;
                    if (result[i] == null)
                    {
                        throw new ArgumentException($"returnType[{i}] is not an IMessage");
                    }

                    var payload = returns[i];
                    result[i].MergeFrom(payload);
                }

                returnMessages = result;
            }
        }

        public Response<T1> MakeGeneric<T1>() => new Response<T1>(ApiUrl, AuthTicket, returnMessages);
        public Response<T1, T2> MakeGeneric<T1, T2>() => new Response<T1, T2>(ApiUrl, AuthTicket, returnMessages);
        public Response<T1, T2, T3> MakeGeneric<T1, T2, T3>() => new Response<T1, T2, T3>(ApiUrl, AuthTicket, returnMessages);
        public Response<T1, T2, T3, T4> MakeGeneric<T1, T2, T3, T4>() => new Response<T1, T2, T3, T4>(ApiUrl, AuthTicket, returnMessages);
        public Response<T1, T2, T3, T4, T5> MakeGeneric<T1, T2, T3, T4, T5>() => new Response<T1, T2, T3, T4, T5>(ApiUrl, AuthTicket, returnMessages);
    }

    public class Response<T1> : Response
    {
        public Response(ResponseEnvelope response) : this(response, new[] { typeof(T1) }) { }
        protected Response(ResponseEnvelope response, Type[] responseTypes) : base(response, responseTypes) { }
        public Response(string apiUrl, AuthTicket authTicket, IMessage[] returnMessages) : base(apiUrl, authTicket, returnMessages) { }
        public T1 Item1 => (T1)returnMessages[0];
    }

    public class Response<T1, T2> : Response<T1>
    {
        public Response(ResponseEnvelope response) : this(response, new[] { typeof(T1), typeof(T2) }) { }
        protected Response(ResponseEnvelope response, Type[] responseTypes) : base(response, responseTypes) { }
        public Response(string apiUrl, AuthTicket authTicket, IMessage[] returnMessages) : base(apiUrl, authTicket, returnMessages) { }

        public T2 Item2 => (T2)returnMessages[1];
    }

    public class Response<T1, T2, T3> : Response<T1, T2>
    {
        public Response(ResponseEnvelope response) : this(response, new[] { typeof(T1), typeof(T2), typeof(T3) }) { }
        protected Response(ResponseEnvelope response, Type[] responseTypes) : base(response, responseTypes) { }
        public Response(string apiUrl, AuthTicket authTicket, IMessage[] returnMessages) : base(apiUrl, authTicket, returnMessages) { }

        public T3 Item3 => (T3)returnMessages[2];
    }

    public class Response<T1, T2, T3, T4> : Response<T1, T2, T3>
    {
        public Response(ResponseEnvelope response) : this(response, new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }) { }
        protected Response(ResponseEnvelope response, Type[] responseTypes) : base(response, responseTypes) { }
        public Response(string apiUrl, AuthTicket authTicket, IMessage[] returnMessages) : base(apiUrl, authTicket, returnMessages) { }

        public T4 Item4 => (T4)returnMessages[3];
    }

    public class Response<T1, T2, T3, T4, T5> : Response<T1, T2, T3, T4>
    {
        public Response(ResponseEnvelope response) : this(response, new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }) { }
        protected Response(ResponseEnvelope response, Type[] responseTypes) : base(response, responseTypes) { }
        public Response(string apiUrl, AuthTicket authTicket, IMessage[] returnMessages) : base(apiUrl, authTicket, returnMessages) { }

        public T5 Item5 => (T5)returnMessages[4];
    }
}