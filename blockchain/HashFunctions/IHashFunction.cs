﻿namespace blockchain.HashFunctions;

public interface IHashFunction
{
	public string GetHash(string data);
}