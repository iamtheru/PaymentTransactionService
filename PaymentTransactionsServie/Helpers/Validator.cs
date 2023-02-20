using PaymentTransactionsServie.Helpers;
using System.Collections.Generic;

namespace PaymentTransactionsServie
{
	internal class Validator
	{
		public bool Validate(string str, ref int counter)
		{
			counter++;

			if (str.Length == 0)
				return false;

			string address;
			var formatted = str.CuttAddressAndFormat(out address);

			if (!ValidateAddress(address))
				return false;

			if (!CheckIsNotEmpty(formatted))
				return false;

			if (!decimal.TryParse(formatted[2], out _) &&
				!long.TryParse(formatted[4], out _))
				return false;

			return true;
		}

		private bool ValidateAddress(string address)
		{

			var addressParts = address.Split(',');

			if (addressParts.Length != 3)
				return false;

			if (!CheckIsNotEmpty(addressParts))
				return false;

			var streetParts = addressParts[1].TrimStart().Split(' ') ;

			if (!int.TryParse(streetParts[1], out _) || !int.TryParse(addressParts[2], out _))
				return false;
			

			return true;
		}

		private bool CheckIsNotEmpty (IEnumerable<string> parts)
		{
			foreach (var item in parts)
			{
				if (item.Length == 0 || item == " ")
				return false;
			}

			return true;
		}
	}
}