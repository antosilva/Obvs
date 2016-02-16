﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using Obvs.Serialization;
using Obvs.Serialization.ProtoBuf;
using Obvs.Types;
using ProtoBuf;
using System;

namespace Obvs.NetMQ.Tests.Console.Subscriber
{
	class Program
	{
		static void Main(string[] args)
		{
			string endPoint = "tcp://localhost:5557";
			System.Console.WriteLine("Listening on {0}\n", endPoint);

			var context = NetMQContext.Create();
			const string topic = "TestTopicxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

			IDisposable sub;
			{
				var source = new MessageSource<IMessage>(endPoint,
					new IMessageDeserializer<IMessage>[]
					{
						new ProtoBufMessageDeserializer<TestMessageWhereTypeIsVeryMuchDefinitionLongerThen32Characters>(),
					},
					context,
					topic);

				sub = source.Messages.Subscribe(msg =>
					{
						System.Console.WriteLine("Received: " + msg);
					},
				   err => System.Console.WriteLine("Error: " + err));
			}

			System.Console.ReadKey();
		}
	}

	[ProtoContract]
	public class TestMessageWhereTypeIsVeryMuchDefinitionLongerThen32Characters : IMessage
	{
		public TestMessageWhereTypeIsVeryMuchDefinitionLongerThen32Characters()
		{

		}

		[ProtoMember(1)]
		public int Id { get; set; }

		public override string ToString()
		{
			return "TestMessageWhereTypeIsVeryMuchDefinitionLongerThen32Characters-" + Id;
		}
	}
}
