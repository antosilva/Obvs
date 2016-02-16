﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NUnit.Framework;
using Obvs.Serialization;
using Obvs.Serialization.ProtoBuf;
using Obvs.Types;
using ProtoBuf;

namespace Obvs.NetMQ.Tests.Console.Publisher
{
	class Program
	{
		static void Main(string[] args)
		{
			int max = 5;
			CountdownEvent cd = new CountdownEvent(max);

			string endPoint = "tcp://localhost:5557";
			System.Console.WriteLine("Publishing on {0}\n", endPoint);

			var context = NetMQContext.Create();
			const string topic = "TestTopicxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

			{
				var publisher = new MessagePublisher<IMessage>("tcp://localhost:5557",
					new ProtoBufMessageSerializer(),
					context,
					topic);

				for (int i = 0; i < max; i++)
				{
					publisher.PublishAsync(new TestMessageWhereTypeIsVeryMuchDefinitionLongerThen32Characters()
					{
						Id = i
					});

					Thread.Sleep(TimeSpan.FromSeconds(0.5));
					System.Console.WriteLine("Published: {0}", i);
				}
			}
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
