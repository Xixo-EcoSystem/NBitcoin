using System;
using NBitcoin.RPC;
using Xunit;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
namespace NBitcoin.Tests
{
	public class MyTest
	{
		
		public MyTest()
		{
			
		}

		[Fact]
		public void CanSendCommand()
		{
			using (var builder = NodeBuilderEx.Create())
			{
				var rpc = builder.CreateNode().CreateRPCClient();
				builder.StartAll();
				var response = rpc.SendCommand(RPCOperations.getblockchaininfo);
				Assert.NotNull(response.Result);
				//var copy = RPCCredentialString.Parse(rpc.CredentialString.ToString());
				//copy.Server = rpc.Address.AbsoluteUri;
				//rpc = new RPCClient(rpc.CredentialString, null as string, builder.Network);
				//response = rpc.SendCommand(RPCOperations.getblockchaininfo);
				//Assert.NotNull(response.Result);
			}
		}

		[Fact]
		public void Test()
		{
			using (var builder = NodeBuilderEx.Create())
			{
				string senderaddress = "bc1qtf5ghqu2t3muamk94n350xavvykszg042ts9hl";
				string privatetestnet = "dress voyage game special dash just middle kitten cake harbor text stage";

				builder.StartAll();
			}
		}
	}
}

