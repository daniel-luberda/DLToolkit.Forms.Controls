namespace DLToolkit.Forms.Controls
{
	/// <summary>
	/// ISmartObservableCollection.
	/// </summary>
	public interface IFlowObservableCollection
	{
		/// <summary>
		/// Start Batch (update data).
		/// </summary>
		void BatchStart();

		/// <summary>
		/// End Batch (update data).
		/// </summary>
		void BatchEnd();

		/// <summary>
		/// Cancel Batch (update data).
		/// </summary>
		void BatchCancel();
	}
}