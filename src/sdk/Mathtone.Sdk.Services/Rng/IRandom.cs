using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathtone.Sdk.Services.Rng {
	public interface IRandom {
		int Next();
		int Next(int maxValue);
		double NextDouble();
		byte[] GetBytes(int values);
		void NextBytes(byte[] values);
	}
}
