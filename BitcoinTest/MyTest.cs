using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Threading.Channels;
using BitcoinTest.Tests;
using NBitcoin;
using NBitcoin.DataEncoders;
using Xunit;

namespace BitcoinTest
{
	public class MyTest
	{
		const string senderaddresssec = ("cQD95A85P4gicNd2otxft2Nonpt6s2YTfiHWPJ3HS9FLWBomH1g9");
		const string senderlegecy = "mpzeuFXXbAGXvoUgRmSdhYWszV46NnaD7h";
		const string sendersewitbench32 = "bcrt1qvlmy9zd8tg9lay2d0qesyjq7hgtzwuz0m0sp9k";
		const string sendersewitp2sh = "2N2x5SA1dVEXPSYrhGaB3VkDa2RBUpcPxGJ";
		const string sendertabroot = "bcrt1pp7e6v08pzcv5jxss87dd3gfp2jcrcj2p9v02z05z3z26f03h83ssn590v6";

		const string receiveraddresssec = ("cPmeLJhKcQXG6gw3NnWuvvWoGbn9bFD4PUYQC4hsVgGjETBTNRzD");
		const string receiverlegecy = "mmpUgNHNrh2miSn83Dt3fZ2ARRTjGqcQ3i";
		const string receiversewitbench32 = "bcrt1qg5snp8lz8n7ylyq9lh3vzlr4mqvk6w4z750aft";
		const string receiversewitp2sh = "2NCRVWqXGjzeLXqWL64BpUCwV9UkxDWBSNf";
		const string receivertabroot = "bcrt1pk6agkq4a57hprhr4yrms6hvyrhuqj50v69dknwakt9rlr8vy4y8qvce9ud";
		public MyTest()
		{
		}
		[Fact]
		public async Task TestSendAddress()
		{
			using (var nodebuilder = NodeBuilderEx.Create())
			{
				try
				{

					// send from legacy to legacy, bench, p2sh, taproot > ok

					// send from bench to legacy, bench, p2sh, taproot > ok
					// send from taproot to legacy, bench, p2sh, taproot > ok
					var network = nodebuilder.Network;
					var rpc = nodebuilder.CreateNode().CreateRPCClient();
					nodebuilder.StartAll();

					rpc.Generate(102);

					BitcoinSecret sendersecrect = network.CreateBitcoinSecret(senderaddresssec);
					BitcoinSecret receiversecrect = network.CreateBitcoinSecret(receiveraddresssec);

					BitcoinAddress receiveraddress = BitcoinAddress.Create(receiverlegecy, network);


					var amount = new Money(1, MoneyUnit.BTC);
					uint256 id = null;
					Transaction tx = null;
					ICoin coin = null;
					TransactionBuilder builder = null;
					var rate = new FeeRate(Money.Satoshis(1), 1);
					// send to from lagecy
					async Task RefreshCoin()
					{
						id = await rpc.SendToAddressAsync(BitcoinAddress.Create(sendertabroot, network), Money.Coins(10));
						tx = await rpc.GetRawTransactionAsync(id);
						coin = tx.Outputs.AsCoins().Where(o => o.ScriptPubKey == BitcoinAddress.Create(sendertabroot, network).ScriptPubKey).Single();
						builder = network.CreateTransactionBuilder(0);
					}
					await RefreshCoin();

					//var currencsenderbalance =	rpc.GetReceivedByAddress(BitcoinAddress.Create(senderlegecy, network));

					// send to lagecy

					var signedTx = builder
						.AddCoins(coin)
						.AddKeys(sendersecrect)
						.Send(receiveraddress, amount)
						.SubtractFees()
						.SetChange(sendersecrect)
						.SendEstimatedFees(rate)
						.BuildTransaction(true);
					rpc.SendRawTransaction(signedTx);

					var trans = rpc.GetRawTransaction(signedTx.GetHash());
					await RefreshCoin();
					// send to bench32

					signedTx = builder
					.AddCoins(coin)
					.AddKeys(sendersecrect)
					.Send(BitcoinAddress.Create(receiversewitbench32, network), amount)
					.SubtractFees()
					.SetChange(sendersecrect)
					.SendEstimatedFees(rate)
					.BuildTransaction(true);
					rpc.SendRawTransaction(signedTx);

					trans = rpc.GetRawTransaction(signedTx.GetHash());
					await RefreshCoin();
					// send to sewitp2sh

					signedTx = builder
					.AddCoins(coin)
					.AddKeys(sendersecrect)
					.Send(BitcoinAddress.Create(receiversewitp2sh, network), amount)
					.SubtractFees()
					.SetChange(sendersecrect)
					.SendEstimatedFees(rate)
					.BuildTransaction(true);
					rpc.SendRawTransaction(signedTx);

					trans = rpc.GetRawTransaction(signedTx.GetHash());
					await RefreshCoin();
					// send to tabroot
					signedTx = builder
					.AddCoins(coin)
					.AddKeys(sendersecrect)
					.Send(BitcoinAddress.Create(receivertabroot, network), amount)
					.SubtractFees()
					.SetChange(sendersecrect)
					.SendEstimatedFees(rate)
					.BuildTransaction(true);
					rpc.SendRawTransaction(signedTx);

					trans = rpc.GetRawTransaction(signedTx.GetHash());


					Console.WriteLine(signedTx.GetHash());

				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}




		}

		[Fact]
		[Trait("UnitTest", "UnitTest")]
		public async Task CanGuessRedeemScriptWithInputKeys()
		{
			using (var nodebuilder = NodeBuilderEx.Create())
			{
				try
				{


					var network = nodebuilder.Network;
					var rpc = nodebuilder.CreateNode().CreateRPCClient();
					nodebuilder.StartAll();

					rpc.Generate(102);

					BitcoinSecret sendersecrect = network.CreateBitcoinSecret(senderaddresssec);
					BitcoinAddress receiveraddress = BitcoinAddress.Create(receiverlegecy, network);

					
					var amount = new Money(25, MoneyUnit.BTC);
					uint256 id = null;
					Transaction tx = null;
					ICoin[] coin = null;
					ICoin[] coin2 = null;
					Transaction tx2 = null;
					TransactionBuilder builder = null;
					var rate = new FeeRate(Money.Satoshis(1), 1);
					uint256 id2 = null;
					// send to from lagecy
					async Task RefreshCoin()
					{
						id = await rpc.SendToAddressAsync(BitcoinAddress.Create(senderlegecy, network), Money.Coins(10));
						tx = await rpc.GetRawTransactionAsync(id);

						id2 = await rpc.SendToAddressAsync(BitcoinAddress.Create(senderlegecy, network), Money.Coins(20));
						tx2 = await rpc.GetRawTransactionAsync(id2);

						coin = tx.Outputs.AsCoins().Where(o => o.ScriptPubKey == BitcoinAddress.Create(senderlegecy, network).ScriptPubKey).ToArray();
					
						coin2 = tx2.Outputs.AsCoins().Where(o => o.ScriptPubKey == BitcoinAddress.Create(senderlegecy, network).ScriptPubKey).ToArray();

						builder = network.CreateTransactionBuilder(0);
					}
					await RefreshCoin();

					//Điều này cung cấp cho bạn một địa chỉ Bech32 (hiện không thực sự tương thích trong ví, vì vậy bạn cần chuyển đổi nó thành P2SH)
					var address = sendersecrect.PubKey.WitHash.GetAddress(network);
					var p2sh = (BitcoinScriptAddress)address.ScriptPubKey.Hash.GetAddress(network);
					//p2sh is now an interoperable P2SH segwit address

					//// Đối với chi tiêu, nó hoạt động giống như P2SH bình thường
					//// Bạn cần lấy ScriptCoin, RedeemScript của đồng xu tập lệnh của bạn phải là k.PubKey.WitHash.ScriptPubKey.
					//var coins =
					//	//Get coins from any block explorer.
					//	GetCoins(p2sh)
					//	//Nobody knows your redeem script, so you add here the information
					//	//This line is actually optional since 4.0.0.38, as the TransactionBuilder is smart enough to figure out
					//	//the redeems from the keys added by AddKeys.
					//	//However, explicitly having the redeem will make code more easy to update to other payment like 2-2
					//	//.Select(c => c.ToScriptCoin(k.PubKey.WitHash.ScriptPubKey))

					//	//Không ai biết tập lệnh đổi thưởng của bạn, vì vậy bạn thêm thông tin vào đây
					//	// Dòng này thực sự là tùy chọn kể từ 4.0.0.38, vì TransactionBuilder đủ thông minh để tìm ra
					//	// phần thưởng từ các khóa được thêm bởi AddKeys.
					//	//Tuy nhiên, rõ ràng việc đổi quà sẽ giúp mã dễ cập nhật hơn cho các khoản thanh toán khác như 2-2
					//	//.Select(c => c.ToScriptCoin(k.PubKey.WitHash.ScriptPubKey))
					//	.ToArray();


					//TransactionBuilder builder = network.CreateTransactionBuilder();
					ICoin[] inputs = coin.Union(coin2).ToArray();
					builder.AddCoins(inputs);
					builder.AddKeys(sendersecrect);
					builder.Send(receiveraddress, amount);
					builder.SendFees(Money.Satoshis(587));
					builder.SetChange(sendersecrect);
					var signedTx = builder.BuildTransaction(true);
					bool _verifyed = builder.Verify(signedTx);
					if (_verifyed)
					{
						var idtx = rpc.SendRawTransaction(signedTx.ToBytes());
						var trans = rpc.GetRawTransaction(signedTx.GetHash());

						inputs = trans.Outputs.AsCoins().Where(o => o.ScriptPubKey == BitcoinAddress.Create(senderlegecy, network).ScriptPubKey).ToArray();
					}

					// resend

					builder = network.CreateTransactionBuilder(0);
					builder.AddCoins(inputs);
					builder.AddKeys(sendersecrect);
					builder.Send(receiveraddress, Money.Coins(3));
					builder.SendFees(Money.Satoshis(587));
					builder.SetChange(sendersecrect);
					var signedTx2 = builder.BuildTransaction(true);
					bool _verifyed2 = builder.Verify(signedTx2);
					if (_verifyed2)
					{
						var idtx2 = rpc.SendRawTransaction(signedTx2.ToBytes());
						var trans2 = rpc.GetRawTransaction(signedTx2.GetHash());
					}


				}
				catch (Exception ex)
				{
				}

			}


		}
		private Coin[] GetCoins(BitcoinScriptAddress p2sh)
		{
			return new Coin[] {
				new Coin(new uint256(Enumerable.Range(0, 32).Select(i => (byte)0xaa).ToArray()), 0, Money.Coins(2.0m), p2sh.ScriptPubKey)

			};
		}
		[Fact]
		[Trait("Core", "Core")]
		public void QuickTestsOnKeyIdBytes()
		{
			var a = new KeyId("93e5d305cad2588d5fb254065fe48ce446028ba3");
			var b = new ScriptId("93e5d305cad2588d5fb254065fe48ce446028ba3");
			var b2 = new WitKeyId("93e5d305cad2588d5fb254065fe48ce446028ba3");
			Assert.Equal(a.ToString(), b.ToString());
			Assert.Equal(a.ToString(), b2.ToString());
			Assert.Equal("93e5d305cad2588d5fb254065fe48ce446028ba3", a.ToString());
			var bytes = Encoders.Hex.DecodeData("93e5d305cad2588d5fb254065fe48ce446028ba3");
			Assert.True(StructuralComparisons.StructuralComparer.Compare(a.ToBytes(), b.ToBytes()) == 0);
			Assert.True(StructuralComparisons.StructuralComparer.Compare(b2.ToBytes(), b.ToBytes()) == 0);

			var c = new KeyId(bytes);
			var d = new ScriptId(bytes);
			var d2 = new WitKeyId(bytes);
			Assert.Equal(a, c);
			Assert.Equal(d, b);
			Assert.Equal(b2, d2);
			Assert.Equal(c.ToString(), d.ToString());
			Assert.Equal(c.ToString(), d2.ToString());
			Assert.Equal("93e5d305cad2588d5fb254065fe48ce446028ba3", c.ToString());
			Assert.True(StructuralComparisons.StructuralComparer.Compare(c.ToBytes(), d.ToBytes()) == 0);
			Assert.True(StructuralComparisons.StructuralComparer.Compare(c.ToBytes(), d2.ToBytes()) == 0);

			var e = new WitScriptId("93e5d305cad2588d5fb254065fe48ce446028ba380e6ee663baea9cd10550089");
			Assert.Equal("93e5d305cad2588d5fb254065fe48ce446028ba380e6ee663baea9cd10550089", e.ToString());
			bytes = Encoders.Hex.DecodeData("93e5d305cad2588d5fb254065fe48ce446028ba380e6ee663baea9cd10550089");
			var e2 = new WitScriptId(bytes);
			Assert.Equal(e, e2);
		}
	}
}

