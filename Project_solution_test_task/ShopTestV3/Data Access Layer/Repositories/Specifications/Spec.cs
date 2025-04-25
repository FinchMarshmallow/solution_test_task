using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayerDataAccess.Repositories.Specification
{
	// я спёр это отсюда: https://github.com/denis-tsv/AutoFilter/blob/master/AutoFilter/Spec.cs
	public class Spec<T>
	{
		private readonly Func<T, bool> predicate;

		public Spec(Func<T, bool> predicate)
		{
			this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
		}

		public bool IsSatisfiedBy(T item)
		{
			return predicate(item);
		}

		public static implicit operator Func<T, bool>(Spec<T> spec)
		{
			return spec.predicate;
		}

		public Spec<T> And(Spec<T> other) => new Spec<T>(x => predicate(x) && other.predicate(x));
		public Spec<T> Or(Spec<T> other) => new Spec<T>(x => predicate(x) || other.predicate(x));
		public Spec<T> Not() => new Spec<T>(x => !predicate(x));
	}
}
