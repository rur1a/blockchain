﻿using System.Collections;
using System.Text.Json;

record TypedBlock<T>(string Hash, string ParentHash, string RawData, T Data);
class TypedBlockchain<T> : IEnumerable<TypedBlock<T>>
{
	private readonly Blockhain _blockchain;
	public TypedBlockchain(Blockhain blockchain)
	{
		_blockchain = blockchain;
	}

	public void AddBlock(T data)
	{
		_blockchain.AddBlock(JsonSerializer.Serialize(data));
	}

	public IEnumerator<TypedBlock<T>> GetEnumerator() =>
		_blockchain.Select(x => 
				new TypedBlock<T>(x.Hash, x.ParentHash, x.Data, JsonSerializer.Deserialize<T>(x.Data)))
			.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}