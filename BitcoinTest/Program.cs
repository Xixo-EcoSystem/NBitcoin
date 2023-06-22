// See https://aka.ms/new-console-template for more information
using NBitcoin;
using NBitcoin.RPC;

Console.WriteLine("Hello, World!");
var network = Network.Main;
var authenstring = "rootbtc:bitcoin@xbk";
var hoststring = "8.209.97.98:8332";
NBitcoin.RPC.RPCClient rPCClient = new NBitcoin.RPC.RPCClient(authenstring, hoststring, Network.Main);
//NBitcoin.RPC.RestClient restClient = new NBitcoin.RPC.RestClient(new Uri(hoststring), Network.Main);
NBitcoin.RPCTransactionRepository repository = new RPCTransactionRepository(rPCClient);
var info = rPCClient.GetBlockchainInfo();
Console.WriteLine(info.Blocks);
string senderaddress = "bc1qtf5ghqu2t3muamk94n350xavvykszg042ts9hl";
string privatetestnet = "dress voyage game special dash just middle kitten cake harbor text stage";

while (true)
{
	NBitcoin.BitcoinAddress bitcoinAddress = NBitcoin.BitcoinAddress.Create(senderaddress, network);
	var sender = new NBitcoin.Mnemonic(privatetestnet, Wordlist.English);

	var keyadd = sender.DeriveExtKey();

	// get

	string pubkey = keyadd.GetPublicKey().GetAddress(ScriptPubKeyType.Legacy, Network.Main).ToString();

	string pubkeywithnit = keyadd.GetPublicKey().GetAddress(ScriptPubKeyType.Segwit,Network.Main).ToString();
	string pubkeywithp2sh = keyadd.GetPublicKey().GetAddress(ScriptPubKeyType.SegwitP2SH, Network.Main).ToString();
	string pubkeytap = keyadd.GetPublicKey().GetAddress(ScriptPubKeyType.TaprootBIP86, Network.Main).ToString();




	//	var infovalue = await rPCClient.SendCommandAsync(RPCOperations.getblockchaininfo);

	NBitcoin.RPC.RestClient restClient = new RestClient(new Uri("https://btc.getblock.io/testnet/"),Network.Main);
	restClient.HttpClient.DefaultRequestHeaders.Add("x-api-key", "2ede8ab6-98b5-48c0-b7f0-22063b9e240c");


	//restClient.GetUnspentOutputsAsync()



	//   var transaction1 = restClient.GetTransaction(new uint256("b6f6991d03df0e2e04dafffcd6bc418aac66049e2cd74b80f14ac86db1e3f0da"));


	//NBitcoin.RPC.RPCClient getblockrpcclient = new NBitcoin.RPC.RPCClient(authenstring, "https://btc.getblock.io/mainnet/", Network.Main);
	//getblockrpcclient.HttpClient.DefaultRequestHeaders.Add("x-api-key", "2ede8ab6-98b5-48c0-b7f0-22063b9e240c");

	////var bestblock = getblockrpcclient.GetBestBlockHash();
	////var blockcurrent = getblockrpcclient.GetBlock(154598);
	//var transaction = getblockrpcclient.GetRawTransactionInfo(new uint256("b6f6991d03df0e2e04dafffcd6bc418aac66049e2cd74b80f14ac86db1e3f0da"));

	

	//var transactions = getblockrpcclient.GetTransactions(new uint256(""));

	//foreach(var item in transactions)
	//{
	//	var itemcheck = item.Check();

	//}

	//var money = await rPCClient.GetBalanceAsync();


	//var txInfo = rPCClient.GetRawTransactionInfo(new uint256(""));
	//txInfo.Confirmations;


}



