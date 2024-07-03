var sha = new SHA256Hash();
var data = Enumerable.Range(0,10);
var blockchain = new Blockhain(sha);
foreach (var item in data)
{
	blockchain.AddBlock(item.ToString());
}

