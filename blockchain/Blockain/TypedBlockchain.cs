﻿using System.Collections;
using System.Text.Json;
using blockchain.Blockain.Rules;

namespace blockchain.Blockain;

record TypedBlock<T>(string Hash, string ParentHash, string RawData, T Data)
{
	public static TypedBlock<T> FromLowLevel(Block block)
	{
		return new TypedBlock<T>(block.Hash, block.ParentHash, block.Data, JsonSerializer.Deserialize<T>(block.Data));
	}
}
class TypedBlockchain<T> : IEnumerable<TypedBlock<T>>
{
	private readonly Blockhain _blockchain;
	private readonly IRule<T>[] _rules;

	public TypedBlockchain(Blockhain blockchain, params IRule<T>[] rules)
	{
		_blockchain = blockchain;
		_rules = rules;
	}

	public void AddBlock(TypedBlock<T> typedBlock)
	{
		foreach (var rule in _rules)
		{
			rule.Execute(this, typedBlock);
		}

		var block = _blockchain.BuildBlock(typedBlock.RawData);
		_blockchain.AddBlock(block);
	}

	public TypedBlock<T> BuildBlock(T data)
	{
		var dataStr = JsonSerializer.Serialize(data);
		var block = _blockchain.BuildBlock(dataStr);
		var typedBlock = new TypedBlock<T>(block.Hash, block.ParentHash, dataStr, data);
		return typedBlock;
	}

	public IEnumerator<TypedBlock<T>> GetEnumerator() =>
		_blockchain.Select(TypedBlock<T>.FromLowLevel)
			.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}