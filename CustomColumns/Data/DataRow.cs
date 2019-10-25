
namespace CustomColumns.Data
{
	/// <summary>
	/// Represents a virtual row within the datasource (which is represented as columns)
	/// NOTE: DO NOT USE THIS CLASS - it is for binding purposes only
	/// </summary>
	/// <typeparam name="R">Match that used on the DataSource</typeparam>
	/// <typeparam name="C">Match that used on the DataSource</typeparam>
	public class DataRow<R, C>
		where R : struct
		where C : new()
	{
		/// <summary>
		/// Class for meta data stored per row
		/// </summary>
		public struct MetaDataContainer<S> where S : struct
		{
			public Key Key;
			public S Data;
		}

		private readonly DataSource<R, C> _source;

		/// <summary>
		/// The index of the row in the data source
		/// </summary>
		public readonly int RowIndex;

		internal DataRow(DataSource<R, C> source, int row)
		{
			_source = source;
			RowIndex = row;
		}

		/// <summary>
		/// Max possible columns (The number of properties below)
		/// </summary>
#if DEBUG
		public const int MaxColumns = 100;
#else
		public const int MaxColumns = 70;
#endif
		//Need MaxColumns of these
		//This must be in ASCII sortable order
		public object c00 { get { return _source[RowIndex, 0]; } }
		public object c01 { get { return _source[RowIndex, 1]; } }
		public object c02 { get { return _source[RowIndex, 2]; } }
		public object c03 { get { return _source[RowIndex, 3]; } }
		public object c04 { get { return _source[RowIndex, 4]; } }
		public object c05 { get { return _source[RowIndex, 5]; } }
		public object c06 { get { return _source[RowIndex, 6]; } }
		public object c07 { get { return _source[RowIndex, 7]; } }
		public object c08 { get { return _source[RowIndex, 8]; } }
		public object c09 { get { return _source[RowIndex, 9]; } }

		public object c10 { get { return _source[RowIndex, 10]; } }
		public object c11 { get { return _source[RowIndex, 11]; } }
		public object c12 { get { return _source[RowIndex, 12]; } }
		public object c13 { get { return _source[RowIndex, 13]; } }
		public object c14 { get { return _source[RowIndex, 14]; } }
		public object c15 { get { return _source[RowIndex, 15]; } }
		public object c16 { get { return _source[RowIndex, 16]; } }
		public object c17 { get { return _source[RowIndex, 17]; } }
		public object c18 { get { return _source[RowIndex, 18]; } }
		public object c19 { get { return _source[RowIndex, 19]; } }

		public object c20 { get { return _source[RowIndex, 20]; } }
		public object c21 { get { return _source[RowIndex, 21]; } }
		public object c22 { get { return _source[RowIndex, 22]; } }
		public object c23 { get { return _source[RowIndex, 23]; } }
		public object c24 { get { return _source[RowIndex, 24]; } }
		public object c25 { get { return _source[RowIndex, 25]; } }
		public object c26 { get { return _source[RowIndex, 26]; } }
		public object c27 { get { return _source[RowIndex, 27]; } }
		public object c28 { get { return _source[RowIndex, 28]; } }
		public object c29 { get { return _source[RowIndex, 29]; } }

		public object c30 { get { return _source[RowIndex, 30]; } }
		public object c31 { get { return _source[RowIndex, 31]; } }
		public object c32 { get { return _source[RowIndex, 32]; } }
		public object c33 { get { return _source[RowIndex, 33]; } }
		public object c34 { get { return _source[RowIndex, 34]; } }
		public object c35 { get { return _source[RowIndex, 35]; } }
		public object c36 { get { return _source[RowIndex, 36]; } }
		public object c37 { get { return _source[RowIndex, 37]; } }
		public object c38 { get { return _source[RowIndex, 38]; } }
		public object c39 { get { return _source[RowIndex, 39]; } }

		public object c40 { get { return _source[RowIndex, 40]; } }
		public object c41 { get { return _source[RowIndex, 41]; } }
		public object c42 { get { return _source[RowIndex, 42]; } }
		public object c43 { get { return _source[RowIndex, 43]; } }
		public object c44 { get { return _source[RowIndex, 44]; } }
		public object c45 { get { return _source[RowIndex, 45]; } }
		public object c46 { get { return _source[RowIndex, 46]; } }
		public object c47 { get { return _source[RowIndex, 47]; } }
		public object c48 { get { return _source[RowIndex, 48]; } }
		public object c49 { get { return _source[RowIndex, 49]; } }

		public object c50 { get { return _source[RowIndex, 50]; } }
		public object c51 { get { return _source[RowIndex, 51]; } }
		public object c52 { get { return _source[RowIndex, 52]; } }
		public object c53 { get { return _source[RowIndex, 53]; } }
		public object c54 { get { return _source[RowIndex, 54]; } }
		public object c55 { get { return _source[RowIndex, 55]; } }
		public object c56 { get { return _source[RowIndex, 56]; } }
		public object c57 { get { return _source[RowIndex, 57]; } }
		public object c58 { get { return _source[RowIndex, 58]; } }
		public object c59 { get { return _source[RowIndex, 59]; } }

		public object c60 { get { return _source[RowIndex, 60]; } }
		public object c61 { get { return _source[RowIndex, 61]; } }
		public object c62 { get { return _source[RowIndex, 62]; } }
		public object c63 { get { return _source[RowIndex, 63]; } }
		public object c64 { get { return _source[RowIndex, 64]; } }
		public object c65 { get { return _source[RowIndex, 65]; } }
		public object c66 { get { return _source[RowIndex, 66]; } }
		public object c67 { get { return _source[RowIndex, 67]; } }
		public object c68 { get { return _source[RowIndex, 68]; } }
		public object c69 { get { return _source[RowIndex, 69]; } }

#if DEBUG

		public object c70 { get { return _source[RowIndex, 60]; } }
		public object c71 { get { return _source[RowIndex, 61]; } }
		public object c72 { get { return _source[RowIndex, 62]; } }
		public object c73 { get { return _source[RowIndex, 63]; } }
		public object c74 { get { return _source[RowIndex, 64]; } }
		public object c75 { get { return _source[RowIndex, 65]; } }
		public object c76 { get { return _source[RowIndex, 66]; } }
		public object c77 { get { return _source[RowIndex, 67]; } }
		public object c78 { get { return _source[RowIndex, 68]; } }
		public object c79 { get { return _source[RowIndex, 69]; } }

		public object c80 { get { return _source[RowIndex, 60]; } }
		public object c81 { get { return _source[RowIndex, 61]; } }
		public object c82 { get { return _source[RowIndex, 62]; } }
		public object c83 { get { return _source[RowIndex, 63]; } }
		public object c84 { get { return _source[RowIndex, 64]; } }
		public object c85 { get { return _source[RowIndex, 65]; } }
		public object c86 { get { return _source[RowIndex, 66]; } }
		public object c87 { get { return _source[RowIndex, 67]; } }
		public object c88 { get { return _source[RowIndex, 68]; } }
		public object c89 { get { return _source[RowIndex, 69]; } }

		public object c90 { get { return _source[RowIndex, 60]; } }
		public object c91 { get { return _source[RowIndex, 61]; } }
		public object c92 { get { return _source[RowIndex, 62]; } }
		public object c93 { get { return _source[RowIndex, 63]; } }
		public object c94 { get { return _source[RowIndex, 64]; } }
		public object c95 { get { return _source[RowIndex, 65]; } }
		public object c96 { get { return _source[RowIndex, 66]; } }
		public object c97 { get { return _source[RowIndex, 67]; } }
		public object c98 { get { return _source[RowIndex, 68]; } }
		public object c99 { get { return _source[RowIndex, 69]; } }

#endif


	}
}
